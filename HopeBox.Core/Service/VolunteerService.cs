using HopeBox.Core.IService;
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

        public VolunteerService(IRepository<Volunteer> repository, IConverter<Volunteer, VolunteerDto> converter)
        {
            _repository = repository;
            _converter = converter;
        }

        #region RegisterVolunteer
        /// <summary>
        /// Mục đích: Đăng ký tình nguyện viên cho một hoạt động.
        /// </summary>
        public async Task<BaseResponseDto<VolunteerDto>> RegisterVolunteerAsync(VolunteerRequestDto request)
        {
            try
            {
                var volunteer = new Volunteer
                {
                    UserId = request.UserId,
                    CauseId = request.CauseId,
                    JoinDate = request.JoinDate,
                    Status = VolunteerStatus.Pending

                };

                await _repository.AddAsync(volunteer);
                await _repository.SaveChangesAsync();

                var dto = _converter.ToDTO(volunteer);

                return new BaseResponseDto<VolunteerDto>
                {
                    Status = 201,
                    Message = "Đăng ký tình nguyện viên thành công!",
                    ResponseData = dto
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<VolunteerDto>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }
        #endregion
    }
}
