namespace Astandy.Types
{
    /// <summary>
    /// Клас для зберігання додаткової інформації про підключення.
    /// </summary>
    public class ConnectionInfo
    {
        // Затримка до сервера в мілісекундах
        public int Ping { get; set; }

        public ConnectionInfo()
        {
            Ping = 0;
        }

        public override string ToString()
        {
            return $"Ping={Ping}ms";
        }
    }
}