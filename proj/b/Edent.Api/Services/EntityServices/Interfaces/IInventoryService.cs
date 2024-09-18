using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface IInventoryService : IEntityService<Inventory>
    {
        void UpdateInventoriesStock(int receptId);
        void UpdateInventoriesStockOnOutcome(int outcomeId);
        void UpdateInventoriesStockOnIncome(int incomId);
    }
}
