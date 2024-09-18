using Edent.Api.Infrastructure.Entities;
using System.Threading.Tasks;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface IUserResolverService
    {
        int? CurrentUserId { get; }
        Task<User> GetUser();
    }
}
