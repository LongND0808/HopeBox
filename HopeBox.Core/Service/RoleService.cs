using HopeBox.Core.IService;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Core.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _repository;
        private readonly IConverter<Role, RoleDto> _converter;

        public RoleService(IRepository<Role> repository, IConverter<Role, RoleDto> converter)
        {
            _repository = repository;
            _converter = converter;
        }

        public async Task<BaseResponseDto<IEnumerable<RoleDto>>> GetAllRolesAsync()
        {
            try
            {
                var entities = await _repository.GetListAsyncUntracked<Role>();
                var dtos = _converter.ToListDTO(entities);

                return new BaseResponseDto<IEnumerable<RoleDto>>
                {
                    Status = 200,
                    Message = "Success",
                    ResponseData = dtos
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<IEnumerable<RoleDto>>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }
    }
}
