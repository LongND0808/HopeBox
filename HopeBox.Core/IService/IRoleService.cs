using HopeBox.Domain.Dtos;
using HopeBox.Domain.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Core.IService
{
    public interface IRoleService
    {
        Task<BaseResponseDto<IEnumerable<RoleDto>>> GetAllRolesAsync();
    }
}
