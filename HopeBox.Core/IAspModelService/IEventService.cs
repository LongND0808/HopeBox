using HopeBox.Domain.Dtos;
using HopeBox.Domain.ResponseDto;

namespace HopeBox.Core.IAspModelService
{
    public interface IEventService
    {
        Task<BaseResponseDto<EventDto>> GetNearestEventAsync();
    }
}
