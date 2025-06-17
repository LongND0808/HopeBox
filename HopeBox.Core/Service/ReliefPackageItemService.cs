using HopeBox.Core.IService;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Core.Service
{
    public class ReliefPackageItemService : BaseService<ReliefPackageItem, ReliefPackageItemDto>, IReliefPackageItemService
    {
        public ReliefPackageItemService(IRepository<ReliefPackageItem> repository, IConverter<ReliefPackageItem, ReliefPackageItemDto> converter) : base(repository, converter)
        {
        }

        public async Task<BaseResponseDto<IEnumerable<ReliefPackageItemDto>>> GetByPackage(Guid packageId)
        {

            try
            {
                var entities = await _repository.GetListAsyncUntracked<ReliefPackageItem>();
                var dtos = _converter.ToListDTO(entities);

                return new BaseResponseDto<IEnumerable<ReliefPackageItemDto>>
                {
                    Status = 200,
                    Message = "Success",
                    ResponseData = dtos
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<IEnumerable<ReliefPackageItemDto>>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }
    }
}
