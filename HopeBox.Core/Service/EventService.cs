using HopeBox.Common.Enum;
using HopeBox.Core.IService;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HopeBox.Core.Service
{
    public class EventService : IEventService
    {
        protected readonly IRepository<Event> _repository;
        protected readonly IConverter<Event, EventDto> _converter;
        protected readonly IOpenMapService _openMapService;
        protected readonly ILogger<EventService> _logger;

        public EventService(
            IRepository<Event> repository,
            IConverter<Event, EventDto> converter,
            IOpenMapService openMapService,
            ILogger<EventService> logger)
        {
            _repository = repository;
            _converter = converter;
            _openMapService = openMapService;
            _logger = logger;
        }

        #region GetNearestEvent
        public async Task<BaseResponseDto<EventDto>> GetNearestEventAsync()
        {
            try
            {
                var today = DateTime.Now;

                var nearestEvent = (await _repository.GetListAsyncUntracked<Event>(
                    filter: e => e.EndDate >= today,
                    include: query => query.AsQueryable()
                                           .Include(e => e.Creator)
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

                var dto = await ConvertEventToDtoWithLocationAsync(nearestEvent);

                return new BaseResponseDto<EventDto>
                {
                    Status = 200,
                    Message = "Success",
                    ResponseData = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting nearest event");
                return new BaseResponseDto<EventDto>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }
        #endregion

        #region GetUpcomingEvents
        public async Task<BaseResponseDto<List<EventDto>>> GetUpcomingEventsAsync()
        {
            try
            {
                _logger.LogInformation("Getting upcoming events started");

                var today = DateTime.Now;

                var filter = PredicateBuilder.New<Event>(true);

                filter = filter.And(e => e.EndDate >= today);

                filter = filter.And(e => e.Status == Enumerate.EventStatus.Upcoming ||
                                        e.Status == Enumerate.EventStatus.Ongoing);

                var upcomingEvents = await _repository.GetListAsyncUntracked<Event>(
                    filter: filter,
                    include: query => query.AsQueryable()
                                           .Include(e => e.Creator)
                                           .Include(e => e.Organization),
                    orderBy: q => q.OrderBy(e => e.StartDate),
                    pageSize: 3,
                    pageNumber: 1
                );

                _logger.LogInformation("Found {Count} upcoming events", upcomingEvents.Count());

                var eventDtos = new List<EventDto>();
                foreach (var eventEntity in upcomingEvents)
                {
                    var dto = await ConvertEventToDtoWithLocationAsync(eventEntity);
                    eventDtos.Add(dto);
                }

                return new BaseResponseDto<List<EventDto>>
                {
                    Status = 200,
                    Message = eventDtos.Count > 0
                        ? $"Lấy thành công {eventDtos.Count} sự kiện sắp tới"
                        : "Không có sự kiện nào sắp tới",
                    ResponseData = eventDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting upcoming events");
                return new BaseResponseDto<List<EventDto>>
                {
                    Status = 500,
                    Message = $"Lỗi hệ thống: {ex.Message}",
                    ResponseData = null
                };
            }
        }
        #endregion

        #region GetEventsByFilter
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
                    include: query => query.AsQueryable()
                                           .Include(e => e.Creator)
                                           .Include(e => e.Organization),
                    pageSize: request.PageSize,
                    pageNumber: request.PageIndex,
                    orderBy: q => q.OrderByDescending(e => e.StartDate)
                );

                var dtos = new List<EventDto>();
                foreach (var entity in entities)
                {
                    var dto = await ConvertEventToDtoWithLocationAsync(entity);
                    dtos.Add(dto);
                }

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
                _logger.LogError(ex, "Error occurred while getting events by filter");
                return new BaseResponseDto<BasePagingResponseDto<EventDto>>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }
        #endregion

        #region GetEventWithLocation
        public async Task<BaseResponseDto<EventDto>> GetEventWithLocationAsync(Guid eventId)
        {
            try
            {
                var eventEntity = await _repository.GetByIdAsync(eventId,
                    include: query => query.AsQueryable()
                                           .Include(e => e.Creator)
                                           .Include(e => e.Organization));

                if (eventEntity == null)
                {
                    return new BaseResponseDto<EventDto>
                    {
                        Status = 404,
                        Message = "Event not found",
                        ResponseData = null
                    };
                }

                var dto = await ConvertEventToDtoWithLocationAsync(eventEntity);

                return new BaseResponseDto<EventDto>
                {
                    Status = 200,
                    Message = "Success",
                    ResponseData = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting event with location: {EventId}", eventId);
                return new BaseResponseDto<EventDto>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }
        #endregion

        #region GetEventsNearLocation
        public async Task<BaseResponseDto<List<EventDto>>> GetEventsNearLocationAsync(double latitude, double longitude, double radiusKm)
        {
            try
            {
                var events = await _repository.GetListAsyncUntracked<Event>(
                    filter: e => e.Latitude.HasValue && e.Longitude.HasValue &&
                                Math.Sqrt(
                                    Math.Pow(111.32 * (e.Latitude.Value - latitude), 2) +
                                    Math.Pow(111.32 * (longitude - e.Longitude.Value) * Math.Cos(latitude * Math.PI / 180), 2)
                                ) <= radiusKm,
                    include: query => query.AsQueryable()
                                           .Include(e => e.Creator)
                                           .Include(e => e.Organization),
                    orderBy: q => q.OrderBy(e => e.StartDate)
                );

                var dtos = new List<EventDto>();
                foreach (var eventEntity in events)
                {
                    var dto = await ConvertEventToDtoWithLocationAsync(eventEntity);
                    dtos.Add(dto);
                }

                return new BaseResponseDto<List<EventDto>>
                {
                    Status = 200,
                    Message = $"Found {dtos.Count} events within {radiusKm}km",
                    ResponseData = dtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting events near location: {Lat}, {Lng}", latitude, longitude);
                return new BaseResponseDto<List<EventDto>>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }
        #endregion

        #region UpdateEventCoordinates
        public async Task<BaseResponseDto<bool>> UpdateEventCoordinatesAsync(Guid eventId, double latitude, double longitude)
        {
            try
            {
                var eventEntity = await _repository.GetByIdAsync(eventId);
                if (eventEntity == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Event not found",
                        ResponseData = false
                    };
                }

                var isValid = await _openMapService.ValidateCoordinatesAsync(latitude, longitude);
                if (!isValid)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 400,
                        Message = "Invalid coordinates",
                        ResponseData = false
                    };
                }

                var formattedAddress = await _openMapService.GetAddressFromCoordinatesAsync(latitude, longitude);

                eventEntity.Latitude = latitude;
                eventEntity.Longitude = longitude;
                eventEntity.FormattedAddress = formattedAddress;

                await _repository.UpdateAsync(eventEntity);

                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Event coordinates updated successfully",
                    ResponseData = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating event coordinates: {EventId}", eventId);
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = false
                };
            }
        }
        #endregion

        #region SearchPlaces
        public async Task<BaseResponseDto<List<OpenMapPlaceSuggestionDto>>> SearchPlacesAsync(string keyword, string? sessionToken = null)
        {
            try
            {
                var suggestions = await _openMapService.AutocompleteAsync(keyword, sessionToken);

                return new BaseResponseDto<List<OpenMapPlaceSuggestionDto>>
                {
                    Status = 200,
                    Message = $"Found {suggestions.Count} place suggestions",
                    ResponseData = suggestions
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching places with keyword: {Keyword}", keyword);
                return new BaseResponseDto<List<OpenMapPlaceSuggestionDto>>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }
        #endregion

        #region GetPlaceDetail
        public async Task<BaseResponseDto<OpenMapPlaceDetailDto>> GetPlaceDetailAsync(string placeId, string? sessionToken = null)
        {
            try
            {
                var placeDetail = await _openMapService.GetPlaceDetailAsync(placeId, sessionToken);

                if (placeDetail == null)
                {
                    return new BaseResponseDto<OpenMapPlaceDetailDto>
                    {
                        Status = 404,
                        Message = "Place not found",
                        ResponseData = null
                    };
                }

                return new BaseResponseDto<OpenMapPlaceDetailDto>
                {
                    Status = 200,
                    Message = "Success",
                    ResponseData = placeDetail
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting place detail: {PlaceId}", placeId);
                return new BaseResponseDto<OpenMapPlaceDetailDto>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }
        #endregion

        #region UpdateEventLocationByPlaceId
        public async Task<BaseResponseDto<bool>> UpdateEventLocationByPlaceIdAsync(Guid eventId, string placeId)
        {
            try
            {
                var eventEntity = await _repository.GetByIdAsync(eventId);
                if (eventEntity == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Event not found",
                        ResponseData = false
                    };
                }

                var placeDetail = await _openMapService.GetPlaceDetailAsync(placeId);
                if (placeDetail == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Place not found",
                        ResponseData = false
                    };
                }

                eventEntity.Location = placeDetail.Label;
                eventEntity.Latitude = placeDetail.Latitude;
                eventEntity.Longitude = placeDetail.Longitude;
                eventEntity.FormattedAddress = placeDetail.Label;

                await _repository.UpdateAsync(eventEntity);

                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Event location updated successfully",
                    ResponseData = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating event location by place ID: {EventId}, {PlaceId}", eventId, placeId);
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = false
                };
            }
        }
        #endregion

        #region Private Helper Methods
        private async Task<EventDto> ConvertEventToDtoWithLocationAsync(Event eventEntity)
        {
            var dto = _converter.ToDTO(eventEntity);
            dto.CreatedByName = eventEntity.Creator?.FullName ?? "Unknown";
            dto.OrganizationName = eventEntity.Organization?.Name ?? "Unknown";

            if ((!eventEntity.Latitude.HasValue || !eventEntity.Longitude.HasValue) &&
                !string.IsNullOrEmpty(eventEntity.Location))
            {
                var (lat, lng, formattedAddress) = await _openMapService.GetCoordinatesFromAddressAsync(eventEntity.Location);

                if (lat.HasValue && lng.HasValue)
                {
                    dto.Latitude = lat;
                    dto.Longitude = lng;
                    dto.FormattedAddress = formattedAddress;

                    try
                    {
                        eventEntity.Latitude = lat;
                        eventEntity.Longitude = lng;
                        eventEntity.FormattedAddress = formattedAddress;
                        await _repository.UpdateAsync(eventEntity);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to cache coordinates for event: {EventId}", eventEntity.Id);
                    }
                }
            }

            return dto;
        }
        #endregion
    }
}
