using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class InventoryService : EntityService<Inventory>, IInventoryService
    {
        private readonly IInventoryOutcomeService _inventoryOutcomeService;
        private readonly IInventoryIncomeService _inventoryIncomeService;

        public InventoryService(IRepository<Inventory> repository, IMapper mapper,
            IInventoryOutcomeService inventoryOutcomeService,
            IInventoryIncomeService inventoryIncomeService)
            : base(repository, mapper)
        {
            _inventoryOutcomeService = inventoryOutcomeService;
            _inventoryIncomeService = inventoryIncomeService;
        }

        public void UpdateInventoriesStock(int receptId)
        {
            var outcome = _inventoryOutcomeService
                .Repository
                .Query()
                .Include("InventoryItems.MeasurementUnit")
                .FirstOrDefault(f => f.ReceptId == receptId);

            if (outcome != null && outcome.InventoryItems.Count > 0)
            {
                var ids = outcome.InventoryItems.Select(s => s.InventoryId);
                var inventories = Repository.Query()
                    .Where(w => ids.Contains(w.Id))
                    .ToList();

                foreach (var item in outcome.InventoryItems)
                {
                    var inventory = inventories.FirstOrDefault(f => f.Id == item.InventoryId);
                    if (inventory != null)
                    {
                        inventory.Stock = inventory.Stock - item.Quantity * item.MeasurementUnit.Multiplicity;
                        Repository.Edit(inventory);
                    }
                }
                Repository.SaveChanges();
            }
        }

        public void UpdateInventoriesStockOnOutcome(int outcomeId)
        {
            var outcome = _inventoryOutcomeService
                .Repository
                .Query()
                .Include("InventoryItems.MeasurementUnit")
                .FirstOrDefault(f => f.Id == outcomeId);

            if (outcome != null && outcome.InventoryItems.Count > 0)
            {
                var ids = outcome.InventoryItems.Select(s => s.InventoryId);
                var inventories = Repository.Query()
                    .Where(w => ids.Contains(w.Id))
                    .ToList();

                foreach (var item in outcome.InventoryItems)
                {
                    var inventory = inventories.FirstOrDefault(f => f.Id == item.InventoryId);
                    if (inventory != null)
                    {
                        inventory.Stock = inventory.Stock - item.Quantity * item.MeasurementUnit.Multiplicity;
                        Repository.Edit(inventory);
                    }
                }
                Repository.SaveChanges();
            }
        }

        public void UpdateInventoriesStockOnIncome(int incomeId)
        {
            var income = _inventoryIncomeService
                .Repository
                .Query()
                .Include("InventoryItems.MeasurementUnit")
                .FirstOrDefault(f => f.Id == incomeId);

            if (income != null && income.InventoryItems.Count > 0)
            {
                var ids = income.InventoryItems.Select(s => s.InventoryId);
                var inventories = Repository.Query()
                    .Where(w => ids.Contains(w.Id))
                    .ToList();

                foreach (var item in income.InventoryItems)
                {
                    var inventory = inventories.FirstOrDefault(f => f.Id == item.InventoryId);
                    if (inventory != null)
                    {
                        inventory.Stock = inventory.Stock + item.Quantity * item.MeasurementUnit.Multiplicity;
                        Repository.Edit(inventory);
                    }
                }
                Repository.SaveChanges();
            }
        }
    }
}
