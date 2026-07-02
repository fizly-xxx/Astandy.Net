using System.Threading.Tasks;
using Astandy.Base;

namespace Astandy.Services
{
    public class PlayerRemoteService : Service
    {
        public PlayerRemoteService(StandClient client) : base(client) { }

        public async Task<GetPlayerResponse> MeAsync()
        {
            var request = new GetPlayerRequest();
            // Надіслати RPC код для отримання профілю (наприклад, 201)
            return await CallAsync(201, request, GetPlayerResponse.Parser, isEncrypted: true);
        }
    }
}