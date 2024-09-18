using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class RefreshTokenService : EntityService<RefreshToken>, IRefreshTokenService
    {
        public RefreshTokenService(IRepository<RefreshToken> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

        public bool DeleteByNameAndUserId(string name, int userId)
        {
            RefreshToken entity = Query().AsNoTracking().FirstOrDefault(f => f.Name.Equals(name) && f.UserId == userId);
            if (entity == null)
                return false;

            _repository.Remove(entity);
            return _repository.SaveChanges();
        }
    }
}
