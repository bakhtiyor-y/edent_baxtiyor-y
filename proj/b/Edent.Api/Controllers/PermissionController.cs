using AutoMapper;
using Edent.Api.Infrastructure;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Filters;
using Edent.Api.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class PermissionController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        public PermissionController(UserManager<User> userManager,
            RoleManager<Role> roleManager, IMapper mapper)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._mapper = mapper;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var type = typeof(Constants.PermissionConstants);
            var props = type.GetFields();
            var result = props.Select(s => new PermissionViewModel()
            {
                Name = s.Name,
                Description = s.GetCustomAttributes(typeof(DescriptionAttribute), false).Cast<DescriptionAttribute>().FirstOrDefault()?.Description
            });
            return Ok(result.ToList());
        }

        [HttpGet("ByRole")]
        public async Task<IActionResult> ByRole(int roleId)
        {
            var user = await _roleManager.FindByIdAsync(roleId.ToString());
            if (user != null)
            {
                var claims = await _roleManager.GetClaimsAsync(user);
                var permissions = claims.Select(s => s.Value);
                return Ok(permissions);
            }
            return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
        }
    }
}
