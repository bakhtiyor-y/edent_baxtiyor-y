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
    public class DiagnoseController : BaseApiController<DiagnoseViewModel, Diagnose>
    {
        private readonly IDiagnoseService _diagnoseService;

        public DiagnoseController(IDiagnoseService diagnoseService) : base(diagnoseService)
        {
            _diagnoseService = diagnoseService;
        }


        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            if (name == null)
                name = string.Empty;

            var query = _diagnoseService
                .Query()
                .Where(w => w.Name.ToLower().StartsWith(name.ToLower()))
                .OrderBy(o => o.Id)
                .ToList();

            var result = _entityService.Mapper.Map<IEnumerable<DiagnoseViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<DiagnoseViewModel> models)
        {
            foreach (var item in models)
            {
                var diagnose = _diagnoseService.Mapper.Map<Diagnose>(item);
                _diagnoseService.Repository.Remove(diagnose);
            }
            return Ok(_diagnoseService.Repository.SaveChanges());
        }
    }
}
