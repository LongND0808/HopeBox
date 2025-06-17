using HopeBox.Core.Email;
using HopeBox.Core.IService;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Core.Service
{
    public class DonationService : BaseService<Donation, DonationDto>, IDonationService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<Cause> _causeRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<ReliefPackage> _reliefPackageRepository;
        private readonly IRepository<DonationReliefPackage> _donationReliefPackageRepository;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DonationService(
            IConfiguration configuration,
            IRepository<Donation> repository,
            IConverter<Donation, DonationDto> converter,
            IRepository<Cause> causeRepository,
            IRepository<User> userRepository,
            IRepository<ReliefPackage> reliefPackageRepository,
            IRepository<DonationReliefPackage> donationReliefPackageRepository,
            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor)
            : base(repository, converter)
        {
            _configuration = configuration;
            _causeRepository = causeRepository;
            _userRepository = userRepository;
            _reliefPackageRepository = reliefPackageRepository;
            _donationReliefPackageRepository = donationReliefPackageRepository;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponseDto<string>> CreateDonation(string userId,
            CreateDonationRequestDto donationDto)
        {
            try
            {
                var cause = await _causeRepository.GetOneAsyncUntracked(filter: f => f.Id == donationDto.CauseId, selector: s => new { s.Id, s.Status });
                if (cause == null)
                {
                    return new BaseResponseDto<string>
                    {
                        Status = 404,
                        Message = "Chiến dịch không tồn tại.",
                        ResponseData = null
                    };
                }
                if (cause.Status == CauseStatus.Completed)
                {
                    return new BaseResponseDto<string>
                    {
                        Status = 400,
                        Message = "Chiến dịch đã hoàn thành, không thể quyên góp.",
                        ResponseData = null
                    };
                }

                if (donationDto.ReliefPackages != null && donationDto.ReliefPackages.Any())
                {
                    var packageIds = donationDto.ReliefPackages.Keys.ToList();

                    var packages = await _reliefPackageRepository.GetListAsyncUntracked<ReliefPackage>(
                        filter: f => packageIds.Contains(f.Id) && f.CauseId == donationDto.CauseId);

                    if (packages.Count() != packageIds.Count)
                    {
                        return new BaseResponseDto<string>
                        {
                            Status = 400,
                            Message = "Một hoặc nhiều gói cứu trợ không tồn tại hoặc không thuộc chiến dịch này.",
                            ResponseData = null
                        };
                    }

                    foreach (var pkg in packages)
                    {
                        var requestedQuantity = donationDto.ReliefPackages[pkg.Id];
                        var availableQuantity = pkg.TargetQuantity - pkg.CurrentQuantity;
                        if (requestedQuantity <= 0 || requestedQuantity > availableQuantity)
                        {
                            return new BaseResponseDto<string>
                            {
                                Status = 400,
                                Message = $"Số lượng yêu cầu cho gói {pkg.Name} không hợp lệ" +
                                $" (còn lại {availableQuantity}).",
                                ResponseData = null
                            };
                        }
                    }
                }

                if (donationDto.DonationAmount < 0 || donationDto.Amount < donationDto.DonationAmount)
                {
                    return new BaseResponseDto<string>
                    {
                        Status = 400,
                        Message = "Số tiền quyên góp không hợp lệ.",
                        ResponseData = null
                    };
                }

                using var transaction = await _repository.BeginTransactionAsync();

                try
                {
                    var donation = new Donation
                    {
                        Id = Guid.NewGuid(),
                        Amount = donationDto.Amount,
                        DonationAmount = donationDto.DonationAmount,
                        CauseId = donationDto.CauseId,
                        UserId = Guid.Parse(userId),
                        DonationDate = DateTime.Now,
                        PaymentMethod = donationDto.PaymentMethod,
                        IsAnonymous = donationDto.IsAnonymous,
                        Message = donationDto.Message,
                        Status = DonationStatus.Pending,
                        TransactionId = null,
                        TradingCode = Guid.NewGuid().ToString("N")
                    };

                    await _repository.AddAsync(donation);

                    if (donationDto.ReliefPackages != null && donationDto.ReliefPackages.Any())
                    {
                        foreach (var (packageId, quantity) in donationDto.ReliefPackages)
                        {
                            var package = await _reliefPackageRepository.GetOneAsync(
                                filter: f => f.Id == packageId);

                            package.CurrentQuantity += quantity;
                            await _reliefPackageRepository.UpdateAsync(package);

                            var donationReliefPackage = new DonationReliefPackage
                            {
                                DonationId = donation.Id,
                                ReliefPackageId = packageId,
                                Quantity = quantity
                            };
                            await _donationReliefPackageRepository.AddAsync(donationReliefPackage);
                        }
                    }

                    string ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "127.0.0.1";
                    string paymentUrl = await CreateVNPayPaymentUrlAsync(donation.Id, ipAddress);

                    await transaction.CommitAsync();

                    return new BaseResponseDto<string>
                    {
                        Status = 200,
                        Message = "Tạo đơn quyên góp thành công.",
                        ResponseData = paymentUrl
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Lỗi khi tạo đơn quyên góp: {ex.Message}", ex);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<string>
                {
                    Status = 500,
                    Message = $"Lỗi server: {ex.Message}",
                    ResponseData = null
                };
            }
        }

        private async Task<string> CreateVNPayPaymentUrlAsync(Guid donationId, string ipAddress)
        {
            var donation = await _repository.GetOneAsyncUntracked<Donation>(filter: f => f.Id == donationId);
            if (donation == null)
            {
                return "";
            }

            string vnp_TmnCode = _configuration["VNPay:TmnCode"];
            string vnp_HashSecret = _configuration["VNPay:HashSecret"];
            string vnp_Url = _configuration["VNPay:Url"];
            string returnUrl = _configuration["VNPay:ReturnUrl"] + $"?donationId={donationId}";

            string vnp_TxnRef = donation.TradingCode ?? Guid.NewGuid().ToString("N");
            donation.TradingCode ??= vnp_TxnRef;

            string vnp_OrderInfo = $"Thanh toán ủng hộ";
            string vnp_Amount = ((int)(donation.Amount * 100)).ToString();

            var payParams = new SortedDictionary<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", vnp_TmnCode },
                { "vnp_Amount", vnp_Amount },
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef", vnp_TxnRef },
                { "vnp_OrderInfo", vnp_OrderInfo },
                { "vnp_OrderType", "donation" },
                { "vnp_Locale", "vn" },
                { "vnp_ReturnUrl", returnUrl },
                { "vnp_IpAddr", ipAddress ?? "127.0.0.1" },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss") }
            };

            string queryString = string.Join("&", payParams.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));
            string secureHash = HmacSHA512(vnp_HashSecret, queryString);
            payParams.Add("vnp_SecureHash", secureHash);

            string paymentUrl = vnp_Url + "?" + string.Join("&", payParams.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));

            return paymentUrl;
        }

        public async Task<BaseResponseDto<bool>> HandleVNPayReturnAsync(VNPayReturnRequestDto dto)
        {
            var donation = await _repository.GetOneAsync(filter: f => f.TradingCode == dto.vnp_TxnRef);
            if (donation == null)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 404,
                    Message = "Không tìm thấy đơn quyên góp.",
                    ResponseData = false
                };
            }

            if (dto.vnp_ResponseCode == "00")
            {
                using var transaction = await _repository.BeginTransactionAsync();
                try
                {
                    donation.TransactionId = dto.vnp_TransactionNo;
                    donation.Status = DonationStatus.Paid;
                    await _repository.UpdateAsync(donation);

                    var cause = await _causeRepository.GetOneAsync(filter: f => f.Id == donation.CauseId);
                    cause.CurrentAmount += donation.Amount;
                    if (cause.CurrentAmount >= cause.TargetAmount)
                    {
                        cause.Status = CauseStatus.Completed;
                    }
                    await _causeRepository.UpdateAsync(cause);

                    var user = await _userRepository.GetOneAsync(filter: f => f.Id == donation.UserId);
                    if (user == null)
                    {
                        return new BaseResponseDto<bool>
                        {
                            Status = 404,
                            Message = "Không tìm thấy người dùng.",
                            ResponseData = false
                        };
                    }

                    user.Point += (int)Math.Round(donation.Amount / 100000);
                    await _userRepository.UpdateAsync(user);

                    string subject = "Hóa đơn quyên góp - HopeBox";
                    string body = GenerateInvoiceHtml(donation, user.FullName, user.Email, cause.Title);
                    await _emailService.SendEmailAsync(user.Email, subject, body);

                    await transaction.CommitAsync();

                    return new BaseResponseDto<bool>
                    {
                        Status = 200,
                        Message = "Thanh toán thành công.",
                        ResponseData = true
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new BaseResponseDto<bool>
                    {
                        Status = 500,
                        Message = $"Lỗi khi xử lý thanh toán: {ex.Message}",
                        ResponseData = false
                    };
                }
            }

            return new BaseResponseDto<bool>
            {
                Status = 400,
                Message = "Thanh toán không thành công.",
                ResponseData = false
            };
        }

        private string GenerateInvoiceHtml(Donation donation, string userName, string userEmail, string causeTitle)
        {
            return $@"
                <h2>Cảm ơn bạn đã quyên góp cho HopeBox!</h2>
                <p><strong>Tên người dùng:</strong> {userName}</p>
                <p><strong>Email:</strong> {userEmail}</p>
                <p><strong>Số tiền:</strong> {donation.Amount:N0} VND</p>
                <p><strong>Ngày quyên góp:</strong> {donation.DonationDate:dd/MM/yyyy HH:mm}</p>
                <p><strong>Mã giao dịch:</strong> {donation.TradingCode}</p>
                <p><strong>ID giao dịch ngân hàng:</strong> {donation.TransactionId ?? "N/A"}</p>
                <p><strong>Chiến dịch:</strong> {causeTitle}</p>
                <br/>
                <p>Một lần nữa, chúng tôi xin chân thành cảm ơn bạn vì sự đóng góp quý báu này!</p>
                <p>HopeBox Team ❤️</p>
            ";
        }

        private static string HmacSHA512(string key, string input)
        {
            var hash = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(key));
            var bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}