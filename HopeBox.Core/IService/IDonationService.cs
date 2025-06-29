using HopeBox.Domain.Dtos;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using HopeBox.Domain.SePayModel;
using HopeBox.Domain.VietQRModel;
using System.Threading.Tasks;

namespace HopeBox.Core.IService
{
    public interface IDonationService
    {
        Task<BaseResponseDto<string>> CreateDonationVNPay(string userId, CreateDonationRequestDto donationDto);
        Task<BaseResponseDto<VietQrApiResponse>> CreateDonationVietQR(string userId, CreateDonationRequestDto donationDto);
        Task<BaseResponseDto<bool>> HandleVNPayReturnAsync(VNPayReturnRequestDto dto);
        Task<BaseResponseDto<bool>> TrackEmailTransactionAsync(string tradingCode);
        Task<BaseResponseDto<SePayQRResponse>> CreateDonationSePay(string userId, CreateDonationRequestDto donation);
        Task<BaseResponseDto<bool>> TrackSePayTransactionAsync(string tradingCode, decimal amount);
    }
}