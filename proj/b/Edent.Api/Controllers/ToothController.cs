using Edent.Api.Infrastructure.Filters;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToothController : ControllerBase
    {
        private readonly IToothService _toothService;

        public ToothController(IToothService toothService)
        {
            _toothService = toothService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var query = _toothService
                .Query()
                .OrderBy(o => o.Id);

            var result = _toothService.Mapper.Map<IEnumerable<ToothViewModel>>(query);
            return Ok(result);
        }

        [HttpPost("UploadImage/{id}"), DisableRequestSizeLimit]
        [FileValidationFilter(1024 * 1024, ".png,.jpg,.jpeg")]
        public IActionResult UploadImage(int id)
        {

            //try
            //{
            //    if (Request.HasFormContentType)
            //    {
            //        var _file = Request.Form.Files[0];
            //        var result = _toothService.ChangeImage(_file, id);
            //        var t = Request.Host;
            //        var fullUrl = GlobalConfiguration.IsDevelopment ? $"http://localhost:5500//{MediaUrls.GetSharedImage(result)}" : MediaUrls.GetSharedImage(result);
            //        return Ok(new { imageUrl = fullUrl });

            //    }
            //    return BadRequest(new JsonErrorResponse { Messages = new[] { "NotFormContentType" } });
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(new JsonErrorResponse { Messages = new[] { ex.Message } });
            //}
            return BadRequest(new JsonErrorResponse { Messages = new[] { "Not implement" } });
        }

        [HttpPost]
        public IActionResult Post(ToothViewModel model)
        {
            var tooth = _toothService.Query().FirstOrDefault(f => f.Id == model.Id);
            if (tooth != null)
            {
                tooth.Name = model.Name;
                _toothService.Repository.Edit(tooth);
                if (_toothService.Repository.SaveChanges())
                {
                    return Ok();
                }
            }
            return BadRequest(new JsonErrorResponse { Messages = new[] { "NotFormContentType" } });
        }
    }
}
