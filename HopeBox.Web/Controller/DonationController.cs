using HopeBox.Core.IService;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : BaseController<Donation, DonationDto>
    {
        public DonationController(IBaseService<Donation, DonationDto> service) : base(service)
        {
        }
    }
}
