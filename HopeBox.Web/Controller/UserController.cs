using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;
using HopeBox.Domain.ResponseDto;
using HopeBox.Core.IAspModelService;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected readonly IUserService _userService;

        public UserController(IUserService baseService)
        {
            _userService = baseService;
        }

        [HttpGet("get-all")]
        public virtual async Task<ActionResult<BaseResponseDto<IEnumerable<UserDto>>>> GetAll()
        {
            var result = await _userService.GetAllAsync();
            return StatusCode(result.Status, result);
        }

        [HttpGet("get-by-id")]
        public virtual async Task<ActionResult<BaseResponseDto<UserDto>>> GetById([FromQuery] Guid id)
        {
            var result = await _userService.GetByIdAsync(id);
            return StatusCode(result.Status, result);
        }

        [HttpPost("add")]
        public virtual async Task<ActionResult<BaseResponseDto<bool>>> Add([FromBody] UserDto dto)
        {
            var result = await _userService.AddAsync(dto);
            return StatusCode(result.Status, result);
        }

        [HttpPost("update")]
        public virtual async Task<ActionResult<BaseResponseDto<bool>>> Update([FromBody] UserDto dto)
        {
            var result = await _userService.UpdateAsync(dto);
            return StatusCode(result.Status, result);
        }

        [HttpPost("delete")]
        public virtual async Task<ActionResult<BaseResponseDto<bool>>> Delete([FromBody] Guid id)
        {
            var result = await _userService.DeleteAsync(id);
            return StatusCode(result.Status, result);
        }
    }
}
