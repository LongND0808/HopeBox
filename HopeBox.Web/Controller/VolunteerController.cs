using HopeBox.Core.IService;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerController : BaseController<Volunteer, VolunteerDto>
    {
        public VolunteerController(IBaseService<Volunteer, VolunteerDto> service) : base(service)
        {
        }
    }
}
