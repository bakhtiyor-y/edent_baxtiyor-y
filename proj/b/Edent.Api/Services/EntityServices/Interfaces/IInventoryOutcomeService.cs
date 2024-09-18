using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface IInventoryOutcomeService : IEntityService<InventoryOutcome>
    {

        bool ProvideReceptOutcome(int receptId);
    }
}
