using HopeBox.Core.IService;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReliefPackageController : BaseController<ReliefPackage, ReliefPackageDto>
    {
        public ReliefPackageController(IBaseService<ReliefPackage, ReliefPackageDto> service) : base(service)
        {
        }
    }
}
