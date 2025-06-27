using HopeBox.Domain.DTOs;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Core.IService
{
    public interface IInkindDonationService : IBaseService<InkindDonation, InkindDonationDto>
    {
        Task<BaseResponseDto<InkindDonationDto>> CreateInkindDonationAsync(Guid userId, CreateInkindDonationRequestDto request);
        Task<BaseResponseDto<BasePagingResponseDto<InkindDonationDto>>> GetUserInkindDonationsAsync(Guid userId, InkindDonationFilterRequestDto request);
        Task<BaseResponseDto<BasePagingResponseDto<InkindDonationDetailDto>>> GetEventInkindDonationsAsync(Guid eventId, InkindDonationFilterRequestDto request);
        Task<BaseResponseDto<bool>> CancelInkindDonationAsync(Guid userId, Guid donationId);
        Task<BaseResponseDto<bool>> ApproveInkindDonationAsync(Guid approverId, Guid donationId, string? notes = null);
        Task<BaseResponseDto<bool>> RejectInkindDonationAsync(Guid approverId, Guid donationId, string rejectionReason);
        Task<BaseResponseDto<bool>> UpdateDeliveryStatusAsync(Guid donationId, InkindDonationStatus status, string? notes = null);
        Task<BaseResponseDto<InkindDonationDetailDto>> GetInkindDonationDetailAsync(Guid donationId);
    }
}
