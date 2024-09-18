using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using System.Collections.Generic;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface IDentalServiceReceptInventorySettingService : IEntityService<DentalServiceReceptInventorySetting>
    {
        bool UpdateReceptInventorySettingItems(int dentalServiceId, IEnumerable<int> receptInventorySettings);
    }
}
