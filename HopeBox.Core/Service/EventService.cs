using HopeBox.Core.IService;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace HopeBox.Core.Service
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

                var nearestEvent = (await _repository.GetListAsyncUntracked<Event>(
                    filter: e => e.EndDate >= today,
                    include: query => query.Include(e => e.Creator)
                                            .Include(e => e.Organization),
                    orderBy: q => q.OrderBy(e => e.StartDate)
                )).FirstOrDefault();

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
                dto.CreatedByName = nearestEvent.Creator?.FullName ?? "Unknown";
                dto.OrganizationName = nearestEvent.Organization?.Name ?? "Unknown";

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

        #region GetEventsByFilter
        /// <summary>
        /// Sử dụng để lấy danh sách các sự kiện theo bộ lọc (Title).
        /// Điểm cần lưu ý là bộ lọc này sẽ trả về các sự kiện đã kết thúc, vì vậy cần phải kiểm tra ngày kết thúc của sự kiện trong quá trình hiển thị.
        /// Chỉ lấy các sự kiện có tiêu đề chứa từ khóa tìm kiếm.
        /// </summary>
        public async Task<BaseResponseDto<BasePagingResponseDto<EventDto>>> GetEventsByFilterAsync(EventFilterRequestDto request)
        {
            try
            {
                var filter = PredicateBuilder.New<Event>(true);

                if (!string.IsNullOrWhiteSpace(request.Title))
                {
                    filter = filter.And(e => EF.Functions.Like(e.Title, $"%{request.Title}%"));
                }

                var totalRecords = await _repository.GetCount(filter);
                var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);

                var entities = await _repository.GetListAsyncUntracked<Event>(
                    filter: filter,
                    include: query => query.Include(e => e.Creator)
                                           .Include(e => e.Organization),
                    pageSize: request.PageSize,
                    pageNumber: request.PageIndex,
                    orderBy: q => q.OrderByDescending(e => e.StartDate)
                );

                var dtos = entities.Select(e =>
                {
                    var dto = _converter.ToDTO(e);
                    dto.CreatedByName = e.Creator?.FullName ?? "Unknown";
                    dto.OrganizationName = e.Organization?.Name ?? "Unknown";
                    return dto;
                }).ToList();

                var pagingResponse = new BasePagingResponseDto<EventDto>
                {
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    PagedData = dtos
                };

                return new BaseResponseDto<BasePagingResponseDto<EventDto>>
                {
                    Status = 200,
                    Message = "Lấy danh sách sự kiện thành công",
                    ResponseData = pagingResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<BasePagingResponseDto<EventDto>>
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
