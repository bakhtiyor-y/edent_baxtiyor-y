namespace Edent.Api.Services.CommonServices
{
    public interface IRsaService
    {
        string Decrypt(string encryptedData);

        string Encrypt(string dataToEncrypt);
    }
}
