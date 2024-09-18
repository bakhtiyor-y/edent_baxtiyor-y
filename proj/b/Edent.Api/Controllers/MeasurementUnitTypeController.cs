using Edent.Api.Controllers.BaseControllers;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementUnitTypeController : BaseApiController<MeasurementUnitTypeViewModel, MeasurementUnitType>
    {
        private readonly IMeasurementUnitTypeService _measurementUnitService;

        public MeasurementUnitTypeController(IMeasurementUnitTypeService measurementUnitService) : base(measurementUnitService)
        {
            _measurementUnitService = measurementUnitService;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var query = _entityService
                .Query()
                .Include(i => i.MeasurementUnits)
                .OrderBy(o => o.Id);

            var result = _entityService.Mapper.Map<IEnumerable<MeasurementUnitTypeViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<MeasurementUnitTypeViewModel> models)
        {
            foreach (var item in models)
            {
                item.MeasurementUnits = null;
                var unitType = _measurementUnitService.Mapper.Map<MeasurementUnitType>(item);
                _measurementUnitService.Repository.Remove(unitType);
            }
            return Ok(_measurementUnitService.Repository.SaveChanges());
        }
    }
}
