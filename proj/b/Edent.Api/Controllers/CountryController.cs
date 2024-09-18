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
    public class CountryController : BaseApiController<CountryViewModel, Country>
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService) :
            base(countryService)
        {
            _countryService = countryService;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var query = _countryService
                         .Query()
                         .OrderBy(o => o.Id);

            var result = _countryService.Mapper.Map<IEnumerable<CountryViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<CountryViewModel> models)
        {
            foreach (var item in models)
            {
                var term = _countryService.Mapper.Map<Country>(item);
                _countryService.Repository.Remove(term);
            }
            return Ok(_countryService.Repository.SaveChanges());
        }
    }
}
