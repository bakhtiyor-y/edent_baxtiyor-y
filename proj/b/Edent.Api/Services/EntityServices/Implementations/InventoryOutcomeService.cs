using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class InventoryOutcomeService : EntityService<InventoryOutcome>, IInventoryOutcomeService
    {
        private readonly IReceptService _receptService;
        private readonly IUserResolverService _userResolverService;

        public InventoryOutcomeService(IRepository<InventoryOutcome> repository, IMapper mapper, IReceptService receptService, IUserResolverService userResolverService)
            : base(repository, mapper)
        {
            _receptService = receptService;
            _userResolverService = userResolverService;
        }

        public bool ProvideReceptOutcome(int receptId)
        {
            var recept = _receptService
                .Query()
                .Include("ReceptInventories")
                .FirstOrDefault(f => f.Id == receptId);

            List<OutcomeInventoryItem> outcomeInventoryItems = new List<OutcomeInventoryItem>();

            foreach (var item in recept.ReceptInventories)
            {
                var outcomeInventoryItem = new OutcomeInventoryItem
                {
                    InventoryId = item.InventoryId,
                    MeasurementUnitId = item.MeasurementUnitId,
                    Quantity = item.Quantity
                };
                outcomeInventoryItems.Add(outcomeInventoryItem);
            }

            var inventoryOutcome = new InventoryOutcome
            {
                ReceptId = receptId,
                InventoryItems = outcomeInventoryItems,
                UserId = _userResolverService.CurrentUserId
            };

            Repository.Add(inventoryOutcome);
            return Repository.SaveChanges();
        }
    }
}
