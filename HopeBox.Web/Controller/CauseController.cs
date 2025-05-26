using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;
using HopeBox.Domain.ResponseDto;
using HopeBox.Domain.RequestDto;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CauseController : BaseController<Cause, CauseDto>
    {
        private readonly ICauseService _causeService;

        public CauseController(ICauseService causeService) : base(causeService)
        {
            _causeService = causeService;
        }

        [HttpGet("get-cause-one")]
        public virtual async Task<ActionResult<BaseResponseDto<IEnumerable<CauseDto>>>> GetCauseOne()
        {
            var result = await _causeService.GetCauseOneAsync();
            return StatusCode(result.Status, result);
        }

        [HttpGet("get-cause-highest-target")]
        public virtual async Task<ActionResult<BaseResponseDto<IEnumerable<CauseDto>>>> GetCauseHighestTarget()
        {
            var result = await _causeService.GetCauseHighestTargetAsync();
            return StatusCode(result.Status, result);
        }

        [HttpPost("get-cause-by-filter")]
        public async Task<IActionResult> GetCauseByFilter([FromBody] CauseFilterRequestDto request)
        {
            var result = await _causeService.GetCauseByFilter(request);
            return StatusCode(result.Status, result);
        }
    }
}
