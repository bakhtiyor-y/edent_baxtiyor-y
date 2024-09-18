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
    public class ProfessionController : BaseApiController<ProfessionViewModel, Profession>
    {
        private readonly IProfessionService _professionService;

        public ProfessionController(IProfessionService professionService) : base(professionService)
        {
            _professionService = professionService;
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<ProfessionViewModel> models)
        {
            foreach (var item in models)
            {
                item.Specializations = null;
                var profession = _professionService.Mapper.Map<Profession>(item);
                _professionService.Repository.Remove(profession);
            }
            return Ok(_professionService.Repository.SaveChanges());
        }
    }
}
