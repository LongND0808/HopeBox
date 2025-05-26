using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReliefPackageItemController : BaseController<ReliefPackageItem, ReliefPackageItemDto>
    {
        public ReliefPackageItemController(IBaseService<ReliefPackageItem, ReliefPackageItemDto> service) : base(service)
        {
        }
    }
}
