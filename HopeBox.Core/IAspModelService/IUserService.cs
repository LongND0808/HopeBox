using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.ResponseDto;
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
    }
}
