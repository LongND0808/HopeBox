using HopeBox.Domain.ResponseDto;

namespace HopeBox.Core.IService
{
    public interface IBaseService<TModel, TDto>
        where TModel : class
        where TDto : class
    {
        Task<BaseResponseDTO<TDto>> GetByIdAsync(Guid id);
        Task<BaseResponseDTO<IEnumerable<TDto>>> GetAllAsync();
        Task<BaseResponseDTO<bool>> AddAsync(TModel model);
        Task<BaseResponseDTO<bool>> UpdateAsync(TModel model);
        Task<BaseResponseDTO<bool>> DeleteAsync(Guid id);
    }
}
