using System.Threading.Tasks;
using Google.Protobuf;

namespace Astandy.Services
{
    public abstract class Service
    {
        protected readonly StandClient Client;

        protected Service(StandClient client)
        {
            Client = client;
        }

        /// <summary>
        /// Хелпер для спрощення виклику RPC-методів всередині похідних сервісів
        /// </summary>
        protected Task<TResponse> CallAsync<TRequest, TResponse>(uint code, TRequest request, MessageParser<TResponse> parser, bool isEncrypted = false)
                    where TRequest : IMessage<TRequest>
                    where TResponse : IMessage<TResponse>
        {
            // Передаємо пусті рядки для serviceName і methodName
            return Client.CallRpcAsync(code, request, parser, "", "", isEncrypted);
        }
    }
}