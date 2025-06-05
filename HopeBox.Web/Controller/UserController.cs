using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;
using HopeBox.Domain.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using HopeBox.Core.IAspModelService;
using Duende.IdentityModel;

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

        [Authorize(Roles = "Admin")]
        [HttpPost("delete")]
        public virtual async Task<BaseResponseDto<bool>> Delete([FromBody] Guid id)
        {
            var result = await _userService.DeleteAsync(id);
            return result;
        }
    }
}
