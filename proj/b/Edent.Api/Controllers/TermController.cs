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
    public class TermController : BaseApiController<TermViewModel, Term>
    {
        private readonly ITermService _termService;

        public TermController(ITermService termService) : base(termService)
        {
            _termService = termService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var query = _termService
                         .Query()
                         .OrderBy(o => o.Id);

            var result = _termService.Mapper.Map<IEnumerable<TermViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<TermViewModel> models)
        {
            foreach (var item in models)
            {
                var term = _termService.Mapper.Map<Term>(item);
                _termService.Repository.Remove(term);
            }
            return Ok(_termService.Repository.SaveChanges());
        }
    }
}
