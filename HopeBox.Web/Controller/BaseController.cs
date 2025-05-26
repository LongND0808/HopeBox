using HopeBox.Core.IService;
using HopeBox.Domain.ResponseDto;
using Microsoft.AspNetCore.Mvc;

namespace HopeBox.Web.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<TModel, TDto> : ControllerBase
        where TModel : class
        where TDto : class
    {
        protected readonly IBaseService<TModel, TDto> _baseService;

        public BaseController(IBaseService<TModel, TDto> baseService)
        {
            _baseService = baseService;
        }

        [HttpGet("get-all")]
        public virtual async Task<ActionResult<BaseResponseDto<IEnumerable<TDto>>>> GetAll()
        {
            var result = await _baseService.GetAllAsync();
            return StatusCode(result.Status, result);
        }

        [HttpGet("get-by-id")]
        public virtual async Task<ActionResult<BaseResponseDto<TDto>>> GetById([FromQuery] Guid id)
        {
            var result = await _baseService.GetByIdAsync(id);
            return StatusCode(result.Status, result);
        }

        [HttpPost("add")]
        public virtual async Task<ActionResult<BaseResponseDto<bool>>> Add([FromBody] TDto dto)
        {
            var result = await _baseService.AddAsync(dto);
            return StatusCode(result.Status, result);
        }

        [HttpPost("update")]
        public virtual async Task<ActionResult<BaseResponseDto<bool>>> Update([FromBody] TDto dto)
        {
            var result = await _baseService.UpdateAsync(dto);
            return StatusCode(result.Status, result);
        }

        [HttpPost("delete")]
        public virtual async Task<ActionResult<BaseResponseDto<bool>>> Delete([FromBody] Guid id)
        {
            var result = await _baseService.DeleteAsync(id);
            return StatusCode(result.Status, result);
        }
    }
}
