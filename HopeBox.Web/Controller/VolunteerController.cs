using HopeBox.Core.IService;
using HopeBox.Core.Service;
using HopeBox.Core.Token;
using HopeBox.Domain.Dtos;
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
        protected readonly ITokenService _tokenService;

        public VolunteerController(IBaseService<Volunteer, VolunteerDto> baserservice, IVolunteerService volunteerService, ITokenService tokenService) : base(baserservice)
        {
            _volunteerService = volunteerService;
            _tokenService = tokenService;
        }

        [HttpPost("register-volunteer")]
        [Authorize]
        public async Task<ActionResult<BaseResponseDto<VolunteerDto>>> Register([FromBody] VolunteerRequestDto request)
        {
            // Debug token
            var token = _tokenService.GetCurrentAccessToken();
            Console.WriteLine($"Token received: {token?.Substring(0, Math.Min(20, token.Length))}...");

            var result = await _volunteerService.RegisterVolunteerAsync(request);
            return StatusCode(result.Status, result);
        }
    }
}
