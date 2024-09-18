namespace Edent.Api.Services.CommonServices
{
    public interface IAesService
    {
        (string key, string iv, string encryptedData) EncryptAes(string textToEncrypt);

        string DecryptAes(string encryptedText, string key, string iv);
    }
}
