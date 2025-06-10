using HopeBox.Core.IService;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Models;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;

namespace HopeBox.Core.Service
{
    public class BaseService<TModel, TDto> : IBaseService<TModel, TDto>
        where TModel : BaseModel
        where TDto : class
    {
        protected readonly IRepository<TModel> _repository;
        protected readonly IConverter<TModel, TDto> _converter;

        public BaseService(IRepository<TModel> repository, IConverter<TModel, TDto> converter)
        {
            _repository = repository;
            _converter = converter;
        }

        public async Task<BaseResponseDto<TDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetOneAsyncUntracked<TModel>(filter: f => f.Id == id);
                if (entity == null)
                {
                    return new BaseResponseDto<TDto>
                    {
                        Status = 404,
                        Message = "Entity not found.",
                        ResponseData = null
                    };
                }

                var dto = _converter.ToDTO(entity);
                return new BaseResponseDto<TDto> { Status = 200, Message = "Success", ResponseData = dto };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<TDto> { Status = 500, Message = ex.Message, ResponseData = null };
            }
        }

        public async Task<BaseResponseDto<IEnumerable<TDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetListAsyncUntracked<TModel>();
                var dtos = _converter.ToListDTO(entities);
                return new BaseResponseDto<IEnumerable<TDto>> { Status = 200, Message = "Success", ResponseData = dtos };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<IEnumerable<TDto>> { Status = 500, Message = ex.Message, ResponseData = null };
            }
        }

        public async Task<BaseResponseDto<bool>> AddAsync(TDto dto)
        {
            try
            {
                var model = _converter.ToModel(dto);
                await _repository.AddAsync(model);
                return new BaseResponseDto<bool> { Status = 201, Message = "Add successful", ResponseData = true };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<bool> { Status = 500, Message = ex.Message, ResponseData = false };
            }
        }

        public async Task<BaseResponseDto<bool>> UpdateAsync(TDto dto)
        {
            try
            {
                var model = _converter.ToModel(dto);
                await _repository.UpdateAsync(model);
                return new BaseResponseDto<bool> { Status = 200, Message = "Update successful", ResponseData = true };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<bool> { Status = 500, Message = ex.Message, ResponseData = false };
            }
        }

        public async Task<BaseResponseDto<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetOneAsyncUntracked<TModel>(filter: f => (f as BaseModel).Id == id);
                if (entity == null)
                {
                    return new BaseResponseDto<bool> { Status = 404, Message = "Entity not found.", ResponseData = false };
                }

                await _repository.DeleteAsync(entity);
                return new BaseResponseDto<bool> { Status = 200, Message = "Delete successful", ResponseData = true };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<bool> { Status = 500, Message = ex.Message, ResponseData = false };
            }
        }
    }
}
