using HopeBox.Core.IService;
using HopeBox.Core.Token;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.DTOs;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using System.Data.Entity;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Core.Service
{
    public class VolunteerService : BaseService<Volunteer, VolunteerDto>, IVolunteerService
    {
        protected readonly ITokenService _tokenService;
        protected readonly IRepository<User> _userRepository;
        protected readonly IRepository<Cause> _causeRepository;
        protected readonly IConverter<User, UserDto> _userConverter;
        protected readonly IConverter<Cause, CauseDto> _causeConverter;

        public VolunteerService(
            IRepository<Volunteer> repository,
            IConverter<Volunteer, VolunteerDto> converter,
            ITokenService tokenService,
            IRepository<User> userRepository,
            IRepository<Cause> causeRepository,
            IConverter<User, UserDto> userConverter,
            IConverter<Cause, CauseDto> causeConverter) : base(repository, converter)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _causeRepository = causeRepository;
            _userConverter = userConverter;
            _causeConverter = causeConverter;
        }

        #region RegisterVolunteer
        /// <summary>
        /// Mục đích: Đăng ký tình nguyện viên cho một hoạt động.
        /// </summary>
        public async Task<BaseResponseDto<VolunteerDto>> RegisterVolunteerAsync(VolunteerRequestDto request)
        {
            try
            {
                if (request.CauseId == Guid.Empty || request.UserId == Guid.Empty)
                {
                    return new BaseResponseDto<VolunteerDto>
                    {
                        Status = 400,
                        Message = "ID chiến dịch hoặc người dùng không hợp lệ.",
                        ResponseData = null
                    };
                }

                var existingVolunteer = await _repository.GetOneAsync(v =>
                    v.UserId == request.UserId && v.CauseId == request.CauseId && v.Status != VolunteerStatus.Rejected);

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
                    UserId = request.UserId,
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

        #region ApproveVolunteer
        /// <summary>
        /// Mục đích: Phê duyệt đơn đăng ký tình nguyện viên cho một chiến dịch cụ thể.
        /// </summary>
        public async Task<BaseResponseDto<bool>> ApproveVolunteerAsync(Guid volunteerId, Guid causeId)
        {
            try
            {
                var volunteer = await _repository.GetOneAsync(v => v.Id == volunteerId && v.CauseId == causeId);
                if (volunteer == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Không tìm thấy đơn đăng ký tình nguyện viên cho chiến dịch này.",
                        ResponseData = false
                    };
                }

                if (volunteer.Status != VolunteerStatus.Pending)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 400,
                        Message = "Đơn đăng ký không ở trạng thái chờ phê duyệt.",
                        ResponseData = false
                    };
                }

                volunteer.Status = VolunteerStatus.Approved;
                await _repository.UpdateAsync(volunteer);

                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Phê duyệt đơn đăng ký thành công.",
                    ResponseData = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = "Có lỗi xảy ra trong quá trình phê duyệt. Vui lòng thử lại sau.",
                    ResponseData = false
                };
            }
        }
        #endregion

        #region RejectVolunteer
        /// <summary>
        /// Mục đích: Từ chối đơn đăng ký tình nguyện viên cho một chiến dịch cụ thể.
        /// </summary>
        public async Task<BaseResponseDto<bool>> RejectVolunteerAsync(Guid volunteerId, Guid causeId)
        {
            try
            {
                var volunteer = await _repository.GetOneAsync(v => v.Id == volunteerId && v.CauseId == causeId);
                if (volunteer == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Không tìm thấy đơn đăng ký tình nguyện viên cho chiến dịch này.",
                        ResponseData = false
                    };
                }

                if (volunteer.Status != VolunteerStatus.Pending)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 400,
                        Message = "Đơn đăng ký không ở trạng thái chờ phê duyệt.",
                        ResponseData = false
                    };
                }

                volunteer.Status = VolunteerStatus.Rejected;
                await _repository.UpdateAsync(volunteer);

                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Từ chối đơn đăng ký thành công.",
                    ResponseData = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = "Có lỗi xảy ra trong quá trình từ chối. Vui lòng thử lại sau.",
                    ResponseData = false
                };
            }
        }
        #endregion

        #region GetVolunteerDetails
        /// <summary>
        /// Mục đích: Lấy thông tin chi tiết của tình nguyện viên, bao gồm thông tin người dùng và chiến dịch.
        /// </summary>
        public async Task<BaseResponseDto<VolunteerDetailDto>> GetVolunteerDetailsAsync(Guid volunteerId)
        {
            try
            {
                var volunteer = await _repository.GetOneAsyncUntracked<Volunteer>(v => v.Id == volunteerId);
                if (volunteer == null)
                {
                    return new BaseResponseDto<VolunteerDetailDto>
                    {
                        Status = 404,
                        Message = "Không tìm thấy thông tin tình nguyện viên.",
                        ResponseData = null
                    };
                }

                var user = await _userRepository.GetOneAsyncUntracked<User>(filter: f => f.Id == volunteer.UserId);
                var cause = await _causeRepository.GetOneAsyncUntracked<Cause>(filter: f => f.Id == volunteer.CauseId);

                var volunteerDto = _converter.ToDTO(volunteer);
                var userDto = _userConverter.ToDTO(user);
                var causeDto = _causeConverter.ToDTO(cause);

                var detailDto = new VolunteerDetailDto
                {
                    Volunteer = volunteerDto,
                    User = userDto,
                    Cause = causeDto
                };

                return new BaseResponseDto<VolunteerDetailDto>
                {
                    Status = 200,
                    Message = "Lấy thông tin chi tiết tình nguyện viên thành công.",
                    ResponseData = detailDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<VolunteerDetailDto>
                {
                    Status = 500,
                    Message = "Có lỗi xảy ra khi lấy thông tin chi tiết. Vui lòng thử lại sau.",
                    ResponseData = null
                };
            }
        }
        #endregion
    }
}