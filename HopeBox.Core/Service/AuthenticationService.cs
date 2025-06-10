using HopeBox.Common.Constant;
using HopeBox.Core.Email;
using HopeBox.Core.IService;
using HopeBox.Core.Token;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Core.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<ConfirmEmail> _confirmEmailRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public AuthenticationService(IRepository<User> userRepository, IRepository<ConfirmEmail> confirmEmailRepository, ITokenService tokenService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _confirmEmailRepository = confirmEmailRepository;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<BaseResponseDto<bool>> ConfirmEmail(ConfirmEmailRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.ConfirmEmail) || string.IsNullOrWhiteSpace(request.ConfirmCode))
            {
                return new BaseResponseDto<bool>
                {
                    Status = 400,
                    Message = "Email and confirmation code cannot be empty.",
                    ResponseData = false
                };
            }

            var user = await _userRepository.GetOneAsync(u => u.Email == request.ConfirmEmail);
            if (user == null)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 404,
                    Message = "User not found.",
                    ResponseData = false
                };
            }

            var confirmEmail = await _confirmEmailRepository.GetOneAsyncUntracked<ConfirmEmail>(
                c => c.UserId == user.Id && c.ConfirmCode == request.ConfirmCode);

            if (confirmEmail == null || confirmEmail.ExpiresAt < DateTime.UtcNow)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 400,
                    Message = "Invalid or expired confirmation code.",
                    ResponseData = false
                };
            }

            confirmEmail.IsConfirmed = true;

            user.UserStatus = UserStatus.Active;

            await _userRepository.UpdateAsync(user);

            await _confirmEmailRepository.UpdateAsync(confirmEmail);

            await _confirmEmailRepository.SaveChangesAsync();

            return new BaseResponseDto<bool>
            {
                Status = 200,
                Message = "Email confirmed successfully.",
                ResponseData = true
            };
        }

        public async Task<BaseResponseDto<LoginResponseDto>> Login(LoginRequestDto request)
        {
            var user = await _userRepository.GetOneAsyncUntracked<User>(filter: f => f.Email == request.LoginEmail);
            if (user == null)
            {
                return new BaseResponseDto<LoginResponseDto>
                {
                    Status = 404,
                    Message = "Email does not exist, please try again.",
                    ResponseData = null
                };
            }
            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash ?? "", request.Password);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                return new BaseResponseDto<LoginResponseDto>
                {
                    Status = 401,
                    Message = "Password does not match, please try again.",
                    ResponseData = null
                };
            }

            if (user.UserStatus != UserStatus.Active)
            {
                return new BaseResponseDto<LoginResponseDto>
                {
                    Status = 400,
                    Message = "Please check your account status.",
                    ResponseData = null
                };
            }

            var accessToken = await _tokenService.CreateAccessTokenAsync(user);
            var refreshToken = await _tokenService.CreateRefreshTokenAsync(user);

            return new BaseResponseDto<LoginResponseDto>
            {
                Status = 200,
                Message = "Login successfully.",
                ResponseData = new LoginResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                }
            };
        }

        public async Task<BaseResponseDto<bool>> Register(RegisterRequestDto request)
        {
            using var transaction = await _userRepository.BeginTransactionAsync();
            try
            {
                var existingUser = await _userRepository.GetOneAsyncUntracked<User>(u => u.Email == request.Email);
                if (existingUser != null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 400,
                        Message = "Email is already taken.",
                        ResponseData = false
                    };
                }

                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = request.Email,
                    UserName = request.FullName,
                    FullName = request.FullName,
                    DateOfBirth = request.DateOfBirth,
                    Point = 0,
                    Gender = request.Gender,
                    AvatarUrl = "DefaultAvatar.jpg",
                    UserStatus = UserStatus.Pending,
                };

                var passwordHasher = new PasswordHasher<User>();
                newUser.PasswordHash = passwordHasher.HashPassword(newUser, request.Password);

                await _userRepository.AddAsync(newUser);
                await _userRepository.SaveChangesAsync();

                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Đăng ký thành công, hãy kích hoạt tài khoản của bạn.",
                    ResponseData = true
                };
            }
            catch (Exception ex)
            {
                await _userRepository.RollbackTransactionAsync(transaction);
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = "An error occurred during registration.",
                    ResponseData = false
                };
            }
        }

        public async Task<BaseResponseDto<bool>> SendConfirmationCodeAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new BaseResponseDto<bool>
                {
                    Status = 400,
                    Message = "Email cannot be empty.",
                    ResponseData = false
                };
            }

            var user = await _userRepository.GetOneAsync(u => u.Email == email);
            if (user == null)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 404,
                    Message = "User not found.",
                    ResponseData = false
                };
            }

            if (user.UserStatus == UserStatus.Active)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 400,
                    Message = "User already activated.",
                    ResponseData = false
                };
            }

            var oldCodes = await _confirmEmailRepository.GetListAsync(
                c => c.UserId == user.Id && !c.IsConfirmed && c.ExpiresAt > DateTime.UtcNow
            );

            foreach (var old in oldCodes)
            {
                old.IsConfirmed = true;
            }

            await _confirmEmailRepository.UpdateRangeAsync(oldCodes);

            string confirmCode = GenerateVerificationCode(8);

            var confirmEmail = new ConfirmEmail
            {
                UserId = user.Id,
                ConfirmCode = confirmCode,
                RequestedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsConfirmed = false
            };

            await _confirmEmailRepository.AddAsync(confirmEmail);
            await _confirmEmailRepository.SaveChangesAsync();

            string subject = "Xác thực tài khoản HopeBox";
            string body = $"Mã xác thực của bạn là: <strong>{confirmCode}</strong>";

            await _emailService.SendEmailAsync(user.Email, subject, body);

            return new BaseResponseDto<bool>
            {
                Status = 200,
                Message = "Confirmation code sent successfully.",
                ResponseData = true
            };
        }

        private string GenerateVerificationCode(int length)
        {
            var AllChars = (Constant.AllChars).ToCharArray();
            var result = new StringBuilder(length);
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[length];

                rng.GetBytes(bytes);

                for (int i = 0; i < length; i++)
                {
                    int index = bytes[i] % AllChars.Length;
                    result.Append(AllChars[index]);
                }
            }
            return result.ToString();
        }
    }

}
