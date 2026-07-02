using System.Threading.Tasks;
using Astandy.Base;

namespace Astandy.Services
{
    public class BoltRemoteService : Service
    {
        public BoltRemoteService(StandClient client) : base(client) { }

        public async Task<SubscribeResponse> SubscribeAsync()
        {
            var request = new SubscribeRequest();
            return await Client.CallRpcAsync(41, request, SubscribeResponse.Parser, isEncrypted: true);
        }
    }
}