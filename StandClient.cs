using Astandy.Configuration;
using Astandy.Types;
using Google.Protobuf;
using K4os.Compression.LZ4;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Astandy
{
    public partial class StandClient
    {
        // Базові поля
        private readonly StandClientOptions _options;
        private readonly ILogger<StandClient> _logger;
        private string _token;
        // Мережеві поля
        private TcpClient? _tcpClient;
        private SslStream? _sslStream;
        private bool _isRunning;
        private readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _pingSemaphore = new SemaphoreSlim(0, 1);
        private readonly ConcurrentDictionary<string, TaskCompletionSource<Astandy.Base.Response>> _pendingRequests = new();

        private const string SERVER_HOST = "your.server.address"; // Заміни на свій
        private const int SERVER_PORT = 12345; // Заміни на свій

        // Публічні властивості
        public AstandyCipher Cipher { get; } = new AstandyCipher();

        public Services.HelloRemoteService Hello { get; }
        public Services.HandshakeRemoteService Handshake { get; }
        public Services.PlayerRemoteService Player { get; }
        public Services.BoltRemoteService Bolt { get; }

        public StandClient(StandClientOptions options, ILogger<StandClient> logger, string token = "dummy_token")
        {
            _options = options ?? new StandClientOptions();
            _logger = logger;
            _token = token;

            // Ініціалізація сервісів
            Hello = new Services.HelloRemoteService(this);
            Handshake = new Services.HandshakeRemoteService(this);
            Player = new Services.PlayerRemoteService(this);
            Bolt = new Services.BoltRemoteService(this);

            _logger.LogInformation("StandClient ініціалізовано.");
        }

        public async Task ConnectAsync()
        {
            int retryCount = 0;
            while (retryCount <= _options.MaxRetryCount)
            {
                await DisconnectAsync();
                try
                {
                    _tcpClient = new TcpClient();
                    if (!string.IsNullOrEmpty(_options.ProxyHost))
                    {
                        await ConnectViaSocks5Async();
                    }
                    else
                    {
                        await _tcpClient.ConnectAsync(SERVER_HOST, SERVER_PORT);
                    }

                    _sslStream = new SslStream(_tcpClient.GetStream(), false, ValidateServerCertificate, null);
                    await _sslStream.AuthenticateAsClientAsync(SERVER_HOST);

                    _isRunning = true;
                    _ = Task.Run(() => ReadLoopAsync());
                    _logger.LogInformation("[Мережа] Успішно підключено!");
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("[Мережа] Помилка: {Message}", ex.Message);
                    retryCount++;
                    await Task.Delay(2000);
                }
            }
        }

        private async Task ConnectViaSocks5Async()
        {
            if (_tcpClient != null)
            {
                await _tcpClient.ConnectAsync(_options.ProxyHost!, _options.ProxyPort ?? 1080);
            }
        }

        public void StartPingLoop()
        {
            _ = Task.Run(() => PingLoopAsync());
        }

        private async Task PingLoopAsync()
        {
            while (_isRunning && _sslStream != null)
            {
                await SendPayloadAsync(new byte[] { 0x01 });
                if (!await _pingSemaphore.WaitAsync(TimeSpan.FromSeconds(_options.PingTimeout)))
                {
                    _logger.LogWarning("[Мережа] Ping Timeout");
                    await DisconnectAsync();
                    break;
                }
                await Task.Delay(5000);
            }
        }

        public async Task<bool> SendPayloadAsync(byte[] payload)
        {
            if (!_isRunning || _sslStream == null) return false;
            await _sendLock.WaitAsync();
            try
            {
                byte[] lengthBytes = BitConverter.GetBytes(payload.Length);
                if (BitConverter.IsLittleEndian) Array.Reverse(lengthBytes);
                await _sslStream.WriteAsync(lengthBytes, 0, 4);
                await _sslStream.WriteAsync(payload, 0, payload.Length);
                await _sslStream.FlushAsync();
                return true;
            }
            finally { _sendLock.Release(); }
        }

        public async Task<Astandy.Base.Response> SendRequestAsync(uint code, byte[] requestPayload, string serviceName = "", string methodName = "", int timeoutSeconds = 15)
        {
            string uuid = Guid.NewGuid().ToString();
            var tcs = new TaskCompletionSource<Astandy.Base.Response>();
            _pendingRequests.TryAdd(uuid, tcs);

            try
            {
                byte[] msg = Astandy.Utils.Parser.NewMsg(uuid, code, requestPayload, serviceName, methodName);

                bool sent = await SendPayloadAsync(msg);
                if (!sent) throw new Exceptions.AstandyRPCException("Не вдалося відправити дані.");

                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(timeoutSeconds));
                var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

                if (completedTask == timeoutTask)
                    throw new TimeoutException($"Запит {uuid} не отримав відповіді.");

                return await tcs.Task;
            }
            finally
            {
                _pendingRequests.TryRemove(uuid, out _);
            }
        }

        public async Task<TResponse> CallRpcAsync<TRequest, TResponse>(uint code, TRequest request, MessageParser<TResponse> parser, string serviceName = "", string methodName = "", bool isEncrypted = false)
            where TRequest : IMessage<TRequest>
            where TResponse : IMessage<TResponse>
        {
            byte[] payload = request.ToByteArray();
            if (isEncrypted) payload = Cipher.Encrypt(payload);

            var response = await SendRequestAsync(code, payload, serviceName, methodName);

            if (response.Exception != null)
                throw new Exceptions.AstandyRPCException($"Помилка RPC: {response.Exception.Code}");

            byte[] responseBytes = response.Data[0].One.ToByteArray();
            if (isEncrypted) responseBytes = Cipher.Decrypt(responseBytes);

            return parser.ParseFrom(responseBytes);
        }

        public async Task DisconnectAsync()
        {
            _isRunning = false;

            if (_sslStream != null) await _sslStream.DisposeAsync();
            _tcpClient?.Close();
            _sslStream = null;
            _tcpClient = null;

            // Запобігаємо зависанню Task-ів
            foreach (var kvp in _pendingRequests)
            {
                kvp.Value.TrySetCanceled();
            }
            _pendingRequests.Clear();
        }

        private bool ValidateServerCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors) => true;

        private async Task ReadLoopAsync()
        {
            while (_isRunning && _sslStream != null)
            {
                byte[]? header = await ReadExactlyAsync(4);
                if (header == null) break;
                if (BitConverter.IsLittleEndian) Array.Reverse(header);
                int len = BitConverter.ToInt32(header, 0);

                byte[]? msg = await ReadExactlyAsync(len);
                if (msg == null) break;

                if (msg.Length == 1 && msg[0] == 0x01)
                {
                    if (_pingSemaphore.CurrentCount == 0) _pingSemaphore.Release();
                    continue;
                }

                var serverMsg = Astandy.Utils.Parser.ParseResponse(msg);
                await HandleMessageAsync(serverMsg); // ВИПРАВЛЕНО ТУТ: додано await та змінено назву на Async
            }
        }

        private async Task<byte[]?> ReadExactlyAsync(int size)
        {
            byte[] buffer = new byte[size];
            int offset = 0;
            while (offset < size)
            {
                int read = await _sslStream!.ReadAsync(buffer, offset, size - offset);
                if (read == 0) return null;
                offset += read;
            }
            return buffer;
        }

        private async Task HandleMessageAsync(Astandy.Base.ServerMsg serverMsg)
        {
            // ВИПРАВЛЕНО: serverMsg.Responses (множина)
            foreach (var response in serverMsg.Responses)
            {
                if (_pendingRequests.TryGetValue(response.Id, out var tcs))
                {
                    tcs.TrySetResult(response);
                }
                else
                {
                    _logger.LogWarning("[Увага] Отримано відповідь для невідомого запиту: {Id}", response.Id);
                }
            }

            // ВИПРАВЛЕНО: serverMsg.Events (множина)
            foreach (var evt in serverMsg.Events)
            {
                byte[] eventPayload = evt.Params.One.ToByteArray();
                // Звертаємось до правильного EventDispatcher
                await EventDispatcher.DispatchAsync((uint)evt.Code, eventPayload);
            }

            // ВИПРАВЛЕНО: serverMsg.CompressedInstances (множина)
            foreach (var compressed in serverMsg.CompressedInstances)
            {
                byte[] compressedBytes = compressed.Compressed.ToByteArray();
                byte[] decompressedBytes = new byte[compressed.UncompressedSize];

                K4os.Compression.LZ4.LZ4Codec.Decode(
                    compressedBytes, 0, compressedBytes.Length,
                    decompressedBytes, 0, decompressedBytes.Length
                );

                var innerMsg = Astandy.Utils.Parser.ParseResponse(decompressedBytes);
                await HandleMessageAsync(innerMsg);
            }
        }
    }
}