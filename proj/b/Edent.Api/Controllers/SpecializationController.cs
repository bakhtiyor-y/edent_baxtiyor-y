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
    public class SpecializationController : BaseApiController<SpecializationViewModel, Specialization>
    {
        private readonly ISpecializationService _specializationService;

        public SpecializationController(ISpecializationService specializationService) : base(specializationService)
        {
            _specializationService = specializationService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var query = _specializationService
                         .Query()
                         .OrderBy(o => o.DisplayOrder);

            var result = _specializationService.Mapper.Map<IEnumerable<SpecializationViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<SpecializationViewModel> models)
        {
            foreach (var item in models)
            {
                var specialization = _specializationService.Mapper.Map<Specialization>(item);
                _specializationService.Repository.Remove(specialization);
            }
            return Ok(_specializationService.Repository.SaveChanges());
        }
    }
}
