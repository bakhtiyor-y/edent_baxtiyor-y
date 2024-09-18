using Edent.Api.Controllers.BaseControllers;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryPriceController : BaseApiController<InventoryPriceViewModel, InventoryPrice>
    {
        private readonly IInventoryPriceService _inventoryPriceService;

        public InventoryPriceController(IInventoryPriceService inventoryPriceService) : base(inventoryPriceService)
        {
            _inventoryPriceService = inventoryPriceService;
        }

        [HttpGet("GetByInventory")]
        public IActionResult GetByInventory(int inventoryId)
        {
            var query = _inventoryPriceService
            .Query()
            .Where(w => w.InventoryId == inventoryId);

            var result = _inventoryPriceService.Mapper.Map<IEnumerable<InventoryPriceViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<InventoryPriceViewModel> models)
        {
            foreach (var item in models)
            {
                item.Inventory = null;
                var price = _inventoryPriceService.Mapper.Map<InventoryPrice>(item);
                _inventoryPriceService.Repository.Remove(price);
            }
            return Ok(_inventoryPriceService.Repository.SaveChanges());
        }
    }
}
