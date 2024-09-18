using Edent.Api.Controllers.BaseControllers;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
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
    public class DentalServiceController : BaseApiController<DentalServiceViewModel, DentalService>
    {
        private readonly IDentalServiceService _dentalServiceService;
        private readonly IDentalServiceReceptInventorySettingService _dentalServiceReceptInventorySettingService;

        public DentalServiceController(IDentalServiceService dentalServiceService, IDentalServiceReceptInventorySettingService dentalServiceReceptInventorySettingService) : base(dentalServiceService)
        {
            _dentalServiceService = dentalServiceService;
            _dentalServiceReceptInventorySettingService = dentalServiceReceptInventorySettingService;
        }

        [HttpGet]
        public override IActionResult Get([FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _dentalServiceService
                .Query()
                .Include(i => i.DentalServiceGroup)
                .Include(i => i.DentalServiceCategory)
                .Include(i => i.DentalServicePrices)
                .Include(i => i.DentalServiceReceptInventorySettings)
                .OrderBy(o => o.Id)
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = _dentalServiceService.Mapper.Map<IEnumerable<DentalServiceViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });

        }

        [HttpPost]
        public override IActionResult Post(DentalServiceViewModel viewModel)
        {
            var result = _dentalServiceService.Create(viewModel);
            if (result.Id > 0)
            {
                foreach (var item in viewModel.ReceptInventorySettings)
                {
                    _dentalServiceReceptInventorySettingService.Repository.Add(new DentalServiceReceptInventorySetting { DentalServiceId = result.Id, ReceptInventorySettingId = item });
                    _dentalServiceReceptInventorySettingService.Repository.SaveChanges();
                }
            }
            var created = _dentalServiceService.GetByIncluding(result.Id);
            return Ok(created);
        }

        [HttpPut]
        public override IActionResult Put(DentalServiceViewModel viewModel)
        {
            var result = _dentalServiceService.Update(viewModel.Id, viewModel);

            if (result.Id > 0)
            {
                _dentalServiceReceptInventorySettingService.UpdateReceptInventorySettingItems(result.Id, viewModel.ReceptInventorySettings);
            }
            var updated = _dentalServiceService.GetByIncluding(result.Id);
            return Ok(updated);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var query = _dentalServiceService
                .Query()
                .Include(i => i.DentalServiceGroup)
                .Include(i => i.DentalServiceCategory)
                .OrderBy(o => o.Id);

            var result = _entityService.Mapper.Map<IEnumerable<DentalServiceViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<DentalServiceViewModel> models)
        {
            foreach (var item in models)
            {
                item.DentalServiceCategory = null;
                item.DentalServiceGroup = null;
                item.DentalServicePrices = null;
                var dentalService = _dentalServiceService.Mapper.Map<DentalService>(item);
                _dentalServiceService.Repository.Remove(dentalService);
            }
            return Ok(_dentalServiceService.Repository.SaveChanges());
        }
    }
}
