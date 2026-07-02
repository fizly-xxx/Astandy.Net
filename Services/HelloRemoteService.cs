using System.Security.Cryptography;
using System.Threading.Tasks;
using Google.Protobuf;
using Astandy.Base;
using Astandy.Exceptions;

namespace Astandy.Services
{
    public class HelloRemoteService : Service
    {
        public HelloRemoteService(StandClient client) : base(client) { }

        public async Task<bool> HelloAsync()
        {
            using var rsaKey = RSA.Create(1024);
            var rsaParams = rsaKey.ExportParameters(true);

            byte[] iv = new byte[16];
            RandomNumberGenerator.Fill(iv);

            // Логіка паддінгу ключів
            byte[] modulus = rsaParams.Modulus!;
            if (modulus.Length < 128) { var temp = new byte[128]; modulus.CopyTo(temp, 128 - modulus.Length); modulus = temp; }
            byte[] exponent = rsaParams.Exponent!;
            if (exponent.Length < 3) { var temp = new byte[3]; exponent.CopyTo(temp, 3 - exponent.Length); exponent = temp; }

            var request = new CHGACEEHFADEDHH
            {
                DCAGCDFCHBBDCDB = ByteString.CopyFrom(modulus),
                GABAFEDDDBEGGAE = ByteString.CopyFrom(exponent),
                CGCGBGBEGCADABH = ByteString.CopyFrom(iv)
            };

            var response = await Client.CallRpcAsync(1, request, BBFGDBCEGCBFBEE.Parser, isEncrypted: true);

            byte[] encryptedKey = response.FBHHACGFCHFEEED.ToByteArray();
            byte[] decryptedKey = rsaKey.Decrypt(encryptedKey, RSAEncryptionPadding.Pkcs1);

            Client.Cipher.NewAesCipher(decryptedKey, iv);
            return true;
        }
    }
}