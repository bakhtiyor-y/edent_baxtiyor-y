using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface IEmployeeService : IEntityService<Employee>
    {
        Employee GetByUserId(int userId);
    }
}
