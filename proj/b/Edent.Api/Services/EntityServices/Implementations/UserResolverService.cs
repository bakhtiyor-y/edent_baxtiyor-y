using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Extensions;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class UserResolverService : IUserResolverService
    {
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor context;
        public UserResolverService(IHttpContextAccessor context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        public async Task<User> GetUser()
        {
            string userName = context.HttpContext.User.GetUserName();
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }
            else
            {
                return await userManager.FindByNameAsync(userName);
            }
        }

        public int? CurrentUserId { get { return context.HttpContext.User.GetUserId(); } }
    }
}
