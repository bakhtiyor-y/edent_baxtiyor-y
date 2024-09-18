using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeNG.TableFilter;
using PrimeNG.TableFilter.Models;
using System.Collections.Generic;
using System.Linq;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryOutcomeController : ControllerBase
    {
        private readonly IInventoryOutcomeService _inventoryOutcomeService;
        private readonly IInventoryService _inventoryService;
        private readonly IUserResolverService _userResolverService;

        public InventoryOutcomeController(IInventoryOutcomeService inventoryOutcomeService,
             IInventoryService inventoryService,
             IUserResolverService userResolverService)
        {
            _inventoryOutcomeService = inventoryOutcomeService;
            _inventoryService = inventoryService;
            _userResolverService = userResolverService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _inventoryOutcomeService
                .Query()
                .Include(i => i.User.Doctor)
                .Include(i => i.User.Employee)
                .Include(i => i.Recipient.Doctor)
                .Include(i => i.Recipient.Employee)
                .Include("InventoryItems.MeasurementUnit")
                .Include("InventoryItems.Inventory.InventoryPrices")
                .OrderByDescending(o => o.CreatedDate)
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = _inventoryOutcomeService.Mapper.Map<IEnumerable<InventoryOutcomeViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });
        }

        [HttpPost]
        public IActionResult Post(InventoryOutcomeViewModel viewModel)
        {
            var userId = _userResolverService.CurrentUserId.HasValue ? _userResolverService.CurrentUserId.Value : 0;
            var entity = _inventoryOutcomeService.Mapper.Map<InventoryOutcome>(viewModel);
            entity.UserId = userId;
            var result = _inventoryOutcomeService.Create(entity);
            if (result.Id > 0)
            {
                _inventoryService.UpdateInventoriesStockOnOutcome(result.Id);
            }
            return Ok(result);
        }

    }
}
