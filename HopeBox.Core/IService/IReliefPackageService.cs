using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Core.IService
{
    public interface IReliefPackageService : IBaseService<ReliefPackage, ReliefPackageDto>
    {
        Task<BaseResponseDto<bool>> CreateReliefPackageAsync(CreateReliefPackageRequestDto dto);
        Task<BaseResponseDto<IEnumerable<ReliefPackageDto>>> GetReliefPackagesByCauseIdAsync(Guid causeId);
        Task<BaseResponseDto<bool>> UpdateReliefPackageAsync(UpdateReliefPackageRequestDto dto);
        Task<BaseResponseDto<bool>> DeleteReliefPackageAsync(Guid packageId);
        Task<BaseResponseDto<string>> ChangeImageAsync(Guid guid, IFormFile file);
    }
}
