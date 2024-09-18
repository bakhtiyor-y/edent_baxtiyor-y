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
    public class IncomeInventoryItemController : ControllerBase
    {
        private readonly IIncomeInventoryItemService _incomeInventoryItemService;

        public IncomeInventoryItemController(IIncomeInventoryItemService incomeInventoryItemService)
        {
            _incomeInventoryItemService = incomeInventoryItemService;
        }

        [HttpGet("GetByIncome")]
        public IActionResult GetByIncome(int incomeId)
        {
            var query = _incomeInventoryItemService
                .Query()
                .Include("MeasurementUnit")
                .Include("Inventory.MeasurementUnit")
                .Include("Inventory.InventoryPrices")
                .Where(w => w.InventoryIncomeId == incomeId);

            var result = _incomeInventoryItemService.Mapper.Map<IEnumerable<IncomeInventoryItemViewModel>>(query);
            return Ok(result);
        }

    }
}
