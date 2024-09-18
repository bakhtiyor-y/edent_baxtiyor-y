using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class EmployeeService : EntityService<Employee>, IEmployeeService
    {
        public EmployeeService(IRepository<Employee> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

        public Employee GetByUserId(int userId)
        {
            return Query().Include(i => i.User).FirstOrDefault(f => f.UserId == userId);
        }
    }
}
