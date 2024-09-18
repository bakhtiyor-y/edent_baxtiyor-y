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
    public class PartnerController : BaseApiController<PartnerViewModel, Partner>
    {
        private readonly IPartnerService _partnerService;

        public PartnerController(IPartnerService partnerService)
            : base(partnerService)
        {
            this._partnerService = partnerService;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var query = _partnerService
                         .Query()
                         .OrderBy(o => o.Id);

            var result = _partnerService.Mapper.Map<IEnumerable<PartnerViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<PartnerViewModel> models)
        {
            foreach (var item in models)
            {
                var partner = _partnerService.Mapper.Map<Partner>(item);
                _partnerService.Repository.Remove(partner);
            }
            return Ok(_partnerService.Repository.SaveChanges());
        }

    }
}
