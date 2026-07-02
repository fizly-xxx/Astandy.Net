using Astandy; // ДОДАНО

namespace Astandy.Events
{
    public interface IListener
    {
        void Register(EventDispatcher dispatcher);
    }
}