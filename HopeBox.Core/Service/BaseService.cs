using HopeBox.Core.IService;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Models;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;

namespace HopeBox.Core.Service
{
    public class BaseService<TModel, TDto> : IBaseService<TModel, TDto>
        where TModel : class
        where TDto : class
    {
        protected readonly IRepository<TModel> _repository;
        protected readonly IConverter<TModel, TDto> _converter;

        public BaseService(IRepository<TModel> repository, IConverter<TModel, TDto> converter)
        {
            _repository = repository;
            _converter = converter;
        }

        public async Task<BaseResponseDTO<TDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetOneAsyncUntracked<TModel>(filter: f => (f as BaseModel).Id == id);
                if (entity == null)
                {
                    return new BaseResponseDTO<TDto>
                    {
                        Status = 404,
                        Message = "Entity not found.",
                        Data = null
                    };
                }

                var dto = _converter.ToDTO(entity);
                return new BaseResponseDTO<TDto> { Status = 200, Message = "Success", Data = dto };
            }
            catch (Exception ex)
            {
                return new BaseResponseDTO<TDto> { Status = 500, Message = ex.Message, Data = null };
            }
        }

        public async Task<BaseResponseDTO<IEnumerable<TDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetListAsyncUntracked<TModel>();
                var dtos = _converter.ToListDTO(entities);
                return new BaseResponseDTO<IEnumerable<TDto>> { Status = 200, Message = "Success", Data = dtos };
            }
            catch (Exception ex)
            {
                return new BaseResponseDTO<IEnumerable<TDto>> { Status = 500, Message = ex.Message, Data = null };
            }
        }

        public async Task<BaseResponseDTO<bool>> AddAsync(TModel model)
        {
            try
            {
                await _repository.AddAsync(model);
                await _repository.SaveChangesAsync();
                return new BaseResponseDTO<bool> { Status = 201, Message = "Add successful", Data = true };
            }
            catch (Exception ex)
            {
                return new BaseResponseDTO<bool> { Status = 500, Message = ex.Message, Data = false };
            }
        }

        public async Task<BaseResponseDTO<bool>> UpdateAsync(TModel model)
        {
            try
            {
                await _repository.UpdateAsync(model);
                await _repository.SaveChangesAsync();
                return new BaseResponseDTO<bool> { Status = 200, Message = "Update successful", Data = true };
            }
            catch (Exception ex)
            {
                return new BaseResponseDTO<bool> { Status = 500, Message = ex.Message, Data = false };
            }
        }

        public async Task<BaseResponseDTO<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetOneAsyncUntracked<TModel>(filter: f => (f as BaseModel).Id == id);
                if (entity == null)
                {
                    return new BaseResponseDTO<bool> { Status = 404, Message = "Entity not found.", Data = false };
                }

                await _repository.DeleteAsync(entity);
                await _repository.SaveChangesAsync();
                return new BaseResponseDTO<bool> { Status = 200, Message = "Delete successful", Data = true };
            }
            catch (Exception ex)
            {
                return new BaseResponseDTO<bool> { Status = 500, Message = ex.Message, Data = false };
            }
        }
    }
}
