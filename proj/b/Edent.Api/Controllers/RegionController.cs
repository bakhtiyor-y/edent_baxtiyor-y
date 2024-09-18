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
    public class RegionController : BaseApiController<RegionViewModel, Region>
    {
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService) : base(regionService)
        {
            _regionService = regionService;
        }

        [HttpGet]
        public override IActionResult Get([FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _entityService
                .Query()
                .Include(i => i.Country)
                .OrderBy(o => o.Id)
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = _entityService.Mapper.Map<IEnumerable<RegionViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });

        }


        [HttpGet("GetByCountry")]
        public IActionResult GetByCountry(int countryId)
        {
            var query = _regionService
                .Query()
                .Include(i => i.Country)
                .Where(w => w.CountryId == countryId);

            var result = _regionService.Mapper.Map<IEnumerable<RegionViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<RegionViewModel> models)
        {
            foreach (var item in models)
            {
                item.Country = null;
                var region = _regionService.Mapper.Map<Region>(item);
                _regionService.Repository.Remove(region);
            }
            return Ok(_regionService.Repository.SaveChanges());
        }
    }
}
