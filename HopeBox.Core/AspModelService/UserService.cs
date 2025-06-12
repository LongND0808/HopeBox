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
        private readonly IRepository<User> _repository;
        private readonly UserManager<User> _userManager;
        private readonly IConverter<User, UserDto> _converter;

        public UserService(
            IRepository<User> repository,
            UserManager<User> userManager,
            IConverter<User, UserDto> converter)
        {
            _repository = repository;
            _userManager = userManager;
            _converter = converter;
        }

        public async Task<BaseResponseDto<UserDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetOneAsyncUntracked<User>(f => f.Id == id);

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

                return new BaseResponseDto<UserDto>
                {
                    Status = 200,
                    Message = "Success",
                    ResponseData = dto
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<UserDto>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }

        public async Task<BaseResponseDto<IEnumerable<UserDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetListAsyncUntracked<User>();
                var dtos = _converter.ToListDTO(entities);

                return new BaseResponseDto<IEnumerable<UserDto>>
                {
                    Status = 200,
                    Message = "Success",
                    ResponseData = dtos
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<IEnumerable<UserDto>>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }

        public async Task<BaseResponseDto<bool>> AddAsync(UserDto dto)
        {
            try
            {
                var model = _converter.ToModel(dto);
                await _repository.AddAsync(model);

                return new BaseResponseDto<bool>
                {
                    Status = 201,
                    Message = "Add successful",
                    ResponseData = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = false
                };
            }
        }

        public async Task<BaseResponseDto<bool>> UpdateAsync(UserDto dto)
        {
            try
            {
                var entity = await _repository.GetOneAsync(filter: f => f.Id == dto.Id);
                if (entity == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "User not found",
                        ResponseData = false
                    };
                }

                var dtoProps = dto.GetType().GetProperties();
                var entityProps = entity.GetType().GetProperties();

                foreach (var dtoProp in dtoProps)
                {
                    var entityProp = entityProps.FirstOrDefault(p => p.Name == dtoProp.Name && p.CanWrite);
                    if (entityProp != null)
                    {
                        var dtoValue = dtoProp.GetValue(dto);
                        entityProp.SetValue(entity, dtoValue);
                    }
                }

                await _repository.UpdateAsync(entity);

                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Update successful",
                    ResponseData = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = false
                };
            }
        }


        public async Task<BaseResponseDto<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetOneAsyncUntracked<User>(f => f.Id == id);

                if (entity == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Entity not found.",
                        ResponseData = false
                    };
                }

                await _repository.DeleteAsync(entity);

                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Delete successful",
                    ResponseData = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = false
                };
            }
        }

        public async Task<BaseResponseDto<int>> GetCountAsync()
        {
            try
            {
                var count = await _repository.GetCount();

                return new BaseResponseDto<int>
                {
                    Status = 200,
                    Message = "Success",
                    ResponseData = count
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<int>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = 0
                };
            }
        }

        public async Task<BaseResponseDto<IEnumerable<UserDto>>> GetManagerAsync()
        {
            try
            {
                var managers = await _userManager.GetUsersInRoleAsync("Manager");

                var dtos = _converter.ToListDTO(managers);

                return new BaseResponseDto<IEnumerable<UserDto>>
                {
                    Status = 200,
                    Message = "Success",
                    ResponseData = dtos
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<IEnumerable<UserDto>>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }
    }
}
