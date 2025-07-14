using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using static HopeBox.Common.Enum.Enumerate;
using System.Text.Json;
using HopeBox.Domain.VietQRModel;
using HopeBox.Domain.SePayModel;

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
        private readonly HttpClient _httpClient;

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
            _httpClient = new HttpClient();
        }

        private async Task<(BaseResponseDto<string>, Donation)> ValidateAndCreateDonationAsync(string userId, CreateDonationRequestDto donationDto)
        {
            try
            {
                var cause = await _causeRepository.GetOneAsyncUntracked(filter: f => f.Id == donationDto.CauseId, selector: s => new { s.Id, s.Status });
                if (cause == null)
                {
                    return (new BaseResponseDto<string>
                    {
                        Status = 404,
                        Message = "Chiến dịch không tồn tại.",
                        ResponseData = null
                    }, null);
                }
                if (cause.Status == CauseStatus.Completed)
                {
                    return (new BaseResponseDto<string>
                    {
                        Status = 400,
                        Message = "Chiến dịch đã hoàn thành, không thể quyên góp.",
                        ResponseData = null
                    }, null);
                }

                if (donationDto.ReliefPackages != null && donationDto.ReliefPackages.Any())
                {
                    var packageIds = donationDto.ReliefPackages.Keys.ToList();
                    var packages = await _reliefPackageRepository.GetListAsyncUntracked<ReliefPackage>(
                        filter: f => packageIds.Contains(f.Id) && f.CauseId == donationDto.CauseId);

                    if (packages.Count() != packageIds.Count)
                    {
                        return (new BaseResponseDto<string>
                        {
                            Status = 400,
                            Message = "Một hoặc nhiều gói cứu trợ không tồn tại hoặc không thuộc chiến dịch này.",
                            ResponseData = null
                        }, null);
                    }

                    foreach (var pkg in packages)
                    {
                        var requestedQuantity = donationDto.ReliefPackages[pkg.Id];
                        var availableQuantity = pkg.TargetQuantity - pkg.CurrentQuantity;
                        if (requestedQuantity <= 0 || requestedQuantity > availableQuantity)
                        {
                            return (new BaseResponseDto<string>
                            {
                                Status = 400,
                                Message = $"Số lượng yêu cầu cho gói {pkg.Name} không hợp lệ (còn lại {availableQuantity}).",
                                ResponseData = null
                            }, null);
                        }
                    }
                }

                if (donationDto.DonationAmount < 0 || donationDto.Amount < donationDto.DonationAmount)
                {
                    return (new BaseResponseDto<string>
                    {
                        Status = 400,
                        Message = "Số tiền quyên góp không hợp lệ.",
                        ResponseData = null
                    }, null);
                }

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
                    TradingCode = Guid.NewGuid().ToString("N").Substring(0, 12)
                };

                return (new BaseResponseDto<string>
                {
                    Status = 200,
                    Message = "Donation object created successfully.",
                    ResponseData = donation.Id.ToString()
                }, donation);
            }
            catch (Exception ex)
            {
                return (new BaseResponseDto<string>
                {
                    Status = 500,
                    Message = $"Lỗi server: {ex.Message}",
                    ResponseData = null
                }, null);
            }
        }

        public async Task<BaseResponseDto<string>> CreateDonationVNPay(string userId, CreateDonationRequestDto donationDto)
        {
            if (donationDto.PaymentMethod != PaymentMethod.VNPay)
            {
                return new BaseResponseDto<string>
                {
                    Status = 400,
                    Message = "Phương thức thanh toán không phải VNPay.",
                    ResponseData = null
                };
            }

            var (validationResult, donation) = await ValidateAndCreateDonationAsync(userId, donationDto);
            if (validationResult.Status != 200)
            {
                return validationResult;
            }

            using var transaction = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.AddAsync(donation);

                if (donationDto.ReliefPackages != null && donationDto.ReliefPackages.Any())
                {
                    foreach (var (packageId, quantity) in donationDto.ReliefPackages)
                    {
                        var package = await _reliefPackageRepository.GetOneAsync(filter: f => f.Id == packageId);
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
                string responseData = await CreateVNPayPaymentUrlAsync(donation.Id, ipAddress);

                if (string.IsNullOrEmpty(responseData))
                {
                    await transaction.RollbackAsync();
                    return new BaseResponseDto<string>
                    {
                        Status = 500,
                        Message = "Không thể tạo URL thanh toán VNPay.",
                        ResponseData = null
                    };
                }

                await transaction.CommitAsync();

                return new BaseResponseDto<string>
                {
                    Status = 200,
                    Message = "Tạo đơn quyên góp thành công, chuyển hướng đến VNPay.",
                    ResponseData = responseData
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new BaseResponseDto<string>
                {
                    Status = 500,
                    Message = $"Lỗi khi tạo đơn quyên góp: {ex.Message}",
                    ResponseData = null
                };
            }
        }

        public async Task<BaseResponseDto<VietQrApiResponse>> CreateDonationVietQR(string userId, CreateDonationRequestDto donationDto)
        {
            if (donationDto.PaymentMethod != PaymentMethod.VietQR)
            {
                return new BaseResponseDto<VietQrApiResponse>
                {
                    Status = 400,
                    Message = "Phương thức thanh toán không phải VietQR.",
                    ResponseData = null
                };
            }

            var (validationResult, donation) = await ValidateAndCreateDonationAsync(userId, donationDto);
            if (validationResult.Status != 200)
            {
                return new BaseResponseDto<VietQrApiResponse>
                {
                    Status = validationResult.Status,
                    Message = validationResult.Message,
                    ResponseData = null
                };
            }

            using var transaction = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.AddAsync(donation);

                if (donationDto.ReliefPackages != null && donationDto.ReliefPackages.Any())
                {
                    foreach (var (packageId, quantity) in donationDto.ReliefPackages)
                    {
                        var package = await _reliefPackageRepository.GetOneAsync(filter: f => f.Id == packageId);
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

                var vietQrApiResponse = await CreateVietQRCodeAsync(donation);

                await transaction.CommitAsync();

                return new BaseResponseDto<VietQrApiResponse>
                {
                    Status = 200,
                    Message = "Tạo đơn quyên góp thành công, hiển thị mã VietQR.",
                    ResponseData = vietQrApiResponse
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new BaseResponseDto<VietQrApiResponse>
                {
                    Status = 500,
                    Message = $"Lỗi khi tạo đơn quyên góp: {ex.Message}",
                    ResponseData = null
                };
            }
        }

        public async Task<BaseResponseDto<SePayQRResponse>> CreateDonationSePay(string userId, CreateDonationRequestDto donationDto)
        {
            if (donationDto.PaymentMethod != PaymentMethod.SePay)
            {
                return new BaseResponseDto<SePayQRResponse>
                {
                    Status = 400,
                    Message = "Phương thức thanh toán không phải SePay.",
                    ResponseData = null
                };
            }

            var (validationResult, donation) = await ValidateAndCreateDonationAsync(userId, donationDto);
            if (validationResult.Status != 200)
            {
                return new BaseResponseDto<SePayQRResponse>
                {
                    Status = validationResult.Status,
                    Message = validationResult.Message,
                    ResponseData = null
                };
            }

            using var transaction = await _repository.BeginTransactionAsync();
            try
            {
                await _repository.AddAsync(donation);

                if (donationDto.ReliefPackages != null && donationDto.ReliefPackages.Any())
                {
                    foreach (var (packageId, quantity) in donationDto.ReliefPackages)
                    {
                        var package = await _reliefPackageRepository.GetOneAsync(filter: f => f.Id == packageId);
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

                var sePayQrResponse = await CreateSePayQRCodeAsync(donation);

                await transaction.CommitAsync();

                return new BaseResponseDto<SePayQRResponse>
                {
                    Status = 200,
                    Message = "Tạo đơn quyên góp thành công, hiển thị mã SePay QR.",
                    ResponseData = sePayQrResponse
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new BaseResponseDto<SePayQRResponse>
                {
                    Status = 500,
                    Message = $"Lỗi khi tạo đơn quyên góp: {ex.Message}",
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

            string vnp_TxnRef = donation.TradingCode ?? Guid.NewGuid().ToString("N").Substring(0, 12);
            donation.TradingCode ??= vnp_TxnRef;

            string vnp_OrderInfo = $"Thanh toán ủng hộ";
            string vnp_Amount = ((double)(donation.Amount * 100)).ToString();

            var vnTz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vnNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTz);

            string createDate = vnNow.ToString("yyyyMMddHHmmss");
            string expireDate = vnNow.AddMinutes(15).ToString("yyyyMMddHHmmss");

            var payParams = new SortedDictionary<string, string>
            {
                { "vnp_Version",  "2.1.0" },
                { "vnp_Command",  "pay" },
                { "vnp_TmnCode",  vnp_TmnCode },
                { "vnp_Amount",   vnp_Amount },
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef",   vnp_TxnRef },
                { "vnp_OrderInfo","Quyên góp HopeBox" },
                { "vnp_OrderType","donation" },
                { "vnp_Locale",   "vn" },
                { "vnp_ReturnUrl",returnUrl },
                { "vnp_IpAddr",   ipAddress ?? "127.0.0.1" },
                { "vnp_CreateDate", createDate },
                { "vnp_ExpireDate", expireDate }
            };

            string query = string.Join("&", payParams.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));
            string secureHash = HmacSHA512(vnp_HashSecret, query);
            payParams.Add("vnp_SecureHash", secureHash);

            string paymentUrl = vnp_Url + "?" +
                string.Join("&", payParams.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));

            return paymentUrl;
        }

        private async Task<VietQrApiResponse> CreateVietQRCodeAsync(Donation donation)
        {
            var vietQrUrl = "https://api.vietqr.io/v2/generate";
            var clientId = _configuration["VietQR:ClientId"];
            var apiKey = _configuration["VietQR:ApiKey"];

            var requestBody = new
            {
                accountNo = "4271000599",
                accountName = "NGUYEN DANG LONG",
                acqId = 970418,
                amount = (int)donation.Amount,
                addInfo = donation.TradingCode.Length > 25 ? donation.TradingCode.Substring(0, 25) : donation.TradingCode,
                format = "text",
                template = "print"
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, vietQrUrl);
            request.Headers.Add("x-client-id", clientId);
            request.Headers.Add("x-api-key", apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var vietQrData = JsonSerializer.Deserialize<VietQrApiResponse>(responseContent);

            if (vietQrData?.code != "00" || vietQrData?.data?.qrDataURL == null)
            {
                throw new Exception($"Không thể tạo mã VietQR: {vietQrData?.desc ?? "Lỗi không xác định"}");
            }

            return vietQrData;
        }

        private async Task<SePayQRResponse> CreateSePayQRCodeAsync(Donation donation)
        {
            var sepayQrUrl = "https://qr.sepay.vn/img";
            var accountNumber = "4271000599";
            var bankCode = "BIDV";
            var amount = ((int)donation.Amount).ToString();
            var description = donation.TradingCode.Length > 25 ? donation.TradingCode.Substring(0, 25) : donation.TradingCode;

            var queryParams = $"?bank={bankCode}&acc={accountNumber}&template=compact&amount={amount}&des={Uri.EscapeDataString(description)}";
            var qrUrl = $"{sepayQrUrl}{queryParams}";

            return new SePayQRResponse
            {
                ImageUrl = qrUrl,
                BankName = "Ngân hàng BIDV",
                AccountName = "NGUYEN DANG LONG",
                TradingCode = donation.TradingCode
            };
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
                        await SendThankYouEmailsAndCertificatesAsync(cause);
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

                    var donationReliefPackages = await _donationReliefPackageRepository.GetListAsync(
                        filter: f => f.DonationId == donation.Id);
                    string subject = "Hóa đơn quyên góp - HopeBox";
                    string body = GenerateInvoiceHtml(donation, user.FullName, user.Email, cause.Title, donationReliefPackages.ToList());
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

        public async Task<BaseResponseDto<bool>> TrackEmailTransactionAsync(string tradingCode)
        {
            try
            {
                var donation = await _repository.GetOneAsync(filter: f => f.TradingCode == tradingCode && f.Status == DonationStatus.Pending);
                if (donation == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Không tìm thấy đơn quyên góp hoặc đơn đã được xử lý.",
                        ResponseData = false
                    };
                }

                var emails = await _emailService.GetEmailsFromSenderAsync("bidvsmartbanking@bidv.com.vn");
                bool transactionFound = false;

                foreach (var email in emails)
                {
                    if (email.Subject.Contains("Thông báo giao dịch thành công") && email.ReceivedDate >= donation.DonationDate)
                    {
                        var match = Regex.Match(email.Body, @"Nội dung giao dịch:\s*([^\n]+)");
                        if (match.Success && match.Groups[1].Value.Trim().Contains(donation.TradingCode))
                        {
                            var amountMatch = Regex.Match(email.Body, @"Số tiền thực tế người thụ hưởng nhận được:\s*([\d,]+)\s*VND");
                            if (amountMatch.Success)
                            {
                                var emailAmount = decimal.Parse(amountMatch.Groups[1].Value.Replace(",", ""));
                                if (emailAmount == donation.Amount)
                                {
                                    transactionFound = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!transactionFound)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 400,
                        Message = "Không tìm thấy giao dịch khớp với mã giao dịch.",
                        ResponseData = false
                    };
                }

                using var transaction = await _repository.BeginTransactionAsync();
                try
                {
                    donation.Status = DonationStatus.Paid;
                    await _repository.UpdateAsync(donation);

                    var cause = await _causeRepository.GetOneAsync(filter: f => f.Id == donation.CauseId);
                    cause.CurrentAmount += donation.Amount;
                    if (cause.CurrentAmount >= cause.TargetAmount)
                    {
                        cause.Status = CauseStatus.Completed;
                        await SendThankYouEmailsAndCertificatesAsync(cause);
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

                    var donationReliefPackages = await _donationReliefPackageRepository.GetListAsync(
                        filter: f => f.DonationId == donation.Id);
                    string subject = "Hóa đơn quyên góp - HopeBox";
                    string body = GenerateInvoiceHtml(donation, user.FullName, user.Email, cause.Title, donationReliefPackages.ToList());
                    await _emailService.SendEmailAsync(user.Email, subject, body);

                    await transaction.CommitAsync();

                    return new BaseResponseDto<bool>
                    {
                        Status = 200,
                        Message = "Xác nhận giao dịch VietQR thành công.",
                        ResponseData = true
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new BaseResponseDto<bool>
                    {
                        Status = 500,
                        Message = $"Lỗi khi xử lý giao dịch: {ex.Message}",
                        ResponseData = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = $"Lỗi server: {ex.Message}",
                    ResponseData = false
                };
            }
        }

        public async Task<BaseResponseDto<bool>> TrackSePayTransactionAsync(string tradingCode, decimal amount)
        {
            try
            {
                var donation = await _repository.GetOneAsync(filter: f => f.TradingCode == tradingCode && f.Status == DonationStatus.Pending);
                if (donation == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Không tìm thấy đơn quyên góp hoặc đơn đã được xử lý.",
                        ResponseData = false
                    };
                }

                var sepayApiUrl = "https://my.sepay.vn/userapi/transactions/list";
                var apiToken = _configuration["SePay:ApiToken"] ?? "7SV05AWLIQHNIIODHRMKFX0R6X4NHABY8DDZBW4TEUJQLYZQGJL5GPKKVMNOMHCM";
                var accountNumber = "4271000599";
                var amountIn = amount.ToString("F2"); // Format as string with 2 decimal places

                var queryParams = $"?account_number={accountNumber}&amount_in={amountIn}";

                using var request = new HttpRequestMessage(HttpMethod.Get, sepayApiUrl + queryParams);
                request.Headers.Add("Authorization", $"Bearer {apiToken}");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var sepayResponse = JsonSerializer.Deserialize<SePayApiResponse>(responseContent);

                if (sepayResponse?.status != 200 || sepayResponse.transactions == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 500,
                        Message = $"Không thể truy vấn giao dịch từ SePay: {sepayResponse?.error ?? "Lỗi không xác định"}",
                        ResponseData = false
                    };
                }

                // Check for transaction with matching TradingCode in transaction_content
                var matchingTransaction = sepayResponse.transactions
                    .FirstOrDefault(t => t.transaction_content?.Contains(tradingCode, StringComparison.OrdinalIgnoreCase) == true);

                if (matchingTransaction == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 400,
                        Message = "Không tìm thấy giao dịch khớp với mã giao dịch.",
                        ResponseData = false
                    };
                }

                // Verify amount_in matches donation amount
                if (decimal.Parse(matchingTransaction.amount_in) != amount)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 400,
                        Message = "Số tiền giao dịch không khớp.",
                        ResponseData = false
                    };
                }

                using var transaction = await _repository.BeginTransactionAsync();
                try
                {
                    donation.Status = DonationStatus.Paid;
                    await _repository.UpdateAsync(donation);

                    var cause = await _causeRepository.GetOneAsync(filter: f => f.Id == donation.CauseId);
                    cause.CurrentAmount += donation.Amount;
                    if (cause.CurrentAmount >= cause.TargetAmount)
                    {
                        cause.Status = CauseStatus.Completed;
                        await SendThankYouEmailsAndCertificatesAsync(cause);
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

                    var donationReliefPackages = await _donationReliefPackageRepository.GetListAsync(
                        filter: f => f.DonationId == donation.Id);
                    string subject = "Hóa đơn quyên góp - HopeBox";
                    string body = GenerateInvoiceHtml(donation, user.FullName, user.Email, cause.Title, donationReliefPackages.ToList());
                    await _emailService.SendEmailAsync(user.Email, subject, body);

                    await transaction.CommitAsync();

                    return new BaseResponseDto<bool>
                    {
                        Status = 200,
                        Message = "Xác nhận giao dịch SePay thành công.",
                        ResponseData = true
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new BaseResponseDto<bool>
                    {
                        Status = 500,
                        Message = $"Lỗi khi xử lý giao dịch: {ex.Message}",
                        ResponseData = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = $"Lỗi server: {ex.Message}",
                    ResponseData = false
                };
            }
        }

        private async Task SendThankYouEmailsAndCertificatesAsync(Cause cause)
        {
            var donations = await _repository.GetListAsyncUntracked<Donation>(filter: f => f.CauseId == cause.Id && f.Status == DonationStatus.Paid);
            var users = await _userRepository.GetListAsyncUntracked(selector: u => new { u.Id, u.FullName, u.Email });

            foreach (var donation in donations)
            {
                var user = users.FirstOrDefault(u => u.Id == donation.UserId);
                if (user == null) continue;

                bool isEligibleForCertificate = donation.Amount >= 10000000 || donation.Amount >= (cause.TargetAmount * (decimal)0.1);

                if (isEligibleForCertificate)
                {
                    string certificateSubject = "Chứng nhận đóng góp - HopeBox";
                    string certificateHtml = GenerateCertificateHtml(donation, user.FullName, cause.Title);
                    await _emailService.SendEmailAsync(user.Email, certificateSubject, certificateHtml);
                }
                else
                {
                    string thankYouSubject = "Cảm ơn bạn đã ủng hộ chiến dịch - HopeBox";
                    string thankYouBody = $@"
                        <p>Xin chào {user.FullName},</p>
                        <p>Cảm ơn bạn đã đóng góp cho chiến dịch <strong>{cause.Title}</strong>. Sự hỗ trợ của bạn đã giúp chúng tôi tiến gần hơn đến mục tiêu. Hy vọng sẽ tiếp tục nhận được sự đồng hành từ bạn!</p>
                        <p>Trân trọng,<br>Đội ngũ HopeBox</p>";
                    await _emailService.SendEmailAsync(user.Email, thankYouSubject, thankYouBody);
                }
            }
        }

        private string GenerateCertificateHtml(Donation donation, string userName, string causeTitle)
        {
            return $@"
                <!DOCTYPE html>
                <html lang=""vi"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Chứng nhận đóng góp – HopeBox</title>
                    <link href=""https://fonts.googleapis.com/css2?family=Be+Vietnam+Pro&display=swap"" rel=""stylesheet"">
                </head>
                <body style=""margin: 0; padding: 0; font-family: 'Be Vietnam Pro', sans-serif; background-color: #ffffff;"">
                    <div style=""position: relative; width: 1123px; height: 794px; margin: 0 auto; background-image: url('https://pub-dc597dd9f97242ceb1fc0179075fabfa.r2.dev/certificate.png'); background-size: cover; background-repeat: no-repeat; padding: 60px 80px; box-sizing: border-box; color: #111111; text-align: center;"">
                        <div style=""text-align: right; font-size: 16px; line-height: 1.5;"">
                            <p style=""margin: 4px 0;""><strong>HOPEBOX</strong></p>
                            <p style=""margin: 4px 0;"">Đại học FPT - Hòa Lạc</p>
                            <p style=""margin: 4px 0;"">0868 390 784 | HopeBoxHola@gmail.com</p>
                        </div>
                        <div style=""text-align: center; font-size: 36px; color: #d4af37; font-weight: bold; text-transform: uppercase; margin-top: 20px;"">CHỨNG NHẬN ĐÓNG GÓP</div>
                        <div style=""text-align: center; margin-top: 30px; font-size: 18px; line-height: 1.8;"">
                            <p style=""margin: 4px 0;"">Chúng tôi xin chân thành cảm ơn</p>
                            <p style=""font-size: 26px; font-weight: bold; text-decoration: underline; margin: 10px 0;"">{userName}</p>
                            <p style=""margin: 4px 0;"">Vì sự đóng góp quý báu, góp phần mang lại cơ hội phát triển cho những mảnh đời khó khăn.</p>
                            <p style=""margin: 4px 0;"">Sự hỗ trợ của quý vị là động lực để dự án tiếp tục lan tỏa và phát triển.</p>
                            <p style=""margin: 4px 0;"">Chiến dịch: <strong>{causeTitle}</strong></p>
                            <p style=""margin: 4px 0;"">Số tiền ủng hộ: <strong>{donation.Amount:N0} VND</strong></p>
                            <p style=""margin: 4px 0;"">Ngày trao: <strong>{DateTime.Now:dd/MM/yyyy}</strong></p>
                        </div>
                        <div style=""text-align: right; font-size: 16px; margin-top: 40px;"">
                            <p style=""margin: 4px 0;"">Trân trọng,</p>
                            <p style=""margin: 4px 0;"">Đội ngũ HopeBox</p>
                        </div>
                    </div>
                </body>
                </html>";
        }

        private string GenerateInvoiceHtml(Donation donation, string userName, string userEmail, string causeTitle, List<DonationReliefPackage> donationReliefPackages)
        {
            var reliefPackageRows = "";
            if (donationReliefPackages != null && donationReliefPackages.Any())
            {
                var packages = _reliefPackageRepository.GetListAsync(filter: f => donationReliefPackages.Select(dr => dr.ReliefPackageId).Contains(f.Id)).Result;

                var countId = 2;

                foreach (var drp in donationReliefPackages)
                {
                    var package = packages.FirstOrDefault(p => p.Id == drp.ReliefPackageId);
                    if (package != null)
                    {
                        reliefPackageRows += $@"
                            <tr>
                                <td>{countId}</td>
                                <td>{package.Name}</td>
                                <td>{package.TotalPrice:N0} VND</td>
                                <td>{drp.Quantity}</td>
                                <td>{drp.Quantity * package.TotalPrice:N0} VND</td>
                            </tr>";
                        countId++;
                    }
                }
            }

            return $@"
                <!DOCTYPE html>
                <html lang=""vi"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <style>
                        body {{
                            font-family: 'Arial', sans-serif;
                            margin: 0;
                            padding: 20px;
                            background-color: #f0f8f0;
                            color: #333;
                        }}
                        .invoice-container {{
                            max-width: 800px;
                            margin: 0 auto;
                            background: #fff;
                            padding: 30px;
                            border: 2px solid #4CAF50;
                            border-radius: 10px;
                            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                        }}
                        .header {{
                            text-align: center;
                            margin-bottom: 20px;
                        }}
                        .header h2 {{
                            margin: 0;
                            color: #4CAF50;
                            font-size: 24px;
                            font-weight: bold;
                        }}
                        .header p {{
                            margin: 5px 0;
                            font-size: 14px;
                            color: #666;
                        }}
                        .details {{
                            display: flex;
                            justify-content: space-between;
                            margin-bottom: 20px;
                            border-bottom: 1px solid #ddd;
                            padding-bottom: 10px;
                        }}
                        .details div {{
                            width: 45%;
                        }}
                        .details h3 {{
                            margin: 0 0 10px 0;
                            color: #4CAF50;
                            font-size: 18px;
                        }}
                        .details p {{
                            margin: 5px 0;
                            font-size: 14px;
                            color: #666;
                        }}
                        .table {{
                            width: 100%;
                            border-collapse: collapse;
                            margin: 20px 0;
                        }}
                        .table th, .table td {{
                            border: 1px solid #ddd;
                            padding: 12px;
                            text-align: left;
                            font-size: 14px;
                        }}
                        .table th {{
                            background-color: #4CAF50;
                            color: white;
                            font-weight: bold;
                        }}
                        .summary {{
                            text-align: right;
                            margin-top: 20px;
                            padding-top: 10px;
                            border-top: 1px solid #ddd;
                        }}
                        .summary p {{
                            margin: 5px 0;
                            font-size: 16px;
                            color: #333;
                            font-weight: bold;
                        }}
                        .summary p span {{
                            color: #4CAF50;
                        }}
                        .footer {{
                            text-align: center;
                            margin-top: 20px;
                            font-size: 12px;
                            color: #666;
                        }}
                        .footer a {{
                            color: #4CAF50;
                            text-decoration: none;
                        }}
                        .footer a:hover {{
                            text-decoration: underline;
                        }}
                    </style>
                </head>
                <body>
                    <h2>Cảm ơn bạn đã quyên góp cho chiến dịch: {causeTitle}</h2>
                    <br/>
                    <div class=""invoice-container"">
                        <div class=""header"">
                            <h2>HÓA ĐƠN</h2>
                            <p><strong>HOPEBOX</strong><br>Đại học FPT - Hòa Lạc<br>0868390784 - HopeBoxHola@gmail.com</p>
                        </div>
                        <div class=""details"">
                            <div>
                                <h3>THANH TOÁN CHO:</h3>
                                <p>{userName}</p>
                                <p>{userEmail}</p>
                                <p>{donation.DonationDate:dd/MM/yyyy}</p>
                                <p><strong>SỐ HÓA ĐƠN</strong><br>{donation.TradingCode}</p>
                                <p><strong>NGÀY HÓA ĐƠN</strong><br>{donation.DonationDate:dd/MM/yyyy HH:mm}</p>
                                <p><strong>HẠN THANH TOÁN</strong><br>{donation.DonationDate.AddDays(14):dd/MM/yyyy}</p>
                            </div>
                        </div>
                        <table class=""table"">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Mô tả</th>
                                    <th>Giá</th>
                                    <th>Số lượng</th>
                                    <th>Tổng</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>1</td>
                                    <td>Số tiền quyên góp</td>
                                    <td>{donation.DonationAmount:N0} VND</td>
                                    <td>1</td>
                                    <td>{donation.DonationAmount:N0} VND</td>
                                </tr>
                                {reliefPackageRows}
                            </tbody>
                        </table>
                        <div>
                            <p><strong>TỔNG CỘNG</strong> <span>{donation.Amount:N0} VND</span></p>
                            <p><strong>THUẾ (%)</strong> <span>0 VND</span></p>
                            <p><strong>TỔNG HÓA ĐƠN</strong> <span>{donation.Amount:N0} VND</span></p>
                        </div>
                        <div>
                            <h3>TÀI KHOẢN NGÂN HÀNG</h3>
                            <p>Tên công ty: HopeBox<br>Số tài khoản: 4271000599<br>Tên ngân hàng: BIDV</p>
                        </div>
                        <div class=""footer"">
                            <p><a href=""https://hopebox.long2003-2014.workers.dev/"">HopeBox</a></p>
                        </div>
                    </div>
                </body>
                </html>
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