using System;
using System.Security.Cryptography;
using System.Text;

namespace Edent.Api.Services.CommonServices
{
    public class AesService : IAesService
    {
        public (string key, string iv, string encryptedData) EncryptAes(string textToEncrypt)
        {
            Aes cipher = Aes.Create();
            cipher.KeySize = 256;
            cipher.BlockSize = 128; // используйте 128 для совместимости с AES
            cipher.Padding = PaddingMode.ISO10126;
            cipher.Mode = CipherMode.CBC;
            cipher.GenerateIV();
            cipher.GenerateKey();

            ICryptoTransform t = cipher.CreateEncryptor();
            byte[] textToEncryptInBytes = Encoding.UTF8.GetBytes(textToEncrypt);
            byte[] result = t.TransformFinalBlock(textToEncryptInBytes, 0, textToEncryptInBytes.Length);

            string key = Convert.ToBase64String(cipher.Key);
            string iv = Convert.ToBase64String(cipher.IV);
            string encryptedData = Convert.ToBase64String(result);

            return (key, iv, encryptedData);
        }

        public string DecryptAes(string encryptedText, string key, string iv)
        {
            Aes cipher = Aes.Create();
            cipher.KeySize = 256;
            cipher.BlockSize = 128;
            cipher.Padding = PaddingMode.ISO10126;
            cipher.Mode = CipherMode.CBC;
            cipher.Key = Convert.FromBase64String(key);
            cipher.IV = Convert.FromBase64String(iv);

            ICryptoTransform t = cipher.CreateDecryptor();
            byte[] encryptedTextInBytes = Convert.FromBase64String(encryptedText);
            byte[] result = t.TransformFinalBlock(encryptedTextInBytes, 0, encryptedTextInBytes.Length);
            return Encoding.UTF8.GetString(result);
        }
    }
}
