using HopeBox.Core.IService;
using HopeBox.Domain.Dtos;
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
    public class VolunteerController : BaseController<Volunteer, VolunteerDto>
    {
        protected readonly IVolunteerService _volunteerService;

        public VolunteerController(IBaseService<Volunteer, VolunteerDto> service, IVolunteerService volunteerService) : base(service)
        {
            _volunteerService = volunteerService;
        }

        [HttpPost("register-volunteer")]
        public async Task<ActionResult<BaseResponseDto<VolunteerDto>>> Register([FromBody] VolunteerRequestDto request)
        {
            var result = await _volunteerService.RegisterVolunteerAsync(request);
            return StatusCode(result.Status, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("approve-volunteer")]
        public async Task<ActionResult<BaseResponseDto<bool>>> ApproveVolunteer([FromBody] ApproveVolunteerRequestDto request)
        {
            var result = await _volunteerService.ApproveVolunteerAsync(request.VolunteerId, request.CauseId);
            return StatusCode(result.Status, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("reject-volunteer")]
        public async Task<ActionResult<BaseResponseDto<bool>>> RejectVolunteer([FromBody] RejectVolunteerRequestDto request)
        {
            var result = await _volunteerService.RejectVolunteerAsync(request.VolunteerId, request.CauseId);
            return StatusCode(result.Status, result);
        }

        [HttpGet("get-volunteer-details")]
        public async Task<ActionResult<BaseResponseDto<VolunteerDetailDto>>> GetVolunteerDetails([FromQuery] Guid volunteerId)
        {
            var result = await _volunteerService.GetVolunteerDetailsAsync(volunteerId);
            return StatusCode(result.Status, result);
        }
    }
}