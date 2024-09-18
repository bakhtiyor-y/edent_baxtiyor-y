using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeNG.TableFilter;
using PrimeNG.TableFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OutcomeInventoryItemController : ControllerBase
    {
        private readonly IOutcomeInventoryItemService _outcomeInventoryItemService;

        public OutcomeInventoryItemController(IOutcomeInventoryItemService outcomeInventoryItemService)
        {
            _outcomeInventoryItemService = outcomeInventoryItemService;
        }

        [HttpGet("GetByOutCome")]
        public IActionResult GetByOutCome(int outcomeId)
        {
            var query = _outcomeInventoryItemService
                .Query()
                .Include("MeasurementUnit")
                .Include("Inventory.MeasurementUnit")
                .Include("Inventory.InventoryPrices")
                .Where(w=>w.InventoryOutcomeId == outcomeId);

            var result = _outcomeInventoryItemService.Mapper.Map<IEnumerable<OutcomeInventoryItemViewModel>>(query);
            return Ok(result);
        }

    }
}
