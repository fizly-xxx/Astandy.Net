using Astandy.Events;

namespace Astandy
{
    public partial class StandClient
    {
        public EventDispatcher EventDispatcher { get; } = new EventDispatcher();

        public void DispatchEvent(Astandy.Base.Event evt)
        {
            if (evt?.Params?.One != null)
            {
                byte[] payload = evt.Params.One.ToByteArray();
                // Змінено на DispatchAsync
                _ = EventDispatcher.DispatchAsync((uint)evt.Code, payload);
            }
        }
    }
}