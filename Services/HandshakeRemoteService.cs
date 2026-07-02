using System.Threading.Tasks;
using Astandy.Base;

namespace Astandy.Services
{
    public class HandshakeRemoteService : Service
    {
        public HandshakeRemoteService(StandClient client) : base(client) { }

        public async Task<ADHHEGBDFBEBGGC> HandshakeAsync(string token)
        {
            var request = new GACHFBFBBEHDAAD { GFCCADHDDFAFHFE = token };
            // Використовуємо універсальний CallRpcAsync клієнта, він сам зашифрує і дешифрує
            return await Client.CallRpcAsync(3, request, ADHHEGBDFBEBGGC.Parser, isEncrypted: true);
        }
    }
}