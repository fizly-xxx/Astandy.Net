using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Astandy
{
    public class AstandyCipher : IDisposable
    {
        private static readonly byte[] START_KEY = Encoding.UTF8.GetBytes("key_abcdefghijkl");
        private static readonly byte[] START_IV = Encoding.UTF8.GetBytes("iv_abcdefghijklm");

        private readonly Aes _aesAlg;
        private ICryptoTransform? _encryptor;
        private ICryptoTransform? _decryptor;

        public AstandyCipher()
        {
            _aesAlg = Aes.Create();
            _aesAlg.Mode = CipherMode.CBC;
            _aesAlg.Padding = PaddingMode.PKCS7;
            NewAesCipher(START_KEY, START_IV);
        }

        public void NewAesCipher(byte[] key, byte[] iv)
        {
            _aesAlg.Key = key;
            _aesAlg.IV = iv;

            _encryptor?.Dispose();
            _decryptor?.Dispose();

            _encryptor = _aesAlg.CreateEncryptor(_aesAlg.Key, _aesAlg.IV);
            _decryptor = _aesAlg.CreateDecryptor(_aesAlg.Key, _aesAlg.IV);
        }

        public byte[] Encrypt(byte[] data)
        {
            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, _encryptor!, CryptoStreamMode.Write);
            csEncrypt.Write(data, 0, data.Length);
            csEncrypt.FlushFinalBlock();
            return msEncrypt.ToArray();
        }

        public byte[] Decrypt(byte[] data)
        {
            using var msDecrypt = new MemoryStream(data);
            using var csDecrypt = new CryptoStream(msDecrypt, _decryptor!, CryptoStreamMode.Read);
            using var msOutput = new MemoryStream();
            csDecrypt.CopyTo(msOutput);
            return msOutput.ToArray();
        }

        public void Dispose()
        {
            _encryptor?.Dispose();
            _decryptor?.Dispose();
            _aesAlg?.Dispose();
        }
    }
}