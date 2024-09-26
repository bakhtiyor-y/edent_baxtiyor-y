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
    [Route("api/cities")]
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

        [HttpGet("{regionId}")]
        public async Task<IActionResult> GetByRegion(int regionId)
        {
            var query = await _cityService
                .Query()
                .Include(i => i.Region)
                .Where(w => w.RegionId == regionId)
                .ToListAsync();

            var result = _cityService.Mapper.Map<List<CityViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("delete-range")]
        public async Task<IActionResult> DeleteSelected([FromBody] int[] cityIds)
        {
            
            await _cityService.Repository.RemoveRangeByIdsAsync(cityIds);
            
            return Ok(await _cityService.Repository.SaveChangesAsync());
        }
    }
}
