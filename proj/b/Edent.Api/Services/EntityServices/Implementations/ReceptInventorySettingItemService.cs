using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class ReceptInventorySettingItemService : EntityService<ReceptInventorySettingItem>, IReceptInventorySettingItemService
    {
        public ReceptInventorySettingItemService(IRepository<ReceptInventorySettingItem> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

        public bool UpdateReceptInventorySettingItems(int receptInventorySettingId, IEnumerable<ReceptInventorySettingItemViewModel> viewModels)
        {
            var settingItems = Query()
               .AsNoTracking()
               .Where(f => f.ReceptInventorySettingId == receptInventorySettingId);

            var itemsForCreate = viewModels.Where(w => w.Id == 0);
            foreach (var item in itemsForCreate)
            {
                item.ReceptInventorySettingId = receptInventorySettingId;
                Repository.Add(Mapper.Map<ReceptInventorySettingItem>(item));
            }

            var itemsForUpdate = viewModels.Where(w => w.Id > 0);
            foreach (var item in settingItems)
            {
                var itemForUpdate = itemsForUpdate.FirstOrDefault(f => f.Id == item.Id);
                if (itemForUpdate != null)
                {
                    item.InventoryId = itemForUpdate.InventoryId;
                    item.Quantity = itemForUpdate.Quantity;
                    Repository.Edit(item);
                }
                else
                {
                    Repository.Remove(item);
                }
            }
            return Repository.SaveChanges();
        }
    }
}
