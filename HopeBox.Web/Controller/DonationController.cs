using HopeBox.Core.IService;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : BaseController<Donation, DonationDto>
    {
        private readonly IDonationService _donationService;

        public DonationController(IDonationService service) : base(service)
        {
            _donationService = service;
        }

        [Authorize]
        [HttpPost("create-donation")]
        public async Task<BaseResponseDto<string>> CreateDonation([FromBody] CreateDonationRequestDto donation)
        {
            var userId = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return new BaseResponseDto<string>
                {
                    Status = 401,
                    Message = "User ID not found in access token",
                    ResponseData = null
                };
            }

            var result = await _donationService.CreateDonation(userId, donation);
            return result;
        }

        [HttpPost("vnpay-return")]
        public async Task<BaseResponseDto<bool>> VNPayReturn(VNPayReturnRequestDto dto)
        {
            var result = await _donationService.HandleVNPayReturnAsync(dto);
            return (result);
        }
    }
}
