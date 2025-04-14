using HopeBox.Domain.Models;

namespace HopeBox.Core.Token
{
    public interface ITokenService
    {
        Task<string> CreateAccessTokenAsync(User user);
        Task<string> CreateRefreshTokenAsync(User user);
        Task<string> RefreshAccessTokenAsync(string refreshToken);
    }
}
