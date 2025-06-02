using HopeBox.Core.IAspModelService;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using Microsoft.AspNetCore.Mvc;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        protected readonly IEventService _eventService;

        public EventController(IEventService baseService)
        {
            _eventService = baseService;
        }

        [HttpGet("get-by-nearest-event")]
        public virtual async Task<ActionResult<BaseResponseDto<EventDto>>> GetByNearestEvent()
        {
            var result = await _eventService.GetNearestEventAsync();
            return StatusCode(result.Status, result);
        }

        [HttpGet("get-event-by-filter")]
        public async Task<IActionResult> GetEventsByFilter([FromQuery] EventFilterRequestDto request)
        {
            var result = await _eventService.GetEventsByFilterAsync(request);
            return StatusCode(result.Status, result);
        }
    }
}
