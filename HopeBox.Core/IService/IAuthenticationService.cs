using HopeBox.Domain.Dtos;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using System.Security.Claims;

namespace HopeBox.Core.IService
{
    public interface IAuthenticationService
    {
        Task<BaseResponseDto<LoginResponseDto>> AdminLogin(LoginRequestDto loginDto);
        Task<BaseResponseDto<bool>> ConfirmEmail(ConfirmEmailRequestDto request);
        Task<BaseResponseDto<LoginResponseDto>> Login(LoginRequestDto request);
        Task<BaseResponseDto<RefreshTokenResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task<BaseResponseDto<bool>> Register(RegisterRequestDto model);
        Task<BaseResponseDto<bool>> SendConfirmationCodeAsync(string email);
    }
}
