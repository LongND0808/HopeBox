using HopeBox.Core.IService;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static HopeBox.Common.Enum.Enumerate;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace HopeBox.Core.Service
{
    public class CauseService : BaseService<Cause, CauseDto>, ICauseService
    {
        private readonly IRepository<Donation> _donationRepository;
        public CauseService(IRepository<Cause> repository, IConverter<Cause, CauseDto> converter, IRepository<Donation> donationRepository)
            : base(repository, converter)
        {
            _donationRepository = donationRepository;
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
    }
}
