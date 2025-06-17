using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Core.IService
{
    public interface IReliefPackageItemService : IBaseService<ReliefPackageItem, ReliefPackageItemDto>
    {
        Task<BaseResponseDto<IEnumerable<ReliefPackageItemDto>>> GetByPackage(Guid packageId);
    }
}
