using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;
using Microsoft.AspNetCore.Authorization;
using HopeBox.Domain.ResponseDto;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReliefPackageItemController : BaseController<ReliefPackageItem, ReliefPackageItemDto>
    {
        private readonly IReliefPackageItemService _reliefPackageItemService;
        public ReliefPackageItemController(IBaseService<ReliefPackageItem, ReliefPackageItemDto> service, IReliefPackageItemService reliefPackageItemService) : base(service)
        {
            _reliefPackageItemService = reliefPackageItemService;
        }

        [HttpGet("get-by-package")]
        public async Task<BaseResponseDto<IEnumerable<ReliefPackageItemDto>>> GetByPackage(Guid packageId)
        {
            var result = await _reliefPackageItemService.GetByPackage(packageId);
            return (result);
        }
    }
}
