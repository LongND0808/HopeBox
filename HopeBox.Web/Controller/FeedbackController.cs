using HopeBox.Core.IService;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : BaseController<Feedback, FeedbackDto>
    {
        public FeedbackController(IBaseService<Feedback, FeedbackDto> service) : base(service)
        {
        }
    }
}
