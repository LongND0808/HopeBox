using HopeBox.Core.IService;
using HopeBox.Domain.DTOs;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class InkindDonationController : BaseController<InkindDonation, InkindDonationDto>
    {
        protected readonly IInkindDonationService _inkindDonationService;

        public InkindDonationController(IBaseService<InkindDonation, InkindDonationDto> service, IInkindDonationService inkindDonationService) : base(service)
        {
            _inkindDonationService = inkindDonationService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<BaseResponseDto<InkindDonationDto>>> CreateInkindDonation(
            [FromBody] CreateInkindDonationRequestDto request)
        {
            var userId = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new BaseResponseDto<InkindDonationDto>
                {
                    Status = 401,
                    Message = "User ID not found in access token",
                    ResponseData = null
                });
            }

            var result = await _inkindDonationService.CreateInkindDonationAsync(Guid.Parse(userId), request);
            return StatusCode(result.Status, result);
        }

        [Authorize]
        [HttpPut("cancel/{id}")]
        public async Task<ActionResult<BaseResponseDto<bool>>> CancelInkindDonation(Guid id)
        {
            var userId = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _inkindDonationService.CancelInkindDonationAsync(Guid.Parse(userId), id);
            return StatusCode(result.Status, result);
        }

        [Authorize(Roles = "Admin,Organization")]
        [HttpPut("approve/{id}")]
        public async Task<ActionResult<BaseResponseDto<bool>>> ApproveInkindDonation(
            Guid id, [FromBody] string? notes = null)
        {
            var userId = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _inkindDonationService.ApproveInkindDonationAsync(Guid.Parse(userId), id, notes);
            return StatusCode(result.Status, result);
        }

        [Authorize]
        [HttpGet("my-donations")]
        public async Task<ActionResult<BaseResponseDto<BasePagingResponseDto<InkindDonationDto>>>> GetMyInkindDonations(
            [FromQuery] InkindDonationFilterRequestDto request)
        {
            var userId = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new BaseResponseDto<BasePagingResponseDto<InkindDonationDto>>
                {
                    Status = 401,
                    Message = "User ID not found in access token",
                    ResponseData = null
                });
            }

            var result = await _inkindDonationService.GetUserInkindDonationsAsync(Guid.Parse(userId), request);
            return StatusCode(result.Status, result);
        }

        [Authorize(Roles = "Admin,Organization")]
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<BaseResponseDto<BasePagingResponseDto<InkindDonationDetailDto>>>> GetEventInkindDonations(
            Guid eventId, [FromQuery] InkindDonationFilterRequestDto request)
        {
            var result = await _inkindDonationService.GetEventInkindDonationsAsync(eventId, request);
            return StatusCode(result.Status, result);
        }
    }
}
