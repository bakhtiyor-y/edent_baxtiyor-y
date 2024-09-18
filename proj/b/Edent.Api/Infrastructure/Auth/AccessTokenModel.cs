namespace Edent.Api.Infrastructure.Auth
{
    public class AccessTokenModel
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public string UserName { get; }
        public int ExpiresIn { get; }

        public AccessTokenModel(string userName, string accessToken, string resfreshToken, int expiresIn)
        {
            UserName = userName;
            AccessToken = accessToken;
            RefreshToken = resfreshToken;
            ExpiresIn = expiresIn;
        }
    }
}
