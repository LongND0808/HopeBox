using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfirmEmailController : BaseController<ConfirmEmail, ConfirmEmailDto>
    {
        public ConfirmEmailController(IBaseService<ConfirmEmail, ConfirmEmailDto> service) : base(service)
        {
        }
    }
}
