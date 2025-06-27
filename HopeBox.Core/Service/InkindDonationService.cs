using HopeBox.Core.IService;
using HopeBox.Domain.Converter;
using HopeBox.Domain.DTOs;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Core.Service
{
    public class InkindDonationService : BaseService<InkindDonation, InkindDonationDto>, IInkindDonationService
    {
        private readonly IRepository<InkindDonation> _inkindDonationRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<InkindDonationService> _logger;
        public InkindDonationService(
            IRepository<InkindDonation> inkindDonationRepository,
            IConverter<InkindDonation, InkindDonationDto> converter,
            IRepository<InkindDonation> repository,
            IRepository<Event> eventRepository,
            IRepository<User> userRepository,
            ILogger<InkindDonationService> logger)
        : base(inkindDonationRepository, converter)
        {
            _inkindDonationRepository = inkindDonationRepository;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        #region CreateInkindDonation
        public async Task<BaseResponseDto<InkindDonationDto>> CreateInkindDonationAsync(
            Guid userId, CreateInkindDonationRequestDto request)
        {
            try
            {
                _logger.LogInformation("Creating inkind donation for user {UserId} and event {EventId}", userId, request.EventId);

                var eventEntity = await _eventRepository.GetOneAsyncUntracked<Event>(
                    filter: e => e.Id == request.EventId,
                    include: q => q.Include(e => e.Cause)
                );

                if (eventEntity == null)
                {
                    return new BaseResponseDto<InkindDonationDto>
                    {
                        Status = 404,
                        Message = "Không tìm thấy sự kiện",
                        ResponseData = null
                    };
                }

                if (eventEntity.Status != EventStatus.Upcoming && eventEntity.Status != EventStatus.Ongoing)
                {
                    return new BaseResponseDto<InkindDonationDto>
                    {
                        Status = 400,
                        Message = "Sự kiện không còn nhận quyên góp",
                        ResponseData = null
                    };
                }

                var user = await _userRepository.GetOneAsyncUntracked<User>(filter: u => u.Id == userId);
                if (user == null)
                {
                    return new BaseResponseDto<InkindDonationDto>
                    {
                        Status = 404,
                        Message = "Không tìm thấy thông tin người dùng",
                        ResponseData = null
                    };
                }

                var inkindDonation = new InkindDonation
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    EventId = request.EventId,
                    InKind = request.InKind,
                    SenderAddress = request.SenderAddress,
                    ReceiverAddress = eventEntity.Location ?? "Địa điểm sự kiện",
                    Message = request.Message,
                    IsAnonymous = request.IsAnonymous,
                    EstimatedDeliveryDate = request.EstimatedDeliveryDate,
                    DonationDate = DateTime.UtcNow,
                    Status = InkindDonationStatus.Pending
                };

                await _repository.AddAsync(inkindDonation);

                var result = _converter.ToDTO(inkindDonation);

                _logger.LogInformation("Successfully created inkind donation {DonationId}", inkindDonation.Id);

                return new BaseResponseDto<InkindDonationDto>
                {
                    Status = 201,
                    Message = "Tạo quyên góp hiện vật thành công",
                    ResponseData = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating inkind donation for user {UserId}", userId);
                return new BaseResponseDto<InkindDonationDto>
                {
                    Status = 500,
                    Message = $"Lỗi hệ thống: {ex.Message}",
                    ResponseData = null
                };
            }
        }
        #endregion

        #region GetUserInkindDonations - FIXED
        public async Task<BaseResponseDto<BasePagingResponseDto<InkindDonationDto>>> GetUserInkindDonationsAsync(
            Guid userId, InkindDonationFilterRequestDto request)
        {
            try
            {
                _logger.LogInformation("Getting user inkind donations for user {UserId}", userId);

                var filter = PredicateBuilder.New<InkindDonation>(d => d.UserId == userId);

                if (request.Status.HasValue)
                {
                    filter = filter.And(d => d.Status == request.Status.Value);
                }

                if (request.FromDate.HasValue)
                {
                    filter = filter.And(d => d.DonationDate >= request.FromDate.Value);
                }

                if (request.ToDate.HasValue)
                {
                    filter = filter.And(d => d.DonationDate <= request.ToDate.Value);
                }

                if (request.IsAnonymous.HasValue)
                {
                    filter = filter.And(d => d.IsAnonymous == request.IsAnonymous.Value);
                }

                var totalRecords = await _repository.GetCount(filter);
                var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);

                var donations = await _repository.GetListAsyncUntracked<InkindDonation>(
                    filter: filter,
                    include: q => q.Include(d => d.Event)
                                  .Include(d => d.Approver),
                    orderBy: q => q.OrderByDescending(d => d.DonationDate),
                    pageSize: request.PageSize,
                    pageNumber: request.PageIndex
                );

                var donationDtos = _converter.ToListDTO(donations);

                var pagingResponse = new BasePagingResponseDto<InkindDonationDto>
                {
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    PagedData = donationDtos.ToList()
                };

                _logger.LogInformation("Successfully retrieved {Count} inkind donations for user {UserId}",
                    donationDtos.Count(), userId);

                return new BaseResponseDto<BasePagingResponseDto<InkindDonationDto>>
                {
                    Status = 200,
                    Message = donationDtos.Any()
                        ? $"Lấy thành công {donationDtos.Count()} quyên góp (trang {request.PageIndex}/{totalPages})"
                        : "Không tìm thấy quyên góp nào",
                    ResponseData = pagingResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user inkind donations for user {UserId}", userId);
                return new BaseResponseDto<BasePagingResponseDto<InkindDonationDto>>
                {
                    Status = 500,
                    Message = $"Lỗi hệ thống: {ex.Message}",
                    ResponseData = null
                };
            }
        }
        #endregion

        #region GetEventInkindDonations - UPDATED
        public async Task<BaseResponseDto<BasePagingResponseDto<InkindDonationDetailDto>>> GetEventInkindDonationsAsync(
            Guid eventId, InkindDonationFilterRequestDto request)
        {
            try
            {
                _logger.LogInformation("Getting event inkind donations for event {EventId}", eventId);

                // Validate Event exists
                var eventEntity = await _eventRepository.GetOneAsyncUntracked<Event>(
                    filter: e => e.Id == eventId
                );

                if (eventEntity == null)
                {
                    return new BaseResponseDto<BasePagingResponseDto<InkindDonationDetailDto>>
                    {
                        Status = 404,
                        Message = "Không tìm thấy sự kiện",
                        ResponseData = null
                    };
                }

                var filter = PredicateBuilder.New<InkindDonation>(d => d.EventId == eventId);

                if (request.Status.HasValue)
                {
                    filter = filter.And(d => d.Status == request.Status.Value);
                }

                if (request.FromDate.HasValue)
                {
                    filter = filter.And(d => d.DonationDate >= request.FromDate.Value);
                }

                if (request.ToDate.HasValue)
                {
                    filter = filter.And(d => d.DonationDate <= request.ToDate.Value);
                }

                if (request.IsAnonymous.HasValue)
                {
                    filter = filter.And(d => d.IsAnonymous == request.IsAnonymous.Value);
                }

                var totalRecords = await _repository.GetCount(filter);
                var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);

                var donations = await _repository.GetListAsyncUntracked<InkindDonation>(
                    filter: filter,
                    include: q => q.Include(d => d.User)
                                  .Include(d => d.Event)
                                  .Include(d => d.Approver),
                    orderBy: q => q.OrderByDescending(d => d.DonationDate),
                    pageSize: request.PageSize,
                    pageNumber: request.PageIndex
                );

                var detailDtos = donations.Select(d => new InkindDonationDetailDto
                {
                    Id = d.Id,
                    UserId = d.UserId,
                    EventId = d.EventId,
                    InKind = d.InKind,
                    DonationDate = d.DonationDate,
                    Status = d.Status,
                    IsAnonymous = d.IsAnonymous,
                    Message = d.Message,
                    SenderAddress = d.SenderAddress,
                    ReceiverAddress = d.ReceiverAddress,
                    ApprovedBy = d.ApprovedBy,
                    ApprovedDate = d.ApprovedDate,
                    EstimatedDeliveryDate = d.EstimatedDeliveryDate,
                    UserName = d.IsAnonymous ? "Ẩn danh" : d.User?.FullName ?? "Không xác định",
                    UserEmail = d.IsAnonymous ? null : d.User?.Email,
                    EventTitle = d.Event?.Title ?? "Không xác định",
                    EventLocation = d.Event?.Location ?? "Không xác định",
                    ApproverName = d.Approver?.FullName,
                    StatusDescription = GetStatusDescription(d.Status),
                    DaysUntilDelivery = d.EstimatedDeliveryDate.HasValue
                        ? (d.EstimatedDeliveryDate.Value - DateTime.UtcNow).Days
                        : 0
                }).ToList();

                var pagingResponse = new BasePagingResponseDto<InkindDonationDetailDto>
                {
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    PagedData = detailDtos
                };

                _logger.LogInformation("Successfully retrieved {Count} inkind donations for event {EventId}",
                    detailDtos.Count, eventId);

                return new BaseResponseDto<BasePagingResponseDto<InkindDonationDetailDto>>
                {
                    Status = 200,
                    Message = detailDtos.Any()
                        ? $"Lấy thành công {detailDtos.Count} quyên góp (trang {request.PageIndex}/{totalPages})"
                        : "Không tìm thấy quyên góp nào",
                    ResponseData = pagingResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting event inkind donations for event {EventId}", eventId);
                return new BaseResponseDto<BasePagingResponseDto<InkindDonationDetailDto>>
                {
                    Status = 500,
                    Message = $"Lỗi hệ thống: {ex.Message}",
                    ResponseData = null
                };
            }
        }

        private string GetStatusDescription(InkindDonationStatus status)
        {
            return status switch
            {
                InkindDonationStatus.Pending => "Chờ duyệt",
                InkindDonationStatus.Approved => "Đã duyệt",
                InkindDonationStatus.Rejected => "Từ chối",
                InkindDonationStatus.InProgress => "Đang vận chuyển",
                InkindDonationStatus.Delivered => "Đã giao",
                InkindDonationStatus.Cancelled => "Đã hủy",
                _ => "Không xác định"
            };
        }
        #endregion

        #region CancelInkindDonation
        public async Task<BaseResponseDto<bool>> CancelInkindDonationAsync(Guid userId, Guid donationId)
        {
            try
            {
                var donation = await _repository.GetOneAsync(
                    filter: d => d.Id == donationId && d.UserId == userId
                );

                if (donation == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Không tìm thấy quyên góp",
                        ResponseData = false
                    };
                }

                if (donation.Status != InkindDonationStatus.Pending)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 400,
                        Message = "Chỉ có thể hủy quyên góp đang chờ duyệt",
                        ResponseData = false
                    };
                }

                donation.Status = InkindDonationStatus.Cancelled;
                await _repository.UpdateAsync(donation);

                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Hủy quyên góp thành công",
                    ResponseData = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling inkind donation {DonationId}", donationId);
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = $"Lỗi hệ thống: {ex.Message}",
                    ResponseData = false
                };
            }
        }
        #endregion

        #region ApproveInkindDonation
        public async Task<BaseResponseDto<bool>> ApproveInkindDonationAsync(
            Guid approverId, Guid donationId, string? notes = null)
        {
            try
            {
                var donation = await _repository.GetOneAsync(filter: d => d.Id == donationId);

                if (donation == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Không tìm thấy quyên góp",
                        ResponseData = false
                    };
                }

                if (donation.Status != InkindDonationStatus.Pending)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 400,
                        Message = "Chỉ có thể duyệt quyên góp đang chờ duyệt",
                        ResponseData = false
                    };
                }

                donation.Status = InkindDonationStatus.Approved;
                donation.ApprovedBy = approverId;
                donation.ApprovedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(donation);

                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Duyệt quyên góp thành công",
                    ResponseData = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving inkind donation {DonationId}", donationId);
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = $"Lỗi hệ thống: {ex.Message}",
                    ResponseData = false
                };
            }
        }
        #endregion

        #region RejectInkindDonation
        public async Task<BaseResponseDto<bool>> RejectInkindDonationAsync(
            Guid approverId, Guid donationId, string rejectionReason)
        {
            try
            {
                var donation = await _repository.GetOneAsync(filter: d => d.Id == donationId);

                if (donation == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Không tìm thấy quyên góp",
                        ResponseData = false
                    };
                }

                if (donation.Status != InkindDonationStatus.Pending)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 400,
                        Message = "Chỉ có thể từ chối quyên góp đang chờ duyệt",
                        ResponseData = false
                    };
                }

                donation.Status = InkindDonationStatus.Rejected;
                donation.ApprovedBy = approverId;
                donation.ApprovedDate = DateTime.UtcNow;

                await _repository.UpdateAsync(donation);

                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Từ chối quyên góp thành công",
                    ResponseData = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting inkind donation {DonationId}", donationId);
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = $"Lỗi hệ thống: {ex.Message}",
                    ResponseData = false
                };
            }
        }
        #endregion

        #region UpdateDeliveryStatus
        public async Task<BaseResponseDto<bool>> UpdateDeliveryStatusAsync(
            Guid donationId, InkindDonationStatus status, string? notes = null)
        {
            try
            {
                var donation = await _repository.GetOneAsync(filter: d => d.Id == donationId);

                if (donation == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Không tìm thấy quyên góp",
                        ResponseData = false
                    };
                }

                if (!IsValidStatusTransition(donation.Status, status))
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 400,
                        Message = "Không thể chuyển trạng thái này",
                        ResponseData = false
                    };
                }

                donation.Status = status;

                if (status == InkindDonationStatus.Delivered)
                {
                    donation.DeliveredDate = DateTime.UtcNow;
                }

                await _repository.UpdateAsync(donation);

                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Cập nhật trạng thái thành công",
                    ResponseData = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating delivery status for donation {DonationId}", donationId);
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = $"Lỗi hệ thống: {ex.Message}",
                    ResponseData = false
                };
            }
        }

        private bool IsValidStatusTransition(InkindDonationStatus currentStatus, InkindDonationStatus newStatus)
        {
            return currentStatus switch
            {
                InkindDonationStatus.Pending => newStatus is InkindDonationStatus.Approved or InkindDonationStatus.Rejected or InkindDonationStatus.Cancelled,
                InkindDonationStatus.Approved => newStatus is InkindDonationStatus.InProgress or InkindDonationStatus.Cancelled,
                InkindDonationStatus.InProgress => newStatus is InkindDonationStatus.Delivered or InkindDonationStatus.Cancelled,
                _ => false
            };
        }
        #endregion

        #region GetInkindDonationDetail
        public async Task<BaseResponseDto<InkindDonationDetailDto>> GetInkindDonationDetailAsync(Guid donationId)
        {
            try
            {
                var donation = await _repository.GetOneAsyncUntracked<InkindDonation>(
                    filter: d => d.Id == donationId,
                    include: q => q.Include(d => d.User)
                                  .Include(d => d.Event)
                                  .Include(d => d.Approver)
                );

                if (donation == null)
                {
                    return new BaseResponseDto<InkindDonationDetailDto>
                    {
                        Status = 404,
                        Message = "Không tìm thấy quyên góp",
                        ResponseData = null
                    };
                }

                var detailDto = new InkindDonationDetailDto
                {
                    Id = donation.Id,
                    UserId = donation.UserId,
                    EventId = donation.EventId,
                    InKind = donation.InKind,
                    DonationDate = donation.DonationDate,
                    Status = donation.Status,
                    IsAnonymous = donation.IsAnonymous,
                    Message = donation.Message,
                    SenderAddress = donation.SenderAddress,
                    ReceiverAddress = donation.ReceiverAddress,
                    ApprovedBy = donation.ApprovedBy,
                    ApprovedDate = donation.ApprovedDate,
                    EstimatedDeliveryDate = donation.EstimatedDeliveryDate,
                    UserName = donation.IsAnonymous ? "Ẩn danh" : donation.User?.FullName ?? "Không xác định",
                    UserEmail = donation.IsAnonymous ? null : donation.User?.Email,
                    EventTitle = donation.Event?.Title ?? "Không xác định",
                    EventLocation = donation.Event?.Location ?? "Không xác định",
                    ApproverName = donation.Approver?.FullName,
                    StatusDescription = GetStatusDescription(donation.Status),
                    DaysUntilDelivery = donation.EstimatedDeliveryDate.HasValue
                        ? (donation.EstimatedDeliveryDate.Value - DateTime.UtcNow).Days
                        : 0
                };

                return new BaseResponseDto<InkindDonationDetailDto>
                {
                    Status = 200,
                    Message = "Lấy chi tiết quyên góp thành công",
                    ResponseData = detailDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting inkind donation detail {DonationId}", donationId);
                return new BaseResponseDto<InkindDonationDetailDto>
                {
                    Status = 500,
                    Message = $"Lỗi hệ thống: {ex.Message}",
                    ResponseData = null
                };
            }
        }
        #endregion
    }
}
