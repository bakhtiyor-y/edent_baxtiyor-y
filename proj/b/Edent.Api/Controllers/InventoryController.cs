using Edent.Api.Controllers.BaseControllers;
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
    public class InventoryController : BaseApiController<InventoryViewModel, Inventory>
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService) : base(inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public override IActionResult Get([FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _inventoryService
                .Query()
                .Include(i => i.MeasurementUnit)
                .Include(i => i.InventoryPrices)
                .OrderBy(o => o.Id)
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = _inventoryService.Mapper.Map<IEnumerable<InventoryViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });

        }

        [HttpGet("GetInventoriesByName")]
        public IActionResult GetInventoriesByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Ok(new List<InventoryViewModel>());

            var query = _inventoryService
                .Query()
                .Include(i => i.MeasurementUnit)
                .OrderBy(o => o.Id)
                .Where(w => w.Name.ToLower().StartsWith(name.ToLower()));

            var result = _inventoryService.Mapper.Map<IEnumerable<InventoryViewModel>>(query);
            return Ok(result);

        }

        [HttpPost]
        public override IActionResult Post(InventoryViewModel viewModel)
        {
            var created = _inventoryService.Create(viewModel);

            var createdWithIncluding = _inventoryService
               .Query()
               .Include(i => i.MeasurementUnit)
               .Include(i => i.InventoryPrices)
               .FirstOrDefault(f => f.Id == created.Id);

            var result = _inventoryService.Mapper.Map<InventoryViewModel>(createdWithIncluding);
            return Ok(result);
        }

        [HttpPut]
        public override IActionResult Put(InventoryViewModel viewModel)
        {
            var updated = _inventoryService.Update(viewModel.Id, viewModel);
            var updatedWithIncluding = _inventoryService
              .Query()
              .Include(i => i.MeasurementUnit)
              .Include(i => i.InventoryPrices)
              .FirstOrDefault(f => f.Id == updated.Id);

            var result = _inventoryService.Mapper.Map<InventoryViewModel>(updatedWithIncluding);
            return Ok(result);
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<InventoryViewModel> models)
        {
            foreach (var item in models)
            {
                item.MeasurementUnit = null;
                var inventory = _inventoryService.Mapper.Map<Inventory>(item);
                _inventoryService.Repository.Remove(inventory);
            }
            return Ok(_inventoryService.Repository.SaveChanges());
        }
    }
}
