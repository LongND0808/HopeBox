using HopeBox.Core.IService;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using HopeBox.Domain.SePayModel;
using HopeBox.Domain.VietQRModel;
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

        public DonationController(IBaseService<Donation, DonationDto> service, IDonationService donationService) : base(service)
        {
            _donationService = donationService;
        }

        [Authorize]
        [HttpPost("create-donation-vn-pay")]
        public async Task<BaseResponseDto<string>> CreateDonationVNPay([FromBody] CreateDonationRequestDto donation)
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

            var result = await _donationService.CreateDonationVNPay(userId, donation);
            return result;
        }

        [Authorize]
        [HttpPost("create-donation-viet-qr")]
        public async Task<BaseResponseDto<VietQrApiResponse>> CreateDonationVietQR([FromBody] CreateDonationRequestDto donation)
        {
            var userId = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return new BaseResponseDto<VietQrApiResponse>
                {
                    Status = 401,
                    Message = "User ID not found in access token",
                    ResponseData = null
                };
            }

            var result = await _donationService.CreateDonationVietQR(userId, donation);
            return result;
        }

        [Authorize]
        [HttpPost("create-donation-sepay")]
        public async Task<BaseResponseDto<SePayQRResponse>> CreateDonationSePay([FromBody] CreateDonationRequestDto donation)
        {
            var userId = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return new BaseResponseDto<SePayQRResponse>
                {
                    Status = 401,
                    Message = "User ID not found in access token",
                    ResponseData = null
                };
            }

            var result = await _donationService.CreateDonationSePay(userId, donation);
            return result;
        }

        [HttpPost("vnpay-return")]
        public async Task<BaseResponseDto<bool>> VNPayReturn([FromBody] VNPayReturnRequestDto dto)
        {
            var result = await _donationService.HandleVNPayReturnAsync(dto);
            return result;
        }

        [Authorize]
        [HttpPost("track-email-transaction")]
        public async Task<BaseResponseDto<bool>> TrackEmailTransaction([FromQuery] string tradingCode)
        {
            if (string.IsNullOrEmpty(tradingCode))
            {
                return new BaseResponseDto<bool>
                {
                    Status = 400,
                    Message = "TradingCode is required.",
                    ResponseData = false
                };
            }

            var result = await _donationService.TrackEmailTransactionAsync(tradingCode);
            return result;
        }

        [Authorize]
        [HttpPost("track-sepay")]
        public async Task<BaseResponseDto<bool>> TrackSePayTransaction([FromQuery] string tradingCode, [FromQuery] decimal amount)
        {
            if (string.IsNullOrEmpty(tradingCode))
            {
                return new BaseResponseDto<bool>
                {
                    Status = 400,
                    Message = "TradingCode is required.",
                    ResponseData = false
                };
            }

            var result = await _donationService.TrackSePayTransactionAsync(tradingCode, amount);
            return result;
        }
    }
}