using HopeBox.Domain.Dtos;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;

namespace HopeBox.Core.IAspModelService
{
    public interface IEventService
    {
        Task<BaseResponseDto<EventDto>> GetNearestEventAsync();
        Task<BaseResponseDto<BasePagingResponseDto<EventDto>>> GetEventsByFilterAsync(EventFilterRequestDto request);
    }
}
