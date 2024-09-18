using AutoMapper;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Filters;
using Edent.Api.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeNG.TableFilter;
using PrimeNG.TableFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Edent.Api.Infrastructure.Constants;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(UserManager<User> userManager,
            RoleManager<Role> roleManager, IMapper mapper)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._mapper = mapper;
        }


        [HttpGet]
        public IActionResult Get([FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _roleManager
              .Roles
              .PrimengTableFilter(filterModel, out int totalRecord);


            var result = _mapper.Map<IEnumerable<RoleViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var query = _roleManager
              .Roles
              .OrderBy(o => o.Id);


            var result = _mapper.Map<IEnumerable<RoleViewModel>>(query);
            return Ok(result);
        }

        [HttpGet("ByUser")]
        public async Task<IActionResult> ByUser(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(roles);
            }
            return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
        }

        [HttpPost]
        public async Task<IActionResult> Post(RoleViewModel viewModel)
        {
            var role = new Role { Name = viewModel.Name };

            var createResult = await _roleManager.CreateAsync(role);
            if (createResult.Succeeded)
            {
                var resultViewModel = _mapper.Map<RoleViewModel>(role);
                foreach (var item in viewModel.Permissions)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, item));
                    resultViewModel.Permissions.Add(item);
                }
                return Ok(resultViewModel);
            }
            return BadRequest(new JsonErrorResponse { Messages = new[] { "Error on creating role" } });
        }

        [HttpPut]
        public async Task<IActionResult> Put(RoleViewModel viewModel)
        {
            var role = await _roleManager.FindByIdAsync(viewModel.Id.ToString());
            if (role == null)
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Role not found" } });
            var currentPermissions = await _roleManager.GetClaimsAsync(role);
            var cpstr = currentPermissions.Select(s => s.Value);

            role.Name = viewModel.Name;
            var roleUpdateResult = await _roleManager.UpdateAsync(role);
            if (roleUpdateResult.Succeeded)
            {
                var resultViewModel = _mapper.Map<RoleViewModel>(role);
                foreach (var claim in currentPermissions)
                {
                    //Remove Permissions
                    if (!viewModel.Permissions.Contains(claim.Value))
                    {
                        await _roleManager.RemoveClaimAsync(role, claim);
                    }
                }
                //Set new Permissions if not exist
                foreach (var item in viewModel.Permissions)
                {
                    if (!cpstr.Contains(item))
                        await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, item));
                    resultViewModel.Permissions.Add(item);
                }
                return Ok(resultViewModel);
            }
            return BadRequest(new JsonErrorResponse { Messages = new[] { "Error on updating role" } });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Error on delete role" } });
            }
        }


        [HttpPost("DeleteSelected")]
        public async Task<IActionResult> DeleteSelected([FromBody] IEnumerable<Role> models)
        {
            foreach (var item in models)
            {
                var role = await _roleManager.FindByIdAsync(item.Id.ToString());
                await _roleManager.DeleteAsync(role);
            }
            return Ok();
        }
    }
}
