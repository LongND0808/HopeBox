using HopeBox.Core.IService;
using HopeBox.Domain.DTOs;
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

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("get-by-nearest-event")]
        public virtual async Task<ActionResult<BaseResponseDto<EventDto>>> GetByNearestEvent()
        {
            var result = await _eventService.GetNearestEventAsync();
            return StatusCode(result.Status, result);
        }

        [HttpGet("get-upcoming-events")]
        public virtual async Task<ActionResult<BaseResponseDto<List<EventDto>>>> GetUpcomingEvents()
        {
            var result = await _eventService.GetUpcomingEventsAsync();
            return StatusCode(result.Status, result);
        }

        [HttpGet("get-event-by-filter")]
        public async Task<IActionResult> GetEventsByFilter([FromQuery] EventFilterRequestDto request)
        {
            var result = await _eventService.GetEventsByFilterAsync(request);
            return StatusCode(result.Status, result);
        }

        [HttpGet("get-events-donation-detail-by-filter")]
        public async Task<ActionResult<BaseResponseDto<BasePagingResponseDto<EventDonationDetailDto>>>> GetEventsDonationDetailByFilter(
    [FromQuery] EventFilterRequestDto request)
        {
            var result = await _eventService.GetEventsDonationDetailByFilterAsync(request);
            return StatusCode(result.Status, result);
        }

        [HttpGet("get-event-donation-detail/{id}")]
        public async Task<ActionResult<BaseResponseDto<EventDonationDetailDto>>> GetEventDonationDetailById(Guid id)
        {
            var result = await _eventService.GetEventDonationDetailByIdAsync(id);
            return StatusCode(result.Status, result);
        }

        [HttpGet("get-event-with-location/{id}")]
        public async Task<ActionResult<BaseResponseDto<EventDto>>> GetEventWithLocation(Guid id)
        {
            var result = await _eventService.GetEventWithLocationAsync(id);
            return StatusCode(result.Status, result);
        }

        [HttpGet("get-events-near-location")]
        public async Task<ActionResult<BaseResponseDto<List<EventDto>>>> GetEventsNearLocation(
            [FromQuery] double latitude,
            [FromQuery] double longitude,
            [FromQuery] double radiusKm = 10)
        {
            var result = await _eventService.GetEventsNearLocationAsync(latitude, longitude, radiusKm);
            return StatusCode(result.Status, result);
        }

        [HttpPut("update-coordinates/{id}")]
        public async Task<ActionResult<BaseResponseDto<bool>>> UpdateEventCoordinates(
            Guid id,
            [FromBody] UpdateCoordinatesRequestDto request)
        {
            var result = await _eventService.UpdateEventCoordinatesAsync(id, request.Latitude, request.Longitude);
            return StatusCode(result.Status, result);
        }

        [HttpGet("search-places")]
        public async Task<ActionResult<BaseResponseDto<List<OpenMapPlaceSuggestionDto>>>> SearchPlaces(
            [FromQuery] string keyword,
            [FromQuery] string? sessionToken = null)
        {
            var result = await _eventService.SearchPlacesAsync(keyword, sessionToken);
            return StatusCode(result.Status, result);
        }

        [HttpGet("place-detail/{placeId}")]
        public async Task<ActionResult<BaseResponseDto<OpenMapPlaceDetailDto>>> GetPlaceDetail(
            string placeId,
            [FromQuery] string? sessionToken = null)
        {
            var result = await _eventService.GetPlaceDetailAsync(placeId, sessionToken);
            return StatusCode(result.Status, result);
        }

        [HttpPut("update-location-by-place/{id}")]
        public async Task<ActionResult<BaseResponseDto<bool>>> UpdateEventLocationByPlace(
            Guid id,
            [FromBody] UpdateLocationByPlaceRequestDto request)
        {
            var result = await _eventService.UpdateEventLocationByPlaceIdAsync(id, request.PlaceId);
            return StatusCode(result.Status, result);
        }

        [HttpGet("test-openmap")]
        public async Task<IActionResult> TestOpenMap()
        {
            try
            {
                var suggestions = await _eventService.SearchPlacesAsync("Hà Nội");
                return Ok(new
                {
                    message = "OpenMap API working",
                    suggestionsCount = suggestions.ResponseData?.Count ?? 0
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "OpenMap API failed", error = ex.Message });
            }
        }
    }
}
