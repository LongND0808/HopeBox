using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationReliefPackageController : BaseController<DonationReliefPackage, DonationReliefPackageDto>
    {
        public DonationReliefPackageController(IBaseService<DonationReliefPackage, DonationReliefPackageDto> service) : base(service)
        {
        }
    }
}
