using Astandy.Enums; // Підключаємо нашу папку Enums, щоб бачити BaseEvent

namespace Astandy.Types
{
    public class Update
    {
        public BaseEvent Event { get; set; }

        // Аналог def __str__(self): з Python
        public override string ToString()
        {
            // GetType().Name автоматично підставить назву класу (наприклад, "Connected")
            return $"{this.GetType().Name}(event={Event})";
        }
    }
}