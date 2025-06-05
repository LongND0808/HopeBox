using HopeBox.Core.IService;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.ResponseDto;
using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.Data;
using HopeBox.Domain.RequestDto;
using Azure;
using Duende.IdentityModel;
using HopeBox.Core.IAspModelService;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IAuthenticationService authenticationService, IUserService userService, IConfiguration configuration)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<BaseResponseDto<LoginResponseDto>> Login([FromBody] LoginRequestDto loginDto)
        {
            var response = await _authenticationService.Login(loginDto);

            if (response.Status != 200)
            {
                return (response);
            }

            var accessToken = response.ResponseData?.AccessToken;
            var refreshToken = response.ResponseData?.RefreshToken;

            var accessTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(int.TryParse(_configuration["JWT:TokenValidityInHours"], out int hours) ? hours : 8),
                Path = "/"
            };

            var refreshTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int validity) ? validity : 7),
                Path = "/"
            };

            Response.Cookies.Append("accessToken", accessToken, accessTokenCookieOptions);
            Response.Cookies.Append("refreshToken", refreshToken, refreshTokenCookieOptions);

            return (new BaseResponseDto<LoginResponseDto>
            {
                Status = 200,
                Message = "Login successfully.",
                ResponseData = null
            });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<BaseResponseDto<UserDto>> Me()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Id);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return new BaseResponseDto<UserDto>
                {
                    Status = 401,
                    Message = "Unauthorized",
                    ResponseData = null
                };
            }

            var result = await _userService.GetByIdAsync(userId);
            return result;
        }


        [HttpPost("register")]
        public async Task<BaseResponseDto<bool>> Register([FromBody] RegisterRequestDto registerDto)
        {
            return await _authenticationService.Register(registerDto);
        }

        [HttpPost("confirm-email")]
        public async Task<BaseResponseDto<bool>> ConfirmEmail([FromBody] ConfirmEmailRequestDto registerDto)
        {
            return await _authenticationService.ConfirmEmail(registerDto);
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTime.Now.AddDays(-1)
            };

            Response.Cookies.Append("accessToken", "", cookieOptions);
            Response.Cookies.Append("refreshToken", "", cookieOptions);

            return Ok(new BaseResponseDto<bool>
            {
                Status = 200,
                Message = "Đăng xuất thành công",
                ResponseData = true
            });
        }
    }
}
