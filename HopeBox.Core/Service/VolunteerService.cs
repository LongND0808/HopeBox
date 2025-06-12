using HopeBox.Core.IService;
using HopeBox.Core.Token;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Core.Service
{
    public class VolunteerService : IVolunteerService
    {
        protected readonly IRepository<Volunteer> _repository;
        protected readonly IConverter<Volunteer, VolunteerDto> _converter;
        protected readonly ITokenService _tokenservice;

        public VolunteerService(IRepository<Volunteer> repository, IConverter<Volunteer, VolunteerDto> converter, ITokenService tokenService)
        {
            _repository = repository;
            _converter = converter;
            _tokenservice = tokenService;
        }

        #region RegisterVolunteer
        /// <summary>
        /// Mục đích: Đăng ký tình nguyện viên cho một hoạt động.
        /// </summary>
        public async Task<BaseResponseDto<VolunteerDto>> RegisterVolunteerAsync(VolunteerRequestDto request)
        {
            try
            {
                string userid = _tokenservice.GetCurrentUserId() ?? "";

                if (request.CauseId == Guid.Empty)
                {
                    return new BaseResponseDto<VolunteerDto>
                    {
                        Status = 400,
                        Message = "ID chiến dịch không hợp lệ.",
                        ResponseData = null
                    };
                }
                var existingVolunteer = await _repository.GetOneAsync(v =>
                    v.UserId == Guid.Parse(userid) && v.CauseId == request.CauseId);

                if (existingVolunteer != null)
                {
                    return new BaseResponseDto<VolunteerDto>
                    {
                        Status = 409,
                        Message = "Bạn đã đăng ký tình nguyện viên cho chiến dịch này rồi.",
                        ResponseData = null
                    };
                }
                var volunteer = new Volunteer
                {
                    UserId = Guid.Parse(userid),
                    CauseId = request.CauseId,
                    Status = VolunteerStatus.Pending,
                    JoinDate = DateTime.Now
                };

                await _repository.AddAsync(volunteer);

                var dto = _converter.ToDTO(volunteer);

                return new BaseResponseDto<VolunteerDto>
                {
                    Status = 201,
                    Message = "Đăng ký tình nguyện viên thành công!",
                    ResponseData = dto
                };
            }
            catch (FormatException ex)
            {
                return new BaseResponseDto<VolunteerDto>
                {
                    Status = 400,
                    Message = "Dữ liệu không đúng định dạng.",
                    ResponseData = null
                };
            }
            catch (ArgumentException ex)
            {
                return new BaseResponseDto<VolunteerDto>
                {
                    Status = 400,
                    Message = "Tham số không hợp lệ.",
                    ResponseData = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<VolunteerDto>
                {
                    Status = 500,
                    Message = "Có lỗi xảy ra trong quá trình đăng ký. Vui lòng thử lại sau.",
                    ResponseData = null
                };
            }
        }
        #endregion
    }
}
