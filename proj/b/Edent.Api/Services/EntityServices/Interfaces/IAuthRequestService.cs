using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Enums;
using Edent.Api.Services.Base;
using Edent.Api.ViewModels.Account;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface IAuthRequestService : IEntityService<AuthRequest>
    {
        AuthRequest GetByAuthCodeViewModel(AuthCodeViewModel viewModel);

        bool ValidateRequestToken(string userName, string token, AuthRequestType authRequestType);
    }
}
