namespace Astandy.Types
{
    public class GMessage
    {
        // Властивості з get; set; дозволяють читати та записувати ці значення
        public int Code { get; set; }
        public byte[] Payload { get; set; }

        // Це конструктор (аналог __init__ у Python)
        public GMessage(int code, byte[] payload)
        {
            Code = code;
            Payload = payload;
        }
    }
}