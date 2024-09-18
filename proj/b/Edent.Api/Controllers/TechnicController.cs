using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrimeNG.TableFilter;
using PrimeNG.TableFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TechnicController : ControllerBase
    {

        private readonly ITechnicService _technicService;

        public TechnicController(ITechnicService technicService)
        {
            _technicService = technicService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _technicService
                .Query()
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = _technicService.Mapper.Map<IEnumerable<TechnicViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var doctor = _technicService
                .Query()
                .FirstOrDefault(f => f.Id == id);

            var result = _technicService.Mapper.Map<TechnicViewModel>(doctor);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post(TechnicViewModel viewModel)
        {
            var created = _technicService.Create(viewModel);
            return Ok(_technicService.Mapper.Map<TechnicViewModel>(created));
        }

        [HttpPut]
        public IActionResult Put(TechnicViewModel viewModel)
        {
            var updated = _technicService.Update(viewModel.Id, viewModel);
            return Ok(_technicService.Mapper.Map<TechnicViewModel>(updated));
        }

        [HttpGet("SearchByName")]
        public IActionResult SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Ok(new TechnicViewModel[0]);
            }
            name = name.ToLower();

            var query = _technicService.GetByName(name);
            var result = _technicService.Mapper.Map<IEnumerable<TechnicViewModel>>(query);
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _technicService.Delete(id);
            return Ok(result);
        }


        [HttpPost("DeleteSelected")]
        public IActionResult DeleteSelected([FromBody] IEnumerable<TechnicViewModel> models)
        {
            foreach (var item in models)
            {
                var technic = _technicService.Mapper.Map<Technic>(item);
                _technicService.Repository.Remove(technic);
            }
            return Ok(_technicService.Repository.SaveChanges());
        }
    }
}
