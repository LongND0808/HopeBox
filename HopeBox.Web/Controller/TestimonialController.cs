using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestimonialController : BaseController<Testimonial, TestimonialDto>
    {
        public TestimonialController(IBaseService<Testimonial, TestimonialDto> service) : base(service)
        {
        }
    }
}
