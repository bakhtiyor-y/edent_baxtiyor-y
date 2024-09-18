using Edent.Api.Infrastructure.Entities;
using Edent.Api.Models;
using Edent.Api.Services.Base;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeNG.TableFilter;
using PrimeNG.TableFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Controllers.BaseControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseApiController<TViewModel, TEntity>
       : ControllerBase where TEntity : Entity where TViewModel : IViewModel
    {
        protected readonly IEntityService<TEntity> _entityService;

        public BaseApiController(IEntityService<TEntity> entityService)
        {
            _entityService = entityService;
        }

        [HttpGet]
        public virtual IActionResult Get([FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _entityService
                .Query()
                .OrderBy(o => o.Id)
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = _entityService.Mapper.Map<IEnumerable<TViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });
        }

        [HttpGet("GetById")]
        public virtual IActionResult GetById(int id)
        {
            var result = _entityService.GetById<TViewModel>(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public virtual IActionResult Post(TViewModel viewModel)
        {
            var result = _entityService.Create(viewModel);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public virtual IActionResult Put(TViewModel viewModel)
        {      
            var result = _entityService.Update(viewModel.Id, viewModel);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize]
        public virtual IActionResult Delete(int id)
        {
            var result = _entityService.Delete(id);
            return Ok(new ApiResultModel<bool> { Result = result });
        }
    }
}
