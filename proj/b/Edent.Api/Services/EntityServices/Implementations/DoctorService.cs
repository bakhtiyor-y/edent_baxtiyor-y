using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class DoctorService : EntityService<Doctor>, IDoctorService
    {
        public DoctorService(IRepository<Doctor> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

        public DoctorViewModel GetByIncluding(int id)
        {
            var entity = Query()
                .Include(i => i.User)
                .Include(i => i.Specialization)
                .Include("DoctorInTerms.Term")
                .FirstOrDefault(f => f.Id == id);

            return Mapper.Map<DoctorViewModel>(entity);
        }

        public Doctor GetByUserId(int userId)
        {
            return Query().Include(i => i.User).FirstOrDefault(f => f.UserId == userId);
        }
    }
}
