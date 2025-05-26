using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : BaseController<Blog, BlogDto>
    {
        public BlogController(IBaseService<Blog, BlogDto> service) : base(service)
        {
        }
    }
}
