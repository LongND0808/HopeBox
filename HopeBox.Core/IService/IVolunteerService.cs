using HopeBox.Domain.Dtos;
using HopeBox.Domain.DTOs;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;

namespace HopeBox.Core.IService
{
    public interface IVolunteerService : IBaseService<Volunteer, VolunteerDto>
    {
        Task<BaseResponseDto<VolunteerDto>> RegisterVolunteerAsync(VolunteerRequestDto request);
        Task<BaseResponseDto<bool>> ApproveVolunteerAsync(Guid volunteerId, Guid causeId);
        Task<BaseResponseDto<bool>> RejectVolunteerAsync(Guid volunteerId, Guid causeId);
        Task<BaseResponseDto<VolunteerDetailDto>> GetVolunteerDetailsAsync(Guid volunteerId);
    }
}