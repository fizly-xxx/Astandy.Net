using Astandy.Enums;
using Astandy.Types;

namespace Astandy.Updates
{
    // Двокрапка (:) означає наслідування (в Python це було б class Connected(Update):)
    public class Connected : Update
    {
        public Connected()
        {
            // Коли створюється об'єкт Connected, він автоматично отримує подію CONNECT
            Event = BaseEvent.CONNECT;
        }
    }
}