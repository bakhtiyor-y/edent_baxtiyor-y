using Edent.Api.Helpers;
using Edent.Api.Infrastructure.Filters;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly IAddressService _addressService;

        public OrganizationController(IOrganizationService organizationService, IAddressService addressService)
        {
            _organizationService = organizationService;
            _addressService = addressService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var organization = _organizationService
                .Query()
                .Include("Address.City.Region")
                .OrderBy(o => o.Id)
                .FirstOrDefault();

            var result = _organizationService.Mapper.Map<OrganizationViewModel>(organization);
            return Ok(result);

        }

        [HttpPut]
        public IActionResult Put(OrganizationViewModel viewModel)
        {
            var updated = _organizationService.Update(viewModel.Id, viewModel);
            return Ok(_organizationService.Mapper.Map<OrganizationViewModel>(updated));
        }

        [HttpPost("UpdateLogo"), DisableRequestSizeLimit]
        [FileValidationFilter(1024 * 1024, ".png,.jpg,.jpeg")]
        public IActionResult UpdateLogo()
        {
            try
            {
                var organization = _organizationService
                    .Query()
                    .Include("Address.City.Region")
                    .OrderBy(o => o.Id)
                    .FirstOrDefault();
                if (organization == null)
                    return BadRequest(new JsonErrorResponse { Messages = new[] { "Organiztion not found" } });

                if (Request.HasFormContentType)
                {
                    var _file = Request.Form.Files[0];
                    var pathToSave = MediaUrls.GetSharedImageDirectory();
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    if (_file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(_file.ContentDisposition).FileName.Trim('"');
                        var fileGeneratedName = Guid.NewGuid().ToString("D") + Path.GetExtension(fileName);
                        var fullPath = MediaUrls.GetSharedImageUrl(fileGeneratedName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            _file.CopyTo(stream);

                            var oldImage = Path.Combine(pathToSave, organization.LogoImage ?? string.Empty);
                            organization.LogoImage = fileGeneratedName;

                            _organizationService.Repository.Edit(organization);
                            if (_organizationService.Repository.SaveChanges() && System.IO.File.Exists(oldImage))
                            {
                                System.IO.File.Delete(oldImage);
                            }

                            var host = Request.Host.ToString();
                            if (host.Equals("localhost:4200"))
                            {
                                host = "localhost:5500";
                            }
                            var fullUrl = $"http://{host}{MediaUrls.GetSharedImage(organization.LogoImage)}";
                            return Ok(new { logoImage = fullUrl });

                        }
                    }
                    else
                    {
                        return BadRequest(new JsonErrorResponse { Messages = new[] { "File not uploaded" } });
                    }
                }
                else
                {
                    return BadRequest(new JsonErrorResponse { Messages = new[] { "Not supported type" } });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { ex.Message } });
            }

        }
    }
}
