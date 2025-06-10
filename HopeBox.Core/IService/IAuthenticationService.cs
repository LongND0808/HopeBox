using HopeBox.Domain.Dtos;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using System.Security.Claims;

namespace HopeBox.Core.IService
{
    public interface IAuthenticationService
    {
        Task<BaseResponseDto<bool>> ConfirmEmail(ConfirmEmailRequestDto request);
        Task<BaseResponseDto<LoginResponseDto>> Login(LoginRequestDto request);
        Task<BaseResponseDto<bool>> Register(RegisterRequestDto model);
        Task<BaseResponseDto<bool>> SendConfirmationCodeAsync(string email);
    }
}
