using AutoMapper;
using Edent.Api.Helpers;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Filters;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Edent.Api.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeNG.TableFilter;
using PrimeNG.TableFilter.Models;
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
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUserResolverService _userResolverService;
        private readonly IEmployeeService _employeeService;
        private readonly IDoctorService _doctorService;

        public UserController(UserManager<User> userManager,
            RoleManager<Role> roleManager, IMapper mapper,
            IUserResolverService userResolverService,
            IEmployeeService employeeService,
            IDoctorService doctorService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _userResolverService = userResolverService;
            _employeeService = employeeService;
            _doctorService = doctorService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Get([FromQuery] string filter)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _employeeService
                .Repository
                .Query()
                .Include(i => i.User)
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = _mapper.Map<IEnumerable<UserViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });
        }

        [HttpGet("GetEmployees")]
        [Authorize(Roles = "reception, admin, rentgen")]
        public async Task<IActionResult> GetEmployees()
        {
            var query = _employeeService
                .Repository
                .Query()
                .Include(i => i.User)
                .ToList();
            //var rentgenUsers = query.Where(async w => await _userManager.IsInRoleAsync(w.User, "rentgen"));
            var rentgenUsers = new List<Employee>();
            foreach (Employee employee in query)
            {
                if (await _userManager.IsInRoleAsync(employee.User, "rentgen"))
                {
                    rentgenUsers.Add(employee);
                }
            }

            var result = _mapper.Map<IEnumerable<UserViewModel>>(rentgenUsers);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post(UserManageViewModel viewModel)
        {
            var user = new User
            {
                Email = viewModel.Email,
                IsActive = viewModel.IsActive,
                PhoneNumber = viewModel.PhoneNumber,
                ProfileImage = viewModel.ProfileImage,
                UserName = viewModel.UserName,
            };

            var createResult = await _userManager.CreateAsync(user);
            if (createResult.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, viewModel.Roles);

                var employee = new Employee
                {
                    BirthDate = viewModel.BirthDate,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Patronymic = viewModel.Patronymic,
                    Gender = viewModel.Gender,
                    UserId = user.Id,
                };
                var created = _employeeService.Create(employee);
                var result = _mapper.Map<UserViewModel>(created);
                return Ok(result);
            }

            return BadRequest(new JsonErrorResponse { Messages = new[] { "Error on create user" } });
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Put(UserManageViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.Id.ToString());
            if (user == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
            }

            user.Email = viewModel.Email;
            user.IsActive = viewModel.IsActive;
            user.PhoneNumber = viewModel.PhoneNumber;
            user.ProfileImage = viewModel.ProfileImage;

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRolesAsync(user, viewModel.Roles);

                var employee = _employeeService.GetByUserId(viewModel.Id);
                employee.BirthDate = viewModel.BirthDate;
                employee.FirstName = viewModel.FirstName;
                employee.LastName = viewModel.LastName;
                employee.Patronymic = viewModel.Patronymic;
                employee.Gender = viewModel.Gender;
                _employeeService.Repository.Edit(employee);
                await _employeeService.Repository.SaveChangesAsync();

                var result = _mapper.Map<UserViewModel>(employee);
                return Ok(result);
            }
            return BadRequest(new JsonErrorResponse { Messages = new[] { "Error on update user" } });
        }

        [HttpPut("SetPassword")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.UserId.ToString());
            if (user == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
            }

            if (await _userManager.HasPasswordAsync(user))
            {
                await _userManager.RemovePasswordAsync(user);
            }
            var updateResult = await _userManager.AddPasswordAsync(user, viewModel.Password);


            return Ok(new ApiResult<bool> { Succeeded = updateResult.Succeeded, Result = true });
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Model state not valid" } });
            }

            var userId = _userResolverService.CurrentUserId.HasValue ? _userResolverService.CurrentUserId.Value : 0;

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
            }
            if (await _userManager.HasPasswordAsync(user))
            {
                await _userManager.RemovePasswordAsync(user);
            }
            var updateResult = await _userManager.AddPasswordAsync(user, viewModel.NewPassword);
            return Ok(new ApiResult<bool> { Succeeded = updateResult.Succeeded, Result = true });
        }

        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(UserManageViewModel viewModel)
        {
            try
            {
                var userId = _userResolverService.CurrentUserId.HasValue ? _userResolverService.CurrentUserId.Value : 0;
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
                }

                user.Email = viewModel.Email;
                user.PhoneNumber = viewModel.PhoneNumber;
                //user.ProfileImage = viewModel.ProfileImage;
                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    var employee = _employeeService.GetByUserId(userId);
                    if (employee != null)
                    {
                        employee.BirthDate = viewModel.BirthDate;
                        employee.FirstName = viewModel.FirstName;
                        employee.LastName = viewModel.LastName;
                        employee.Patronymic = viewModel.Patronymic;
                        _employeeService.Repository.Edit(employee);
                        await _employeeService.Repository.SaveChangesAsync();
                    }

                    var doctor = _doctorService.GetByUserId(userId);
                    if (doctor != null)
                    {
                        doctor.BirthDate = viewModel.BirthDate;
                        doctor.FirstName = viewModel.FirstName;
                        doctor.LastName = viewModel.LastName;
                        doctor.Patronymic = viewModel.Patronymic;
                        _doctorService.Repository.Edit(doctor);
                        await _doctorService.Repository.SaveChangesAsync();
                    }

                    return Ok(new ApiResult<bool> { Succeeded = updateResult.Succeeded, Result = true });
                }
                else
                {
                    return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { ex.Message } });
            }

        }

        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = _userResolverService.CurrentUserId.HasValue ? _userResolverService.CurrentUserId.Value : 0;

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
            }

            var employee = _employeeService.GetByUserId(userId);
            if (employee != null)
            {
                var result = _mapper.Map<UserViewModel>(employee);
                return Ok(result);

            }

            var doctor = _doctorService.GetByUserId(userId);
            if (doctor != null)
            {
                var result = _mapper.Map<UserViewModel>(doctor);
                
                return Ok(result);
            }

            return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
        }

        [HttpPost("UpdateAvatar"), DisableRequestSizeLimit]
        [FileValidationFilter(1024 * 1024, ".png,.jpg,.jpeg")]
        public async Task<IActionResult> UpdateAvatar()
        {

            try
            {
                var userId = _userResolverService.CurrentUserId.HasValue ? _userResolverService.CurrentUserId.Value : 0;
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
                }

                if (Request.HasFormContentType)
                {
                    var _file = Request.Form.Files[0];
                    var pathToSave = MediaUrls.GetUserMediaDirectory(userId);
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    if (_file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(_file.ContentDisposition).FileName.Trim('"');
                        var fileGeneratedName = Guid.NewGuid().ToString("D") + Path.GetExtension(fileName);
                        var fullPath = MediaUrls.GetUserMediaUrl(fileGeneratedName, userId);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            _file.CopyTo(stream);

                            var oldImage = Path.Combine(pathToSave, user.ProfileImage ?? string.Empty);
                            user.ProfileImage = fileGeneratedName;

                            var updateResult = await _userManager.UpdateAsync(user);
                            if (updateResult.Succeeded && System.IO.File.Exists(oldImage))
                            {
                                System.IO.File.Delete(oldImage);
                            }

                            var fullUrl = $"http://{Request.Host}{MediaUrls.GetUserMedia(user.ProfileImage, userId)}";
                            return Ok(new { profileImage = fullUrl });

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

        [HttpGet("SearchByName")]
        public IActionResult SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Ok(new List<UserViewModel>());

            name = name.ToLower();

            var empQuery = _employeeService
                .Query()
                .Include(i=>i.User)
                .Where(w => w.FirstName.ToLower().StartsWith(name) || w.LastName.ToLower().StartsWith(name)).ToList();

            var docQuery = _doctorService
                .Query()
                .Include(i => i.User)
                .Where(w => w.FirstName.ToLower().StartsWith(name) || w.LastName.ToLower().StartsWith(name)).ToList();

            var empResult = _employeeService.Mapper.Map<IEnumerable<UserViewModel>>(empQuery);
            var docResult = _doctorService.Mapper.Map<IEnumerable<UserViewModel>>(docQuery);

            var result = new List<UserViewModel>();
            result.AddRange(empResult);
            result.AddRange(docResult);

            return Ok(result);
        }

    }
}
