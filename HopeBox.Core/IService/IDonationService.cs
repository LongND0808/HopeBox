using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using Microsoft.AspNetCore.Http;

namespace HopeBox.Core.IService
{
    public interface IDonationService : IBaseService<Donation, DonationDto>
    {
        Task<BaseResponseDto<bool>> HandleVNPayReturnAsync(VNPayReturnRequestDto dto);
        Task<BaseResponseDto<string>> CreateDonation(string userId, CreateDonationRequestDto donation);
    }
}
