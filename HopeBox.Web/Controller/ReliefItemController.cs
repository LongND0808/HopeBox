using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReliefItemController : BaseController<ReliefItem, ReliefItemDto>
    {
        public ReliefItemController(IBaseService<ReliefItem, ReliefItemDto> service) : base(service)
        {
        }
    }
}
