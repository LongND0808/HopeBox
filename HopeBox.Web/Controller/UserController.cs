using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;
using HopeBox.Domain.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using HopeBox.Core.IAspModelService;
using Duende.IdentityModel;
using HopeBox.Core.Service;
using HopeBox.Domain.RequestDto;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-all")]
        public virtual async Task<BaseResponseDto<IEnumerable<UserDto>>> GetAll()
        {
            var result = await _userService.GetAllAsync();
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-manager")]
        public virtual async Task<BaseResponseDto<IEnumerable<UserDto>>> GetManager()
        {
            var result = await _userService.GetManagerAsync();
            return result;
        }

        [HttpGet("get-by-id")]
        public virtual async Task<BaseResponseDto<UserDto>> GetById([FromQuery] Guid id)
        {
            var result = await _userService.GetByIdAsync(id);
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public virtual async Task<BaseResponseDto<bool>> Add([FromBody] UserDto dto)
        {
            var result = await _userService.AddAsync(dto);
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update")]
        public virtual async Task<BaseResponseDto<bool>> Update([FromBody] UserDto dto)
        {
            var result = await _userService.UpdateAsync(dto);
            return result;
        }

        [HttpPost("update-user-info")]
        public virtual async Task<BaseResponseDto<bool>> UpdateUserInfo([FromBody] UpdateUserInfoRequestDto dto)
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Id);
            dto.UserId = Guid.Parse(userId ?? throw new InvalidOperationException("User ID not found in claims"));
            var result = await _userService.UpdateUserInfoAsync(dto);
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("delete")]
        public virtual async Task<BaseResponseDto<bool>> Delete([FromBody] Guid id)
        {
            var result = await _userService.DeleteAsync(id);
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-count")]
        public virtual async Task<BaseResponseDto<int>> GetCount()
        {
            var result = await _userService.GetCountAsync();
            return (result);
        }

        [Authorize]
        [HttpPost("change-avatar")]
        public async Task<BaseResponseDto<string>> ChangeAvatar(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new BaseResponseDto<string> { Status = 400, Message = "File rỗng" };

            var userId = User.FindFirstValue(JwtClaimTypes.Id);
            if (userId == null)
                return new BaseResponseDto<string> { Status = 401, Message = "Không xác thực được người dùng" };

            var result = await _userService.ChangeAvatarAsync(Guid.Parse(userId), file);
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("admin-change-avatar")]
        public async Task<BaseResponseDto<string>> AdminChangeAvatar(string userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new BaseResponseDto<string> { Status = 400, Message = "File rỗng" };

            var result = await _userService.ChangeAvatarAsync(Guid.Parse(userId), file);
            return result;
        }
    }
}
