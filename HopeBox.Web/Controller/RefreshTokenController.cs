using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController : BaseController<RefreshToken, RefreshTokenDto>
    {
        public RefreshTokenController(IBaseService<RefreshToken, RefreshTokenDto> service) : base(service)
        {
        }
    }
}
