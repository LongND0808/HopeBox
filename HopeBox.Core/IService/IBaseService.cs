using HopeBox.Domain.ResponseDto;

namespace HopeBox.Core.IService
{
    public interface IBaseService<TModel, TDto>
        where TModel : class
        where TDto : class
    {
        Task<BaseResponseDto<int>> GetCountAsync();
        Task<BaseResponseDto<TDto>> GetByIdAsync(Guid id);
        Task<BaseResponseDto<IEnumerable<TDto>>> GetAllAsync();
        Task<BaseResponseDto<bool>> AddAsync(TDto model);
        Task<BaseResponseDto<bool>> UpdateAsync(TDto model);
        Task<BaseResponseDto<bool>> DeleteAsync(Guid id);
        Task<BaseResponseDto<bool>> DeleteAllAsync();
    }
}
