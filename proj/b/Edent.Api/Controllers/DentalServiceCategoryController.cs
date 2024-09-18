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
    public class DentalServiceCategoryController : BaseApiController<DentalServiceCategoryViewModel, DentalServiceCategory>
    {
        private readonly IDentalServiceCategoryService _dentalServiceCategoryService;

        public DentalServiceCategoryController(IDentalServiceCategoryService dentalServiceCategoryService) : base(dentalServiceCategoryService)
        {
            _dentalServiceCategoryService = dentalServiceCategoryService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var query = _dentalServiceCategoryService
                .Query()
                .OrderBy(o => o.Id);

            var result = _dentalServiceCategoryService.Mapper.Map<IEnumerable<DentalServiceCategoryViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<DentalServiceCategoryViewModel> models)
        {
            foreach (var item in models)
            {
                item.DentalServices = null;
                var category = _dentalServiceCategoryService.Mapper.Map<DentalServiceCategory>(item);
                _entityService.Repository.Remove(category);
            }
            return Ok(_dentalServiceCategoryService.Repository.SaveChanges());
        }
    }
}
