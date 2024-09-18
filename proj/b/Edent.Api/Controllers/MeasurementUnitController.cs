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
    public class MeasurementUnitController : BaseApiController<MeasurementUnitViewModel, MeasurementUnit>
    {
        private readonly IMeasurementUnitService _measurementUnitOptionService;

        public MeasurementUnitController(IMeasurementUnitService measurementUnitOptionService) : base(measurementUnitOptionService)
        {
            _measurementUnitOptionService = measurementUnitOptionService;
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<MeasurementUnitViewModel> models)
        {
            foreach (var item in models)
            {
                item.MeasurementUnitType = null;
                var unit = _measurementUnitOptionService.Mapper.Map<MeasurementUnit>(item);
                _measurementUnitOptionService.Repository.Remove(unit);
            }
            return Ok(_measurementUnitOptionService.Repository.SaveChanges());
        }
    }
}
