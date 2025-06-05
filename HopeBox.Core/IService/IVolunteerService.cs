using HopeBox.Domain.Dtos;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;

namespace HopeBox.Core.IService
{
    public interface IVolunteerService
    {
        Task<BaseResponseDto<VolunteerDto>> RegisterVolunteerAsync(VolunteerRequestDto request);
    }
}
