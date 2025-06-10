using HopeBox.Core.IAspModelService;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopeBox.Core.IdentityModelService
{
    public class UserService : IUserService
    {
        protected readonly IRepository<User> _repository;
        protected readonly IConverter<User, UserDto> _converter;

        public UserService(IRepository<User> repository, IConverter<User, UserDto> converter)
        {
            _repository = repository;
            _converter = converter;
        }

        public async Task<BaseResponseDto<UserDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetOneAsyncUntracked<User>(filter: f => f.Id == id);
                if (entity == null)
                {
                    return new BaseResponseDto<UserDto>
                    {
                        Status = 404,
                        Message = "Entity not found.",
                        ResponseData = null
                    };
                }

                var dto = _converter.ToDTO(entity);
                return new BaseResponseDto<UserDto> { Status = 200, Message = "Success", ResponseData = dto };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<UserDto> { Status = 500, Message = ex.Message, ResponseData = null };
            }
        }

        public async Task<BaseResponseDto<IEnumerable<UserDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetListAsyncUntracked<User>();
                var dtos = _converter.ToListDTO(entities);
                return new BaseResponseDto<IEnumerable<UserDto>> { Status = 200, Message = "Success", ResponseData = dtos };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<IEnumerable<UserDto>> { Status = 500, Message = ex.Message, ResponseData = null };
            }
        }

        public async Task<BaseResponseDto<bool>> AddAsync(UserDto dto)
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

        public async Task<BaseResponseDto<bool>> UpdateAsync(UserDto dto)
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
                var entity = await _repository.GetOneAsyncUntracked<User>(filter: f => f.Id == id);
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
