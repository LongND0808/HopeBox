using Duende.IdentityModel;
using HopeBox.Domain.Models;
using HopeBox.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HopeBox.Core.Token
{
    public class TokenService : ITokenService
    {
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public TokenService(IRepository<RefreshToken> refreshTokenRepository, IConfiguration configuration, UserManager<User> userManager)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> CreateAccessTokenAsync(User user)
        {
            var secretKey = _configuration["JWT:Secret"];
            var issuer = _configuration["JWT:ValidIssuer"];
            var audience = _configuration["JWT:ValidAudience"];
            var tokenValidityInHours = int.TryParse(_configuration["JWT:TokenValidityInHours"], out int hours) ? hours : 8;

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(JwtClaimTypes.Id, user.Id.ToString()),
                new(JwtClaimTypes.Name, user.UserName),
                new(JwtClaimTypes.Email, user.Email),
                new(JwtClaimTypes.GivenName, user.FullName),
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(tokenValidityInHours),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(securityToken);

            return tokenString;
        }

        public async Task<string> CreateRefreshTokenAsync(User user)
        {
            var existingTokens = await _refreshTokenRepository.GetListAsync(x => x.UserId == user.Id);

            var lastToken = existingTokens.LastOrDefault();

            if (lastToken != null && lastToken.ExpiredTime > DateTime.Now)
            {
                return lastToken.Token;
            }

            var refreshToken = Guid.NewGuid().ToString();
            var refreshTokenValidity = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int validity) ? validity : 7;

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                CreateTime = DateTime.Now,
                ExpiredTime = DateTime.Now.AddDays(refreshTokenValidity),
            };

            await _refreshTokenRepository.AddAsync(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<string> RefreshAccessTokenAsync(string refreshToken)
        {
            var existingToken = await _refreshTokenRepository
                .GetOneAsync(x => x.Token == refreshToken);

            if (existingToken == null || existingToken.ExpiredTime <= DateTime.UtcNow)
            {
                return string.Empty;
            }

            var user = await _userManager.FindByIdAsync(existingToken.UserId.ToString());
            if (user == null)
            {
                return string.Empty;
            }

            return await CreateAccessTokenAsync(user);
        }

    }
}
