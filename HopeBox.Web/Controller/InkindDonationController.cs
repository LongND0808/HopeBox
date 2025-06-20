using HopeBox.Core.IService;
using HopeBox.Domain.DTOs;
using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class InkindDonationController : BaseController<InkindDonation, InkindDonationDto>
    {
        protected readonly IInkindDonationService _inkindDonationService;

        public InkindDonationController(IBaseService<InkindDonation, InkindDonationDto> service, IInkindDonationService inkindDonationService) : base(service)
        {
            _inkindDonationService = inkindDonationService;
        }
    }
}
