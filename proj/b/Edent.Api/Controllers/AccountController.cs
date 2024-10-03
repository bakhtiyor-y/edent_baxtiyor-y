using Edent.Api.Helpers;
using Edent.Api.Infrastructure;
using Edent.Api.Infrastructure.Auth;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Enums;
using Edent.Api.Infrastructure.Filters;
using Edent.Api.Models;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.Services.Notification.Interfaces;
using Edent.Api.Services.Notification.PlayMobile;
using Edent.Api.Services.Notification.Smtp;
using Edent.Api.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IAuthRequestService _authRequestService;
        private readonly ISmsSender<PlayMobileSmsSender> _smsSender;
        private readonly IEmailSender<SmtpSender> _emailSender;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IJwtFactory jwtFactory,
            ITokenFactory tokenFactory,
            IJwtTokenHandler jwtTokenHandler,
            IRefreshTokenService refreshTokenService,
            IAuthRequestService authRequestService,
            ISmsSender<PlayMobileSmsSender> smsSender,
            IEmailSender<SmtpSender> emailSender,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
            _jwtTokenHandler = jwtTokenHandler;
            _refreshTokenService = refreshTokenService;
            _authRequestService = authRequestService;
            _smsSender = smsSender;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            User user = await _userManager.FindByNameAsync(viewModel.UserName);

            if (user != null && user.IsActive)
            {
                if (await _userManager.CheckPasswordAsync(user, viewModel.Password))
                {
                    var tokenModel = await GetAccessTokenModel(user);
                    _refreshTokenService.Create(new RefreshToken { Name = "SPA Refresh Token", Value = tokenModel.RefreshToken, Expires = DateTime.Now.AddDays(7), UserId = user.Id });
                    return Ok(tokenModel);
                }
                else
                {
                    return BadRequest(new JsonErrorResponse { Messages = new[] { "Invalid username or password." } });
                }
            }
            else
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Invalid username or password." } });
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel model)
        {
            var claimsPrincipal = _jwtTokenHandler.GetPrincipalFromToken(model.AccessToken);
            if (claimsPrincipal == null)
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Invalid access token." } });

            var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == Constants.JwtClaimIdentifiers.Id);
            if (claim == null || string.IsNullOrWhiteSpace(claim.Value))
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Invalid access token." } });

            var user = await _userManager.FindByIdAsync(claim.Value);
            if (user == null)
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Invalid access token." } });


            var refreshToken = _refreshTokenService.Query()
                .AsNoTracking()
                .FirstOrDefault(f => f.UserId == user.Id && f.Value.Equals(model.RefreshToken));

            if (refreshToken == null)
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Invalid refresh token." } });

            if (refreshToken.Expires <= DateTime.Now)
            {
                _refreshTokenService.Delete(refreshToken.Id);
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Invalid refresh token." } });
            }

            _refreshTokenService.Delete(refreshToken.Id);

            var tokenModel = await GetAccessTokenModel(user);
            _refreshTokenService.Create(new RefreshToken { Name = "Mobile Refresh Token", Value = tokenModel.RefreshToken, Expires = DateTime.Now.AddDays(7), UserId = user.Id });
            return Ok(tokenModel);
        }

        [HttpGet("RegisterRequest")]
        public async Task<IActionResult> RegisterRequest(string userName)
        {
            //User user = await _userManager.FindByNameAsync(userName);
            //if (user != null)
            //    return BadRequest(new JsonErrorResponse { Messages = new[] { $"User with username {userName} exists." } });

            //if (Regex.IsMatch(userName, RegexPatterns.PhoneNumber))
            //    user = _userManager.Users.FirstOrDefault(w => w.PhoneNumber.Equals(userName));
            //else
            //    user = await _userManager.FindByEmailAsync(userName);

            //if (user != null)
            //    return BadRequest(new JsonErrorResponse { Messages = new[] { $"User with username {userName} exists." } });

            //bool isTestMode = Convert.ToBoolean(_configuration["IsTestMode"]);
            userName = "harhil46@gmail.com";
            bool isTestMode = false;
            int code = isTestMode ? 3139 : RandomCodeGenerator.GetRandomCode();
            string token = _tokenFactory.GenerateToken();
            var authRequest = _authRequestService.Create(new AuthRequest
            {
                Code = code,
                UserName = userName,
                RequestToken = token,
                ExpireDate = DateTime.Now.AddMinutes(2),
                AuthRequestType = AuthRequestType.RegisterUser
            });

            if (authRequest != null)
            {
                
                var message = new NotificationMessage { Title = "Код подтверждения", Message = $"Код подтверждения: {code}", Recipient = "harhil46@gmail.com" };
                bool result = false;

                if (!isTestMode)
                {
                    if (Regex.IsMatch(userName, RegexPatterns.PhoneNumber))
                        result = await _smsSender.SendAsync(message);
                    else
                        result = await _emailSender.SendAsync(message);
                }
                else
                {
                    result = true;
                }

                if (result)
                    return Ok(new AuthRequestResultViewModel { RequestToken = authRequest.RequestToken });
                else
                    return BadRequest(new JsonErrorResponse { Messages = new[] { "Error on sending request." } });
            }
            else
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Error on create request." } });
            }
            //return BadRequest(new JsonErrorResponse { Messages = new[] { "Error on create request." } }); 
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!_authRequestService.ValidateRequestToken(viewModel.UserName, viewModel.AuthRequestToken, AuthRequestType.RegisterUser))
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Request token not validated" } });

            User user = await _userManager.FindByNameAsync(viewModel.UserName);
            if (user != null)
                return BadRequest(new JsonErrorResponse { Messages = new[] { $"User with username {viewModel.UserName} exists." } });

            if (Regex.IsMatch(viewModel.UserName, RegexPatterns.PhoneNumber))
                user = _userManager.Users.FirstOrDefault(w => w.PhoneNumber.Equals(viewModel.UserName));
            else
                user = await _userManager.FindByEmailAsync(viewModel.UserName);

            if (user != null)
                return BadRequest(new JsonErrorResponse { Messages = new[] { $"User with username {viewModel.UserName} exists." } });

            if (user == null)
            {
                bool isPhoneNumber = Regex.IsMatch(viewModel.UserName, RegexPatterns.PhoneNumber);
                user = new User
                {
                    UserName = viewModel.UserName,
                    PhoneNumber = isPhoneNumber ? viewModel.UserName : "",
                    PhoneNumberConfirmed = isPhoneNumber,
                    Email = isPhoneNumber ? "" : viewModel.UserName,
                    EmailConfirmed = !isPhoneNumber,
                };

                var result = await _userManager.CreateAsync(user, viewModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim("userName", user.UserName));
                    await _userManager.AddClaimAsync(user, new Claim("email", user.Email));
                    await _userManager.AddClaimAsync(user, new Claim("phone", user.PhoneNumber));

                    var tokenModel = await GetAccessTokenModel(user);
                    _refreshTokenService.Create(new RefreshToken { Name = "Mobile Refresh Token", Value = tokenModel.RefreshToken, Expires = DateTime.Now.AddDays(7), UserId = user.Id });
                    return Ok(tokenModel);
                }
                else
                {
                    return BadRequest(new JsonErrorResponse { Messages = new[] { "Error on create user." } });
                }
            }
            else
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { $"User with username {viewModel.UserName} exist." } });
            }
        }

        [HttpGet("RecoverPasswordRequest")]
        public async Task<IActionResult> RecoverPasswordRequest(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);
            if (user != null)
                return BadRequest(new JsonErrorResponse { Messages = new[] { $"User with username {userName} exists." } });

            bool isTestMode = Convert.ToBoolean(_configuration["IsTestMode"]);
            int code = isTestMode ? 3139 : RandomCodeGenerator.GetRandomCode();
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var authRequest = _authRequestService.Create(new AuthRequest
            {
                Code = code,
                UserName = userName,
                RequestToken = token,
                ExpireDate = DateTime.Now.AddMinutes(2),
                AuthRequestType = AuthRequestType.RecoverPassword
            });

            if (authRequest != null)
            {
                var message = new NotificationMessage { Title = "Код подтверждения", Message = $"Код подтверждения: {code}", Recipient = userName };
                bool result = false;

                if (!isTestMode)
                {
                    if (Regex.IsMatch(userName, RegexPatterns.PhoneNumber))
                        result = await _smsSender.SendAsync(message);
                    else
                        result = await _emailSender.SendAsync(message);
                }
                else
                {
                    result = true;
                }

                if (result)
                    return Ok(new AuthRequestResultViewModel { RequestToken = authRequest.RequestToken });
                else
                    return BadRequest(new JsonErrorResponse { Messages = new[] { "Error on sending request." } });
            }
            else
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Error on create request." } });
            }
        }

        [HttpPost("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel viewModel)
        {
            if (!_authRequestService.ValidateRequestToken(viewModel.UserName, viewModel.ResetToken, AuthRequestType.RecoverPassword))
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Reset token not validated" } });

            User user = await _userManager.FindByNameAsync(viewModel.UserName);
            if (user == null)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found." } });
            }
            else
            {
                var result = await _userManager.ResetPasswordAsync(user, viewModel.ResetToken, viewModel.NewPassword);
                if (result.Succeeded)
                {
                    var tokenModel = await GetAccessTokenModel(user);
                    _refreshTokenService.Create(new RefreshToken { Name = "Mobile Refresh Token", Value = tokenModel.RefreshToken, Expires = DateTime.Now.AddDays(7), UserId = user.Id });
                    return Ok(tokenModel);
                }
                else
                {
                    return BadRequest(new JsonErrorResponse { Messages = new[] { "Error on create user." } });
                }
            }
        }

        [HttpPost("ValidateRequest")]
        public IActionResult ValidateRequest(AuthCodeViewModel viewModel)
        {
            AuthRequest authRequest = _authRequestService.GetByAuthCodeViewModel(viewModel);

            if (authRequest == null)
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Authentication request not found." } });

            if (authRequest.ExpireDate <= DateTime.Now)
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Authentication request code life time expired." } });

            authRequest.IsValidated = true;
            _authRequestService.Repository.Edit(authRequest);

            return Ok(new ApiResultModel<bool> { Result = _authRequestService.Repository.SaveChanges() });
        }

        private async Task<AccessTokenModel> GetAccessTokenModel(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>();
            foreach (var item in roles)
            {
                var role = await _roleManager.FindByNameAsync(item);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    if (roleClaims != null && roleClaims.Count > 0)
                        claims.AddRange(roleClaims);
                }
            }
            string accessToken = _jwtFactory.GenerateEncodedToken(user, roles, claims);
            string refreshtoken = _tokenFactory.GenerateToken();

            return new AccessTokenModel(user.UserName, accessToken, refreshtoken, Convert.ToInt32(_configuration["Authentication:Jwt:AccessTokenExpiresIn"]));
        }
    }
}
