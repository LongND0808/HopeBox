using HopeBox.Core.IService;
using HopeBox.Core.Service;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using Microsoft.AspNetCore.Mvc;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerController : BaseController<Volunteer, VolunteerDto>
    {
        protected readonly IVolunteerService _volunteerService;

        public VolunteerController(IBaseService<Volunteer, VolunteerDto> service, IVolunteerService volunteerservice) : base(service)
        {
            _volunteerService = volunteerservice;
        }
        
        [HttpPost("register-volunteer")]
        public async Task<ActionResult<BaseResponseDto<VolunteerDto>>> Register([FromBody] VolunteerRequestDto request)
        {
            var result = await _volunteerService.RegisterVolunteerAsync(request);
            return StatusCode(result.Status, result);
        }
    }
}
