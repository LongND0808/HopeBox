using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController<Role, RoleDto>
    {
        public RoleController(IBaseService<Role, RoleDto> service) : base(service)
        {
        }
    }
}
