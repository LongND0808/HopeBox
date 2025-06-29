using Duende.IdentityModel;
using HopeBox.Core.IService;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

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
            return (result);
        }

        [HttpGet("get-cause-highest-target")]
        public virtual async Task<BaseResponseDto<CauseDto>> GetCauseHighestTarget()
        {
            var result = await _causeService.GetCauseHighestTargetAsync();
            return (result);
        }

        [HttpPost("get-cause-by-filter")]
        public async Task<BaseResponseDto<BasePagingResponseDto<CauseDto>>> GetCauseByFilter([FromBody] CauseFilterRequestDto request)
        {
            var result = await _causeService.GetCauseByFilter(request);
            return (result);
        }

        [HttpGet("get-cause-revenue")]
        [Authorize(Roles = "Admin")]
        public async Task<BaseResponseDto<IEnumerable<CauseRevenueResponseDto>>> GetCauseRevenue()
        {
            var result = await _causeService.GetCauseRevenueAsync();
            return result;
        }

        [HttpPost("get-cause-by-filter-with-user-status")]
        [Authorize]
        public async Task<BaseResponseDto<BasePagingResponseDto<CauseWithVolunteerStatusDto>>> GetCauseByFilterWithUserStatus(
    [FromBody] CauseFilterRequestDto request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Id);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return new BaseResponseDto<BasePagingResponseDto<CauseWithVolunteerStatusDto>>
                {
                    Status = 401,
                    Message = "Unauthorized",
                    ResponseData = null
                };
            }

            var requestWithUser = new CauseFilterWithUserRequestDto
            {
                Name = request.Name,
                CauseType = request.CauseType,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                UserId = userId
            };

            var result = await _causeService.GetCauseByFilterWithUserStatus(requestWithUser);
            return result;
        }

        [HttpGet("get-most-urgent-cause")]
        public async Task<BaseResponseDto<CauseDto>> GetMostUrgentCause()
        {
            return await _causeService.GetMostUrgentCauseAsync();
        }

        [HttpPost("change-hero-image")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeHeroImage(string causeId, IFormFile file)
        {
            var result = await _causeService.ChangeHeroImageAsync(Guid.Parse(causeId), file);
            return Ok(result);
        }

        [HttpPost("change-challenge-image")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeChallengeImage(string causeId, IFormFile file)
        {
            var result = await _causeService.ChangeChallengeImageAsync(Guid.Parse(causeId), file);
            return Ok(result);
        }

        [HttpPost("change-summary-image")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeSummaryImage(string causeId, IFormFile file)
        {
            var result = await _causeService.ChangeSummaryImageAsync(Guid.Parse(causeId), file);
            return Ok(result);
        }
    }
}
