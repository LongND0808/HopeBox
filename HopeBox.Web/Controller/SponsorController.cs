using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SponsorController : BaseController<Sponsor, SponsorDto>
    {
        public SponsorController(IBaseService<Sponsor, SponsorDto> service) : base(service)
        {
        }
    }
}
