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
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using static Azure.Core.HttpHeader;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Core.Service
{
    public class DonationService : BaseService<Donation, DonationDto>, IDonationService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<Cause> _causeRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IEmailService _emailService;

        public DonationService(IConfiguration configuration, IRepository<Donation> repository, IConverter<Donation, DonationDto> converter, IRepository<Cause> causeRepository, IRepository<User> userRepository, IEmailService emailService)
            : base(repository, converter)
        {
            _configuration = configuration;
            _causeRepository = causeRepository;
            _userRepository = userRepository;
            _emailService = emailService;
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

                if(cause.CurrentAmount >= cause.TargetAmount)
                {
                    cause.Status = CauseStatus.Completed;
                }

                await _causeRepository.UpdateAsync(cause);

                var user = await _userRepository.GetOneAsync(
                    filter: f => f.Id == donation.UserId);

                user.Point += (int) Math.Round(donation.Amount / 100000);

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
    }
}
