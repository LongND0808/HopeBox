using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : BaseController<Organization, OrganizationDto>
    {
        public OrganizationController(IBaseService<Organization, OrganizationDto> service) : base(service)
        {
        }
    }
}
