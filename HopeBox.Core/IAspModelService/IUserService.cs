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

namespace HopeBox.Core.IAspModelService
{
    public interface IUserService
    {
        Task<BaseResponseDto<UserDto>> GetByIdAsync(Guid id);
        Task<BaseResponseDto<IEnumerable<UserDto>>> GetAllAsync();
        Task<BaseResponseDto<bool>> AddAsync(UserDto model);
        Task<BaseResponseDto<bool>> UpdateAsync(UserDto model);
        Task<BaseResponseDto<bool>> DeleteAsync(Guid id);
        Task<BaseResponseDto<int>> GetCountAsync();
        Task<BaseResponseDto<IEnumerable<UserDto>>> GetManagerAsync();
        Task<BaseResponseDto<string>> ChangeAvatarAsync(Guid guid, IFormFile file);
        Task<BaseResponseDto<bool>> UpdateUserInfoAsync(UpdateUserInfoRequestDto dto);
        Task<BaseResponseDto<IEnumerable<TopDonorResponseDto>>> GetTopDonorsAsync(int v);
    }
}
