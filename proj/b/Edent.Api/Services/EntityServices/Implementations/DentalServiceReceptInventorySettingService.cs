using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class DentalServiceReceptInventorySettingService : EntityService<DentalServiceReceptInventorySetting>, IDentalServiceReceptInventorySettingService
    {
        public DentalServiceReceptInventorySettingService(IRepository<DentalServiceReceptInventorySetting> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }


        public bool UpdateReceptInventorySettingItems(int dentalServiceId, IEnumerable<int> receptInventorySettings)
        {
            var settingItems = Query()
               .AsNoTracking()
               .Where(f => f.DentalServiceId == dentalServiceId)
               .ToList();

            var settingItemIds = settingItems.Select(s => s.ReceptInventorySettingId).ToList();


            foreach (var id in settingItemIds)
            {
                if (!receptInventorySettings.Contains(id))
                {
                    var itemForRemove = settingItems.FirstOrDefault(f => f.ReceptInventorySettingId == id && f.DentalServiceId == dentalServiceId);
                    if (itemForRemove != null)
                    {
                        Repository.Remove(itemForRemove);
                    }
                }
            }

            foreach (var id in receptInventorySettings)
            {
                if (!settingItemIds.Contains(id))
                {
                    Repository.Add(new DentalServiceReceptInventorySetting { DentalServiceId = dentalServiceId, ReceptInventorySettingId = id });
                }
            }

            return Repository.SaveChanges();
        }
    }
}
