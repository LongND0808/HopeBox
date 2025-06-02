using HopeBox.Core.IAspModelService;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using MailKit;

namespace HopeBox.Core.AspModelService
{
    public class EventService : IEventService
    {    
        protected readonly IRepository<Event> _repository;
        protected readonly IConverter<Event, EventDto> _converter;

        public EventService(IRepository<Event> repository, IConverter<Event, EventDto> converter)
        {
            _repository = repository;
            _converter = converter;
        }

        #region GetNearestEvent
        /// <summary>
        /// Lấy các sự kiện sắp diễn ra gần nhất.
        /// Và bỏ quá qua các sự kiện đã kết thúc.
        /// </summary>
        public async Task<BaseResponseDto<EventDto>> GetNearestEventAsync()
        {
            try
            {
                var today = DateTime.Now;
                var events = await _repository.GetListAsyncUntracked<Event>(
                    filter: e => e.EndDate >= today,
                    orderBy: q => q.OrderBy(e => e.StartDate)
                );
                var nearestEvent = events.FirstOrDefault();

                if (nearestEvent == null)
                {
                    return new BaseResponseDto<EventDto>
                    {
                        Status = 404,
                        Message = "No upcoming events.",
                        ResponseData = null
                    };
                }

                var dto = _converter.ToDTO(nearestEvent);
                return new BaseResponseDto<EventDto>
                {
                    Status = 200,
                    Message = "Success",
                    ResponseData = dto
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<EventDto>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }
        #endregion

    }
}
