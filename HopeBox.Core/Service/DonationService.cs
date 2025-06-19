using HopeBox.Core.Email;
using HopeBox.Core.IService;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Core.Service
{
    public class DonationService : BaseService<Donation, DonationDto>, IDonationService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<Cause> _causeRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IEmailService _emailService;
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<ReliefPackage> _reliefPackageRepository;
        private readonly IRepository<EventReliefPackage> _eventReliefPackageRepository;
        private readonly IConverter<ReliefPackage, ReliefPackageDto> _reliefPackageConverter;

        public DonationService(
            IConfiguration configuration,
            IRepository<Donation> repository,
            IConverter<Donation, DonationDto> converter,
            IRepository<Cause> causeRepository,
            IRepository<User> userRepository,
            IEmailService emailService,
            IRepository<Event> eventRepository,
            IRepository<ReliefPackage> reliefPackageRepository,
            IRepository<EventReliefPackage> eventReliefPackageRepository,
            IConverter<ReliefPackage, ReliefPackageDto> reliefPackageConverter)
            : base(repository, converter)
        {
            _configuration = configuration;
            _causeRepository = causeRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _eventRepository = eventRepository;
            _reliefPackageRepository = reliefPackageRepository;
            _eventReliefPackageRepository = eventReliefPackageRepository;
            _reliefPackageConverter = reliefPackageConverter;
        }

        public async Task<BaseResponseDto<string>> CreateVNPayPaymentUrlAsync(Guid donationId, string ipAddress)
        {
            var donation = await _repository.GetOneAsyncUntracked<Donation>(filter: f => f.Id == donationId);
            if (donation == null)
            {
                return new BaseResponseDto<string>
                {
                    Status = 404,
                    Message = "Không tìm thấy donationDto",
                    ResponseData = null
                };
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
                { "vnp_OrderType", "donationDto" },
                { "vnp_Locale", "vn" },
                { "vnp_ReturnUrl", returnUrl },
                { "vnp_IpAddr", ipAddress ?? "127.0.0.1" },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss") }
            };

            string queryString = string.Join("&", payParams.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));
            string secureHash = HmacSHA512(vnp_HashSecret, queryString);
            payParams.Add("vnp_SecureHash", secureHash);

            await UpdateTradingCodeAsync(donationId, vnp_TxnRef);

            string paymentUrl = vnp_Url + "?" + string.Join("&", payParams.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));

            return new BaseResponseDto<string>
            {
                Status = 200,
                Message = "Tạo URL thanh toán thành công",
                ResponseData = paymentUrl
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
                    Message = "Không tìm thấy donationDto",
                    ResponseData = false
                };
            }

            if (donation == null)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 404,
                    Message = "Không tìm thấy donation",
                    ResponseData = false
                };
            }

            if (dto.vnp_ResponseCode == "00")
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

                var user = await _userRepository.GetOneAsync(
                    filter: f => f.Id == donation.UserId);

                user.Point += (int)Math.Round(donation.Amount / 100000);

                if (user == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Không tìm thấy email người dùng",
                        ResponseData = false
                    };
                }

                string subject = "Hóa đơn quyên góp - HopeBox";
                string body = GenerateInvoiceHtml(donation, user.FullName, user.Email, cause.Title);
                await _emailService.SendEmailAsync(user.Email, subject, body);


                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Thanh toán thành công",
                    ResponseData = true
                };
            }

            return new BaseResponseDto<bool>
            {
                Status = 400,
                Message = "Thanh toán không thành công",
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
                <p><strong>ID giao dịch ngân hàng:</strong> {donation.TransactionId}</p>
                <p><strong>Chiến dịch:</strong> {causeTitle}</p>
                <br/>
                <p>Một lần nữa, chúng tôi xin chân thành cảm ơn bạn vì sự đóng góp quý báu này!</p>
                <p>HopeBox Team ❤️</p>
            ";
        }


        public async Task<BaseResponseDto<DonationDto>> GetByTradingCodeAsync(string tradingCode)
        {
            var donation = await _repository.GetOneAsyncUntracked<Donation>(filter: d => d.TradingCode == tradingCode);
            if (donation == null)
            {
                return new BaseResponseDto<DonationDto>
                {
                    Status = 404,
                    Message = "Không tìm thấy donationDto",
                    ResponseData = null
                };
            }

            var dto = _converter.ToDTO(donation);
            return new BaseResponseDto<DonationDto>
            {
                Status = 200,
                Message = "Thành công",
                ResponseData = dto
            };
        }

        public async Task<BaseResponseDto<bool>> MarkAsPaidAsync(Guid donationId, string transactionId)
        {
            var donation = await _repository.GetOneAsyncUntracked<Donation>(filter: f => f.Id == donationId);
            if (donation == null)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 404,
                    Message = "Không tìm thấy donationDto",
                    ResponseData = false
                };
            }

            donation.TransactionId = transactionId;
            await _repository.UpdateAsync(donation);

            return new BaseResponseDto<bool>
            {
                Status = 200,
                Message = "Đánh dấu thanh toán thành công",
                ResponseData = true
            };
        }

        public async Task<BaseResponseDto<bool>> UpdateTradingCodeAsync(Guid donationId, string tradingCode)
        {
            var donation = await _repository.GetOneAsyncUntracked<Donation>(filter: f => f.Id == donationId);
            if (donation == null)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 404,
                    Message = "Không tìm thấy donationDto",
                    ResponseData = false
                };
            }

            donation.TradingCode = tradingCode;
            await _repository.UpdateAsync(donation);

            return new BaseResponseDto<bool>
            {
                Status = 200,
                Message = "Cập nhật mã giao dịch thành công",
                ResponseData = true
            };
        }

        private static string HmacSHA512(string key, string input)
        {
            var hash = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(key));
            var bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public async Task<BaseResponseDto<string>> CreateDonation(DonationDto donationDto)
        {
            try
            {
                if (donationDto.UserId == null || donationDto.UserId == Guid.Empty)
                {
                    return new BaseResponseDto<string>
                    {
                        Status = 400,
                        Message = "UserId is missing in donationDto data",
                        ResponseData = null
                    };
                }

                var model = _converter.ToModel(donationDto);

                await _repository.AddAsync(model);

                var donationId = model.Id.ToString();

                return new BaseResponseDto<string>
                {
                    Status = 200,
                    Message = "Đơn quyên góp đã được tạo thành công",
                    ResponseData = donationId
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<string>
                {
                    Status = 500,
                    Message = "Internal server error: " + ex.Message,
                    ResponseData = null
                };
            }
        }

        #region Event Donation Methods

        /// <summary>
        /// Tạo donation cho Event (cash hoặc package với EventReliefPackage)
        /// </summary>
        public async Task<BaseResponseDto<EventDonationResponseDto>> CreateEventDonationAsync(
            Guid userId, EventDonationRequestDto request)
        {
            try
            {
                var eventEntity = await _eventRepository.GetOneAsyncUntracked<Event>(
                    filter: e => e.Id == request.EventId,
                    include: q => q.Include(e => e.Cause)
                );

                if (eventEntity == null)
                {
                    return new BaseResponseDto<EventDonationResponseDto>
                    {
                        Status = 404,
                        Message = "Không tìm thấy sự kiện",
                        ResponseData = null
                    };
                }

                var user = await _userRepository.GetOneAsyncUntracked<User>(
                    filter: u => u.Id == userId
                );

                if (user == null)
                {
                    return new BaseResponseDto<EventDonationResponseDto>
                    {
                        Status = 404,
                        Message = "Không tìm thấy thông tin người dùng",
                        ResponseData = null
                    };
                }

                decimal totalAmount = 0;
                var packageDetails = new List<DonatedPackageInfo>();

                if (request.IsPackageDonation)
                {
                    if (request.Packages == null || !request.Packages.Any())
                    {
                        return new BaseResponseDto<EventDonationResponseDto>
                        {
                            Status = 400,
                            Message = "Vui lòng chọn ít nhất một gói cứu trợ",
                            ResponseData = null
                        };
                    }

                    var packageCalculation = await CalculatePackageTotalAsync(request.Packages);
                    totalAmount = packageCalculation.TotalAmount;
                    packageDetails = packageCalculation.PackageDetails;
                }
                else
                {
                    totalAmount = request.CashAmount ?? 0;
                }

                if (totalAmount <= 0)
                {
                    return new BaseResponseDto<EventDonationResponseDto>
                    {
                        Status = 400,
                        Message = "Số tiền quyên góp phải lớn hơn 0",
                        ResponseData = null
                    };
                }

                var donation = new Donation
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CauseId = eventEntity.CauseId ?? eventEntity.Cause.Id,
                    EventId = request.EventId,
                    Amount = totalAmount,
                    DonationDate = DateTime.UtcNow,
                    PaymentMethod = request.PaymentMethod,
                    Status = DonationStatus.Pending,
                    TradingCode = Guid.NewGuid().ToString("N")
                };

                await _repository.AddAsync(donation);

                var eventReliefPackageIds = new List<Guid>();
                if (request.IsPackageDonation && request.Packages?.Any() == true)
                {
                    eventReliefPackageIds = await CreateEventReliefPackagesAsync(
                        donation.Id, request.EventId, request.Packages, user.Address, eventEntity.Location);
                }

                var paymentResult = await CreateVNPayPaymentUrlAsync(donation.Id, "127.0.0.1");
                string paymentUrl = paymentResult.ResponseData ?? "";

                DeliveryInfo? deliveryInfo = null;
                if (request.IsPackageDonation)
                {
                    deliveryInfo = new DeliveryInfo
                    {
                        SenderAddress = user.Address ?? "Địa chỉ không xác định",
                        ReceiverAddress = eventEntity.Location ?? "Địa điểm sự kiện không xác định",
                        EventReliefPackageIds = eventReliefPackageIds
                    };
                }

                return new BaseResponseDto<EventDonationResponseDto>
                {
                    Status = 200,
                    Message = "Tạo donation thành công",
                    ResponseData = new EventDonationResponseDto
                    {
                        DonationId = donation.Id,
                        TotalAmount = totalAmount,
                        PaymentUrl = paymentUrl,
                        IsPackageDonation = request.IsPackageDonation,
                        PackageDetails = packageDetails,
                        DeliveryInfo = deliveryInfo
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<EventDonationResponseDto>
                {
                    Status = 500,
                    Message = $"Lỗi hệ thống: {ex.Message}",
                    ResponseData = null
                };
            }
        }

        /// <summary>
        /// Tạo EventReliefPackage records với address mapping
        /// </summary>
        private async Task<List<Guid>> CreateEventReliefPackagesAsync(
            Guid donationId, Guid eventId, List<PackageDonationItem> packages,
            string? senderAddress, string? receiverAddress)
        {
            var eventReliefPackageIds = new List<Guid>();

            foreach (var package in packages)
            {
                var eventReliefPackage = new EventReliefPackage
                {
                    Id = Guid.NewGuid(),
                    EventId = eventId,
                    ReliefPackageId = package.ReliefPackageId,
                    DonationId = donationId,
                    Quantity = package.Quantity,
                    SenderAddress = senderAddress ?? "Địa chỉ không xác định",
                    ReceiverAddress = receiverAddress ?? "Địa điểm sự kiện không xác định",
                    DonationDate = DateTime.UtcNow
                };

                await _eventReliefPackageRepository.AddAsync(eventReliefPackage);
                eventReliefPackageIds.Add(eventReliefPackage.Id);
            }

            return eventReliefPackageIds;
        }

        /// <summary>
        /// Tính tổng tiền cho package donation
        /// </summary>
        private async Task<(decimal TotalAmount, List<DonatedPackageInfo> PackageDetails)>
            CalculatePackageTotalAsync(List<PackageDonationItem> packages)
        {
            decimal total = 0;
            var details = new List<DonatedPackageInfo>();

            foreach (var package in packages)
            {
                var reliefPackage = await _reliefPackageRepository.GetOneAsyncUntracked<ReliefPackage>(
                    filter: rp => rp.Id == package.ReliefPackageId,
                    include: q => q.Include(rp => rp.PackageItems)
                                  .ThenInclude(pi => pi.ReliefItem)
                );

                if (reliefPackage != null)
                {
                    var packageTotal = reliefPackage.TotalAmount * package.Quantity;
                    total += packageTotal;

                    details.Add(new DonatedPackageInfo
                    {
                        PackageName = reliefPackage.Name,
                        Description = reliefPackage.Description,
                        Quantity = package.Quantity,
                        UnitPrice = reliefPackage.TotalAmount,
                        TotalPrice = packageTotal
                    });
                }
            }

            return (total, details);
        }

        /// <summary>
        /// Xử lý VNPay return cho Event donation với EventReliefPackage
        /// </summary>
        public async Task<BaseResponseDto<bool>> HandleEventVNPayReturnAsync(VNPayReturnRequestDto dto)
        {
            var donation = await _repository.GetOneAsync(
                filter: f => f.TradingCode == dto.vnp_TxnRef,
                include: q => q.Include(d => d.Event)
                              .Include(d => d.Cause)
                              .Include(d => d.User)
            );

            if (donation == null)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 404,
                    Message = "Không tìm thấy donation",
                    ResponseData = false
                };
            }

            if (dto.vnp_ResponseCode == "00")
            {
                donation.TransactionId = dto.vnp_TransactionNo;
                donation.Status = DonationStatus.Paid;
                await _repository.UpdateAsync(donation);

                if (donation.Event != null)
                {
                    donation.Event.CurrentAmount += donation.Amount;
                    await _eventRepository.UpdateAsync(donation.Event);
                }

                if (donation.Cause != null)
                {
                    donation.Cause.CurrentAmount += donation.Amount;

                    if (donation.Cause.CurrentAmount >= donation.Cause.TargetAmount)
                    {
                        donation.Cause.Status = CauseStatus.Completed;
                    }

                    await _causeRepository.UpdateAsync(donation.Cause);
                }

                if (donation.User != null)
                {
                    donation.User.Point += (int)Math.Round(donation.Amount / 100000);
                    await _userRepository.UpdateAsync(donation.User);
                }

                await SendEventDonationReceiptAsync(donation);

                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Thanh toán thành công",
                    ResponseData = true
                };
            }
            else
            {
                await CleanupFailedEventReliefPackagesAsync(donation.Id);
            }

            return new BaseResponseDto<bool>
            {
                Status = 400,
                Message = "Thanh toán không thành công",
                ResponseData = false
            };
        }

        /// <summary>
        /// Cleanup EventReliefPackage khi payment failed
        /// </summary>
        private async Task CleanupFailedEventReliefPackagesAsync(Guid donationId)
        {
            try
            {
                var eventReliefPackages = await _eventReliefPackageRepository.GetListAsyncUntracked<EventReliefPackage>(
                    filter: erp => erp.DonationId == donationId
                );

                foreach (var package in eventReliefPackages)
                {
                    await _eventReliefPackageRepository.DeleteAsync(package);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to cleanup EventReliefPackages: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách ReliefPackage chi tiết của Event
        /// </summary>
        public async Task<BaseResponseDto<List<ReliefPackageDto>>> GetEventReliefPackagesAsync(Guid eventId)
        {
            try
            {
                var eventEntity = await _eventRepository.GetOneAsyncUntracked<Event>(
                    filter: e => e.Id == eventId,
                    include: q => q.Include(e => e.Cause)
                                  .ThenInclude(c => c.ReliefPackages)
                                  .ThenInclude(rp => rp.PackageItems)
                                  .ThenInclude(pi => pi.ReliefItem)
                );

                if (eventEntity?.Cause?.ReliefPackages == null)
                {
                    return new BaseResponseDto<List<ReliefPackageDto>>
                    {
                        Status = 404,
                        Message = "Không tìm thấy gói cứu trợ cho sự kiện này",
                        ResponseData = new List<ReliefPackageDto>()
                    };
                }

                var reliefPackageDtos = eventEntity.Cause.ReliefPackages.Select(rp => new ReliefPackageDto
                {
                    Id = rp.Id,
                    CauseId = rp.CauseId,
                    Name = rp.Name,
                    Description = rp.Description,
                    TotalAmount = rp.TotalAmount,
                    PackageItems = rp.PackageItems?.Select(pi => new ReliefPackageItemDto
                    {
                        Id = pi.Id,
                        ReliefPackageId = pi.ReliefPackageId,
                        ReliefItemId = pi.ReliefItemId,
                        Quantity = pi.Quantity,
                        TotalPrice = pi.TotalPrice,
                        ReliefItem = pi.ReliefItem != null ? new ReliefItemDto
                        {
                            Id = pi.ReliefItem.Id,
                            ItemName = pi.ReliefItem.ItemName,
                            Unit = (Unit)pi.ReliefItem.Unit,
                            UnitPrice = pi.ReliefItem.UnitPrice
                        } : null
                    }).ToList() ?? new List<ReliefPackageItemDto>()
                }).ToList();

                return new BaseResponseDto<List<ReliefPackageDto>>
                {
                    Status = 200,
                    Message = "Lấy danh sách gói cứu trợ thành công",
                    ResponseData = reliefPackageDtos
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<List<ReliefPackageDto>>
                {
                    Status = 500,
                    Message = $"Lỗi hệ thống: {ex.Message}",
                    ResponseData = new List<ReliefPackageDto>()
                };
            }
        }

        /// <summary>
        /// Send email receipt for event donation với EventReliefPackage info
        /// </summary>
        private async Task SendEventDonationReceiptAsync(Donation donation)
        {
            try
            {
                if (donation.User?.Email == null) return;

                var eventReliefPackages = await _eventReliefPackageRepository.GetListAsyncUntracked<EventReliefPackage>(
                    filter: erp => erp.DonationId == donation.Id,
                    include: q => q.Include(erp => erp.ReliefPackage)
                );

                string subject = "Hóa đơn quyên góp sự kiện - HopeBox";
                string body = GenerateEventInvoiceHtml(donation, eventReliefPackages);
                await _emailService.SendEmailAsync(donation.User.Email, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }

        /// <summary>
        /// Generate HTML for event donation invoice với EventReliefPackage
        /// </summary>
        private string GenerateEventInvoiceHtml(Donation donation, IEnumerable<EventReliefPackage> eventReliefPackages)
        {
            var packageInfo = "";
            var deliveryInfo = "";

            if (eventReliefPackages?.Any() == true)
            {
                packageInfo = "<h3>Gói cứu trợ đã quyên góp:</h3><ul>";
                foreach (var pkg in eventReliefPackages)
                {
                    packageInfo += $"<li>{pkg.ReliefPackage?.Name} - Số lượng: {pkg.Quantity}</li>";
                }
                packageInfo += "</ul>";

                var firstPackage = eventReliefPackages.First();
                deliveryInfo = $@"
                <h3>Thông tin giao hàng:</h3>
                <p><strong>Địa chỉ gửi:</strong> {firstPackage.SenderAddress}</p>
                <p><strong>Địa chỉ nhận:</strong> {firstPackage.ReceiverAddress}</p>
                <p><strong>Ngày quyên góp:</strong> {firstPackage.DonationDate:dd/MM/yyyy HH:mm}</p>
            ";
            }

            return $@"
            <h2>Cảm ơn bạn đã quyên góp cho sự kiện - HopeBox!</h2>
            <p><strong>Tên người dùng:</strong> {donation.User?.FullName}</p>
            <p><strong>Email:</strong> {donation.User?.Email}</p>
            <p><strong>Sự kiện:</strong> {donation.Event?.Title}</p>
            <p><strong>Chiến dịch:</strong> {donation.Cause?.Title}</p>
            <p><strong>Số tiền:</strong> {donation.Amount:N0} VND</p>
            <p><strong>Ngày quyên góp:</strong> {donation.DonationDate:dd/MM/yyyy HH:mm}</p>
            <p><strong>Mã giao dịch:</strong> {donation.TradingCode}</p>
            <p><strong>ID giao dịch ngân hàng:</strong> {donation.TransactionId}</p>
            {packageInfo}
            {deliveryInfo}
            <br/>
            <p>Một lần nữa, chúng tôi xin chân thành cảm ơn bạn vì sự đóng góp quý báu này!</p>
            <p>HopeBox Team ❤️</p>
        ";
        }
        #endregion

    }
}
