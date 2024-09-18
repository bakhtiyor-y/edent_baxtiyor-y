using Edent.Api.Controllers.BaseControllers;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Enums;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DentalServiceGroupController : BaseApiController<DentalServiceGroupViewModel, DentalServiceGroup>
    {
        private readonly IDentalServiceGroupService _dentalServiceGroupService;

        public DentalServiceGroupController(IDentalServiceGroupService dentalServiceGroupService) : base(dentalServiceGroupService)
        {
            _dentalServiceGroupService = dentalServiceGroupService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var query = _entityService
                .Query()
                .OrderBy(o => o.Id);

            var result = _entityService.Mapper.Map<IEnumerable<DentalServiceGroupViewModel>>(query);
            return Ok(result);
        }

        [HttpGet("GetByDentalService")]
        public IActionResult GetByDentalService(string name)
        {
            if (name == null)
                name = string.Empty;

            var query = _dentalServiceGroupService
                .Query()
                .Include("DentalServices.DentalServicePrices")
                .Include(i => i.DentalServices.Where(w => w.Name.ToLower().StartsWith(name.ToLower())))
                .OrderBy(o => o.Id)
                .ToList();

            var result = _entityService.Mapper.Map<IEnumerable<DentalServiceGroupViewModel>>(query.Where(w => w.DentalServices.Count > 0));
            return Ok(result);
        }

        [HttpGet("GetTypedByDentalService")]
        public IActionResult GetTypedByDentalService(string name, DentalServiceType type)
        {
            if (name == null)
                name = string.Empty;

            var query = _dentalServiceGroupService
                .Query()
                .Include("DentalServices.DentalServicePrices")
                .Include(i => i.DentalServices.Where(w => w.Name.ToLower().StartsWith(name.ToLower()) && w.Type == type))
                .OrderBy(o => o.Id)
                .ToList();

            var result = _entityService.Mapper.Map<IEnumerable<DentalServiceGroupViewModel>>(query.Where(w => w.DentalServices.Count > 0));
            return Ok(result);
        }

        [HttpGet("GetAsPriceList")]
        public IActionResult GetAsPriceList()
        {
            var query = _entityService
                .Query()
                .Include("DentalServices.DentalServiceCategory")
                .Include("DentalServices.DentalServicePrices")
                .OrderBy(o => o.Id);

            var result = _entityService.Mapper.Map<IEnumerable<DentalServiceGroupViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<DentalServiceGroupViewModel> models)
        {
            foreach (var item in models)
            {
                item.DentalServices = null;
                var group = _dentalServiceGroupService.Mapper.Map<DentalServiceGroup>(item);
                _dentalServiceGroupService.Repository.Remove(group);
            }
            return Ok(_dentalServiceGroupService.Repository.SaveChanges());
        }
    }
}
