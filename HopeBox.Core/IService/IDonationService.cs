using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using Microsoft.AspNetCore.Http;

namespace HopeBox.Core.IService
{
    public interface IDonationService : IBaseService<Donation, DonationDto>
    {
        Task<BaseResponseDto<string>> CreateVNPayPaymentUrlAsync(Guid donationId, string ipAddress);
        Task<BaseResponseDto<bool>> HandleVNPayReturnAsync(VNPayReturnRequestDto dto);
        Task<BaseResponseDto<DonationDto>> GetByTradingCodeAsync(string tradingCode);
        Task<BaseResponseDto<bool>> MarkAsPaidAsync(Guid donationId, string transactionId);
        Task<BaseResponseDto<bool>> UpdateTradingCodeAsync(Guid donationId, string tradingCode);
        Task<BaseResponseDto<string>> CreateDonation(DonationDto donation);
        Task<BaseResponseDto<EventDonationResponseDto>> CreateEventDonationAsync(
            Guid userId, EventDonationRequestDto request);
        Task<BaseResponseDto<bool>> HandleEventVNPayReturnAsync(VNPayReturnRequestDto dto);
        Task<BaseResponseDto<List<ReliefPackageDto>>> GetEventReliefPackagesAsync(Guid eventId);
    }
}
