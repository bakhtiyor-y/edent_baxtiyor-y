using Edent.Api.Controllers.BaseControllers;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeNG.TableFilter;
using PrimeNG.TableFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReceptInventorySettingController : BaseApiController<ReceptInventorySettingViewModel, ReceptInventorySetting>
    {

        private readonly IReceptInventorySettingService _receptInventorySettingService;
        private readonly IReceptInventorySettingItemService _receptInventorySettingItemService;

        public ReceptInventorySettingController(IReceptInventorySettingService receptInventorySettingService,
            IReceptInventorySettingItemService receptInventorySettingItemService)
            : base(receptInventorySettingService)
        {
            _receptInventorySettingService = receptInventorySettingService;
            _receptInventorySettingItemService = receptInventorySettingItemService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public override IActionResult Get([FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _receptInventorySettingService
                .Query()
                .Include("ReceptInventorySettingItems.Inventory")
                .OrderBy(o => o.Id)
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = _entityService.Mapper.Map<IEnumerable<ReceptInventorySettingViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });

        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public override IActionResult Put(ReceptInventorySettingViewModel viewModel)
        {
            var setting = _receptInventorySettingService
                .Query()
                .AsNoTracking()
                .FirstOrDefault(f => f.Id == viewModel.Id);

            _receptInventorySettingItemService.UpdateReceptInventorySettingItems(viewModel.Id, viewModel.ReceptInventorySettingItems);

            setting.Name = viewModel.Name;
            setting.IsActive = viewModel.IsActive;
            setting.IsDefault = viewModel.IsDefault;
            _receptInventorySettingService.Repository.Edit(setting);
            _receptInventorySettingService.Repository.SaveChanges();

            return Ok(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public override IActionResult Post(ReceptInventorySettingViewModel viewModel)
        {
            return base.Post(viewModel);
        }

        [HttpGet("GetByName")]
        [Authorize(Roles = "admin")]
        public IActionResult GetByName([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }
            var query = _receptInventorySettingService
                .Query()
                .Include("ReceptInventorySettingItems.Inventory")
                .OrderBy(o => o.Id)
                .Where(w => w.Name.ToLower().StartsWith(name.ToLower()));

            var result = _entityService.Mapper.Map<IEnumerable<ReceptInventorySettingViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("ByIds")]
        [Authorize(Roles = "admin")]
        public IActionResult ByIds([FromBody] int[] ids)
        {
            var query = _receptInventorySettingService
                .Query()
                .Include("ReceptInventorySettingItems.Inventory")
                .OrderBy(o => o.Id)
                .Where(w => ids.Contains(w.Id));

            var result = _entityService.Mapper.Map<IEnumerable<ReceptInventorySettingViewModel>>(query);
            return Ok(result);
        }

        [HttpGet("GetDefaults")]
        public IActionResult GetDefaults()
        {
            var query = _receptInventorySettingService
                .Query()
                .Include("ReceptInventorySettingItems.Inventory")
                .OrderBy(o => o.Id)
                .Where(w => w.IsDefault)
                .SelectMany(s => s.ReceptInventorySettingItems);

            var result = _entityService.Mapper.Map<IEnumerable<ReceptInventorySettingItemViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<ReceptInventorySettingViewModel> models)
        {
            foreach (var item in models)
            {
                item.ReceptInventorySettingItems = null;
                var setting = _receptInventorySettingService.Mapper.Map<ReceptInventorySetting>(item);
                _receptInventorySettingService.Repository.Remove(setting);
            }
            return Ok(_receptInventorySettingService.Repository.SaveChanges());
        }

    }
}
