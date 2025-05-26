using HopeBox.Core.IService;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : BaseController<Media, MediaDto>
    {
        public MediaController(IBaseService<Media, MediaDto> service) : base(service)
        {
        }
    }
}
