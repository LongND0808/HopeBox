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
        public async Task<BaseResponseDto<string>> CreateDonation([FromBody] DonationDto donation)
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

            donation.UserId = Guid.Parse(userId);

            var result = await _donationService.CreateDonation(donation);
            return result;
        }


        [HttpGet("create-payment-url")]
        public async Task<BaseResponseDto<string>> CreatePaymentUrl([FromQuery] Guid donationId)
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
            var result = await _donationService.CreateVNPayPaymentUrlAsync(donationId, ipAddress);
            return (result);
        }

        [HttpPost("vnpay-return")]
        public async Task<BaseResponseDto<bool>> VNPayReturn(VNPayReturnRequestDto dto)
        {
            var result = await _donationService.HandleVNPayReturnAsync(dto);
            return (result);
        }

        [HttpGet("get-by-trading-code")]
        public async Task<BaseResponseDto<DonationDto>> GetByTradingCode([FromQuery] string tradingCode)
        {
            var result = await _donationService.GetByTradingCodeAsync(tradingCode);
            return (result);
        }

        [HttpPost("mark-paid")]
        public async Task<BaseResponseDto<bool>> MarkAsPaid([FromBody] MarkPaidRequestDto request)
        {
            var result = await _donationService.MarkAsPaidAsync(request.DonationId, request.TransactionId);
            return (result);
        }

        [HttpPost("update-trading-code")]
        public async Task<BaseResponseDto<bool>> UpdateTradingCode([FromBody] UpdateTradingCodeRequestDto request)
        {
            var result = await _donationService.UpdateTradingCodeAsync(request.DonationId, request.TradingCode);
            return (result);
        }
    }
}
