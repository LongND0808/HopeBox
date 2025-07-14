using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;
using Microsoft.AspNetCore.Identity;
using HopeBox.Domain.ResponseDto;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController
    {
        public IRoleService _roleService { get; set; }
        public RoleController(IRoleService service)
        {
            _roleService = service;
        }

        [HttpGet("get-all")]
        public async Task<BaseResponseDto<IEnumerable<RoleDto>>> GetAllRolesAsync()
        {
            return await _roleService.GetAllRolesAsync();
        }
    }
}