using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Microsoft.AspNetCore.Http;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface IToothService : IEntityService<Tooth>
    {
        string ChangeImage(IFormFile _file, int toothId);
    }
}
