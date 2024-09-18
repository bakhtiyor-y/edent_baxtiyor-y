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

    public class CityController : BaseApiController<CityViewModel, City>
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService) : base(cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public override IActionResult Get([FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _entityService
                .Query()
                .Include(i => i.Region)
                .OrderBy(o => o.Id)
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = _entityService.Mapper.Map<IEnumerable<CityViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });

        }

        [HttpGet("GetByRegion")]
        public IActionResult GetByRegion(int regionId)
        {
            var query = _cityService
                .Query()
                .Include(i => i.Region)
                .Where(w => w.RegionId == regionId);

            var result = _cityService.Mapper.Map<IEnumerable<CityViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<CityViewModel> models)
        {
            foreach (var item in models)
            {
                item.Region = null;
                var city = _cityService.Mapper.Map<City>(item);
                _cityService.Repository.Remove(city);
            }
            return Ok(_cityService.Repository.SaveChanges());
        }
    }
}
