using Edent.Api.Infrastructure.Enums;

namespace Edent.Api.ViewModels.Account
{
    public class AuthCodeViewModel
    {
        public string UserName { get; set; }
        public int Code { get; set; }
        public AuthRequestType AuthRequestType { get; set; }
    }
}
