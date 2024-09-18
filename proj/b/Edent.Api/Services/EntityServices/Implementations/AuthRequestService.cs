using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Enums;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels.Account;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class AuthRequestService : EntityService<AuthRequest>, IAuthRequestService
    {
        public AuthRequestService(IRepository<AuthRequest> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

        public AuthRequest GetByAuthCodeViewModel(AuthCodeViewModel viewModel)
        {
            var result = Query()
                .Where(f => f.UserName.Equals(viewModel.UserName)
                                    && f.Code == viewModel.Code
                                    && f.AuthRequestType == viewModel.AuthRequestType)
                .OrderByDescending(o => o.ExpireDate)
                .FirstOrDefault();

            return result;
        }

        public bool ValidateRequestToken(string userName, string token, AuthRequestType authRequestType)
        {
            var result = Query()
                .FirstOrDefault(f => f.UserName.Equals(userName)
                                    && f.RequestToken.Equals(token)
                                    && f.AuthRequestType == authRequestType
                                    && f.IsValidated);

            return result != null;
        }
    }
}
