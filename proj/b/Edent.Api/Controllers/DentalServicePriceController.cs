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
    public class DentalServicePriceController : BaseApiController<DentalServicePriceViewModel, DentalServicePrice>
    {
        private readonly IDentalServicePriceService _dentalServicePriceService;

        public DentalServicePriceController(IDentalServicePriceService dentalServicePriceService) : base(dentalServicePriceService)
        {
            _dentalServicePriceService = dentalServicePriceService;
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<DentalServicePriceViewModel> models)
        {
            foreach (var item in models)
            {
                item.DentalService = null;
                var price = _dentalServicePriceService.Mapper.Map<DentalServicePrice>(item);
                _dentalServicePriceService.Repository.Remove(price);
            }
            return Ok(_dentalServicePriceService.Repository.SaveChanges());
        }
    }
}
