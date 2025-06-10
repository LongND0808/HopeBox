using HopeBox.Domain.Dtos;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;

namespace HopeBox.Core.IService
{
    public interface IEventService
    {
        Task<BaseResponseDto<EventDto>> GetNearestEventAsync();
        Task<BaseResponseDto<BasePagingResponseDto<EventDto>>> GetEventsByFilterAsync(EventFilterRequestDto request);
        Task<BaseResponseDto<EventDto>> GetEventWithLocationAsync(Guid eventId);
        Task<BaseResponseDto<List<EventDto>>> GetEventsNearLocationAsync(double latitude, double longitude, double radiusKm);
        Task<BaseResponseDto<bool>> UpdateEventCoordinatesAsync(Guid eventId, double latitude, double longitude);

        // Thêm methods cho OpenMap
        Task<BaseResponseDto<List<OpenMapPlaceSuggestionDto>>> SearchPlacesAsync(string keyword, string? sessionToken = null);
        Task<BaseResponseDto<OpenMapPlaceDetailDto>> GetPlaceDetailAsync(string placeId, string? sessionToken = null);
        Task<BaseResponseDto<bool>> UpdateEventLocationByPlaceIdAsync(Guid eventId, string placeId);
    }
}
