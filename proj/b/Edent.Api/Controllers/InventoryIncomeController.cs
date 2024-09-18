using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
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
    public class InventoryIncomeController : ControllerBase
    {
        private readonly IInventoryIncomeService _inventoryIncomeService;
        private readonly IInventoryService _inventoryService;
        private readonly IUserResolverService _userResolverService;

        public InventoryIncomeController(IInventoryIncomeService inventoryIncomeService,
             IInventoryService inventoryService,
             IUserResolverService userResolverService)
        {
            _inventoryIncomeService = inventoryIncomeService;
            _inventoryService = inventoryService;
            _userResolverService = userResolverService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _inventoryIncomeService
                .Query()
                .Include("User.Employee")
                .Include("User.Doctor")
                .Include("InventoryItems.MeasurementUnit")
                .Include("InventoryItems.Inventory.InventoryPrices")
                .OrderByDescending(o => o.CreatedDate)
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = _inventoryIncomeService.Mapper.Map<IEnumerable<InventoryIncomeViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });
        }

        [HttpPost]
        public IActionResult Post(InventoryIncomeViewModel viewModel)
        {
            var userId = _userResolverService.CurrentUserId.HasValue ? _userResolverService.CurrentUserId.Value : 0;
            var entity = _inventoryIncomeService.Mapper.Map<InventoryIncome>(viewModel);
            entity.UserId = userId;
            var result = _inventoryIncomeService.Create(entity);
            if (result.Id > 0)
            {
                _inventoryService.UpdateInventoriesStockOnIncome(result.Id);
            }
            return Ok(result);
        }
    }
}
