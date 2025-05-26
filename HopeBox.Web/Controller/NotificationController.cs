using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseController<Notification, NotificationDto>
    {
        public NotificationController(IBaseService<Notification, NotificationDto> service) : base(service)
        {
        }
    }
}
