using HopeBox.Domain.Models;

namespace HopeBox.Core.Token
{
    public interface ITokenService
    {
        Task<string> CreateAccessTokenAsync(User user);
        string? ValidateTokenUser(string token);
        string? GetCurrentAccessToken();
        string? GetCurrentUserId();
        Task<string> CreateRefreshTokenAsync(User user);
        Task<string> RefreshAccessTokenAsync(string refreshToken);
    }
}
