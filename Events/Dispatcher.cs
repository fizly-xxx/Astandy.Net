using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf;

namespace Astandy
{
    /// <summary>
    /// Інтерфейс для створення ізольованих обробників подій (SOLID: Single Responsibility)
    /// </summary>
    public interface IEventHandler<T> where T : IMessage<T>
    {
        Task HandleAsync(T message);
    }

    public class EventDispatcher
    {
        // Зберігаємо обробники у вигляді вхідного масиву байт, щоб розпаковувати їх безпосередньо під час виклику
        private readonly Dictionary<uint, Func<byte[], Task>> _eventHandlers = new();

        /// <summary>
        /// Реєстрація типізованого обробника події
        /// </summary>
        public void RegisterListener<T>(uint eventCode, IEventHandler<T> handler) where T : IMessage<T>, new()
        {
            _eventHandlers[eventCode] = async (payload) =>
            {
                var message = new T();
                message.MergeFrom(payload);
                await handler.HandleAsync(message);
            };
        }

        /// <summary>
        /// Реєстрація через лямбда-вираз (спрощений варіант)
        /// </summary>
        public void RegisterListener<T>(uint eventCode, Func<T, Task> action) where T : IMessage<T>, new()
        {
            _eventHandlers[eventCode] = async (payload) =>
            {
                var message = new T();
                message.MergeFrom(payload);
                await action(message);
            };
        }

        /// <summary>
        /// Виклик обробника події при надходженні пакета від сервера
        /// </summary>
        public async Task DispatchAsync(uint eventCode, byte[] payload)
        {
            if (_eventHandlers.TryGetValue(eventCode, out var handler))
            {
                try
                {
                    await handler(payload);
                }
                catch (Exception ex)
                {
                    // Тут має бути логування помилки обробника події
                    Console.WriteLine($"[Dispatcher Error] Помилка обробки події {eventCode}: {ex.Message}");
                }
            }
        }
    }
}