using HopeBox.Core.IService;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Core.Service
{
    public class CauseService : BaseService<Cause, CauseDto>, ICauseService
    {
        private readonly IRepository<Donation> _donationRepository;
        private readonly IRepository<Volunteer> _volunteerRepository;
        public CauseService(
            IRepository<Cause> repository,
            IConverter<Cause, CauseDto> converter,
            IRepository<Donation> donationRepository,
            IRepository<Volunteer> volunteerRepository)
            : base(repository, converter)
        {
            _donationRepository = donationRepository;
            _volunteerRepository = volunteerRepository;
        }

        public async Task<BaseResponseDto<BasePagingResponseDto<CauseDto>>> GetCauseByFilter(
            CauseFilterRequestDto request)
        {
            try
            {
                Expression<Func<Cause, bool>> filter = PredicateBuilder.New<Cause>(true);

                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    filter = filter.And(f => EF.Functions.Like(f.Title, $"%{request.Name}%"));
                }

                if (request.CauseType.HasValue)
                {
                    filter = filter.And(f => (int)f.Type == request.CauseType.Value);
                }

                filter = filter.And(f => f.Status == CauseStatus.Ongoing);

                var totalRecords = await _repository.GetCount(filter);

                var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);

                var query = await _repository.GetListAsyncUntracked<Cause>(
                    filter: filter,
                    pageSize: request.PageSize,
                    pageNumber: request.PageIndex
                );

                var dtos = _converter.ToListDTO(query);

                var pagingResponse = new BasePagingResponseDto<CauseDto>
                {
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    PagedData = dtos
                };

                return new BaseResponseDto<BasePagingResponseDto<CauseDto>>
                {
                    Status = 200,
                    Message = "Lấy danh sách thành công",
                    ResponseData = pagingResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<BasePagingResponseDto<CauseDto>>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }


        public async Task<BaseResponseDto<CauseDto>> GetCauseHighestTargetAsync()
        {
            try
            {
                var entity = await base._repository.GetOneAsyncUntracked<Cause>(
                    filter: f => f.Status == CauseStatus.Ongoing,
                    orderBy: o => o.OrderByDescending(od => od.TargetAmount)
                );
                if (entity == null)
                {
                    return new BaseResponseDto<CauseDto>
                    {
                        Status = 404,
                        Message = "Entity not found.",
                        ResponseData = null
                    };
                }
                var dto = _converter.ToDTO(entity);
                return new BaseResponseDto<CauseDto> { Status = 200, Message = "Success", ResponseData = dto };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<CauseDto> { Status = 500, Message = ex.Message, ResponseData = null };
            }
        }

        public async Task<BaseResponseDto<IEnumerable<CauseDto>>> GetCauseOneAsync()
        {
            try
            {
                var entities = await base._repository.GetListAsyncUntracked<Cause>(
                    filter: f => f.Status == CauseStatus.Ongoing,
                    orderBy: o => o.OrderByDescending(od => od.TargetAmount),
                    pageSize: 3,
                    pageNumber: 1
                );
                var dtos = _converter.ToListDTO(entities);
                return new BaseResponseDto<IEnumerable<CauseDto>> { Status = 200, Message = "Success", ResponseData = dtos };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<IEnumerable<CauseDto>> { Status = 500, Message = ex.Message, ResponseData = null };
            }
        }

        public async Task<BaseResponseDto<IEnumerable<CauseRevenueResponseDto>>> GetCauseRevenueAsync()
        {
            try
            {
                var donations = await _donationRepository.GetListAsyncUntracked<Donation>(
                    filter: d => d.Status == DonationStatus.Paid
                );

                var grouped = donations
                    .GroupBy(d => new { d.DonationDate.Year, d.DonationDate.Month })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        Total = g.Sum(d => d.Amount)
                    })
                    .ToList();

                var allYears = grouped.Select(g => g.Year).Distinct().OrderBy(y => y);

                var result = new List<CauseRevenueResponseDto>();

                foreach (var year in allYears)
                {
                    var monthlyRevenue = new decimal[12];

                    for (int month = 1; month <= 12; month++)
                    {
                        var monthData = grouped.FirstOrDefault(g => g.Year == year && g.Month == month);
                        monthlyRevenue[month - 1] = monthData?.Total ?? 0;
                    }

                    result.Add(new CauseRevenueResponseDto
                    {
                        Year = year,
                        MonthlyRevenue = monthlyRevenue
                    });
                }

                return new BaseResponseDto<IEnumerable<CauseRevenueResponseDto>>
                {
                    Status = 200,
                    Message = "Thống kê doanh thu theo năm thành công",
                    ResponseData = result
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<IEnumerable<CauseRevenueResponseDto>>
                {
                    Status = 500,
                    Message = $"Lỗi: {ex.Message}",
                    ResponseData = null
                };
            }
        }

        public async Task<BaseResponseDto<BasePagingResponseDto<CauseWithVolunteerStatusDto>>> GetCauseByFilterWithUserStatus(
    CauseFilterWithUserRequestDto request)
        {
            try
            {
                Expression<Func<Cause, bool>> filter = PredicateBuilder.New<Cause>(true);

                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    filter = filter.And(f => EF.Functions.Like(f.Title, $"%{request.Name}%"));
                }

                if (request.CauseType.HasValue)
                {
                    filter = filter.And(f => (int)f.Type == request.CauseType.Value);
                }

                filter = filter.And(f => f.Status == CauseStatus.Ongoing);

                var totalRecords = await _repository.GetCount(filter);
                var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);

                var causes = await _repository.GetListAsyncUntracked<Cause>(
                    filter: filter,
                    pageSize: request.PageSize,
                    pageNumber: request.PageIndex
                );

                var result = new List<CauseWithVolunteerStatusDto>();

                foreach (var cause in causes)
                {
                    var causeDto = _converter.ToDTO(cause);
                    var causeWithStatus = new CauseWithVolunteerStatusDto
                    {
                        Id = causeDto.Id,
                        Title = causeDto.Title,
                        Description = causeDto.Description,
                        Detail = causeDto.Detail,
                        HeroImage = causeDto.HeroImage,
                        Challenge = causeDto.Challenge,
                        ChallengeImage = causeDto.ChallengeImage,
                        Summary = causeDto.Summary,
                        SummaryImage = causeDto.SummaryImage,
                        Type = causeDto.Type,
                        StartDate = causeDto.StartDate,
                        EndDate = causeDto.EndDate,
                        TargetAmount = causeDto.TargetAmount,
                        CurrentAmount = causeDto.CurrentAmount,
                        Status = causeDto.Status,
                        CreatedBy = causeDto.CreatedBy,
                        OrganizationId = causeDto.OrganizationId,
                        IsVolunteerRegistered = false,
                        VolunteerStatus = null,
                        VolunteerJoinDate = null
                    };

                    if (request.UserId.HasValue)
                    {
                        var volunteer = await _volunteerRepository.GetOneAsyncUntracked<Volunteer>(
                            filter: v => v.UserId == request.UserId.Value && v.CauseId == cause.Id
                        );

                        if (volunteer != null)
                        {
                            causeWithStatus.IsVolunteerRegistered = true;
                            causeWithStatus.VolunteerStatus = volunteer.Status;
                            causeWithStatus.VolunteerJoinDate = volunteer.JoinDate;
                        }
                    }

                    result.Add(causeWithStatus);
                }

                var pagingResponse = new BasePagingResponseDto<CauseWithVolunteerStatusDto>
                {
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    PagedData = result
                };

                return new BaseResponseDto<BasePagingResponseDto<CauseWithVolunteerStatusDto>>
                {
                    Status = 200,
                    Message = "Lấy danh sách thành công",
                    ResponseData = pagingResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<BasePagingResponseDto<CauseWithVolunteerStatusDto>>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }

        public async Task<BaseResponseDto<CauseDto>> GetMostUrgentCauseAsync()
        {
            try
            {
                var causes = await _repository.GetListAsyncUntracked<Cause>(
                    filter: c =>
                        c.Status == CauseStatus.Ongoing &&
                        c.CurrentAmount < c.TargetAmount &&
                        c.EndDate >= DateTime.UtcNow,
                    orderBy: null,
                    pageSize: null,
                    pageNumber: null
                );

                var mostUrgent = causes
                    .Select(c =>
                    {
                        var totalDays = (c.EndDate - c.StartDate).TotalDays;
                        var daysLeft = (c.EndDate - DateTime.UtcNow).TotalDays;

                        totalDays = totalDays <= 0 ? 1 : totalDays;
                        var target = c.TargetAmount == 0 ? 1 : c.TargetAmount;

                        var missingRate = 1 - ((double)c.CurrentAmount / (double)target);
                        var timeUrgency = 1 - (daysLeft / totalDays);

                        timeUrgency = Math.Max(0, Math.Min(1, timeUrgency));
                        missingRate = Math.Max(0, Math.Min(1, missingRate));

                        var urgencyScore = missingRate * 0.6 + timeUrgency * 0.4;

                        return new
                        {
                            Cause = c,
                            UrgencyScore = urgencyScore
                        };
                    })
                    .OrderByDescending(x => x.UrgencyScore)
                    .FirstOrDefault();

                if (mostUrgent == null)
                {
                    return new BaseResponseDto<CauseDto>
                    {
                        Status = 404,
                        Message = "Không tìm thấy chiến dịch phù hợp",
                        ResponseData = null
                    };
                }

                var dto = _converter.ToDTO(mostUrgent.Cause);

                return new BaseResponseDto<CauseDto>
                {
                    Status = 200,
                    Message = "Lấy chiến dịch cấp thiết nhất thành công",
                    ResponseData = dto
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<CauseDto>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }

    }
}
