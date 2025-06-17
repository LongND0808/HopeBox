using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Core.IService
{
    public interface ICauseService : IBaseService<Cause, CauseDto>
    {
        Task<BaseResponseDto<IEnumerable<CauseDto>>> GetCauseOneAsync();
        Task<BaseResponseDto<CauseDto>> GetCauseHighestTargetAsync();
        Task<BaseResponseDto<BasePagingResponseDto<CauseDto>>> GetCauseByFilter(CauseFilterRequestDto request);
        Task<BaseResponseDto<IEnumerable<CauseRevenueResponseDto>>> GetCauseRevenueAsync();
        Task<BaseResponseDto<BasePagingResponseDto<CauseWithVolunteerStatusDto>>> GetCauseByFilterWithUserStatus(
            CauseFilterWithUserRequestDto request);
        Task<BaseResponseDto<CauseDto>> GetMostUrgentCauseAsync();
    }
}
