using Edent.Api.Infrastructure.Security;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Edent.Api.Services.CommonServices
{
    public class RsaService : IRsaService
    {
        public string Decrypt(string encryptedData)
        {
            var csp = new RSACryptoServiceProvider(2048);
            csp.FromXmlString(SecurityConstants.RSAPrivateKey);
            var resultBytes = Convert.FromBase64String(encryptedData);
            var decryptedBytes = csp.Decrypt(resultBytes, false);
            var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
            return decryptedData;
        }

        public string Encrypt(string dataToEncrypt)
        {
            throw new NotImplementedException();
        }
    }
}
