using HopeBox.Core.IService;
using HopeBox.Core.Service;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReliefPackageController : BaseController<ReliefPackage, ReliefPackageDto>
    {
        private readonly IReliefPackageService _reliefPackageService;
        public ReliefPackageController(IBaseService<ReliefPackage, ReliefPackageDto> service, IReliefPackageService reliefPackageService) : base(service)
        {
            _reliefPackageService = reliefPackageService;
        }

        [HttpGet("get-relief-packages-by-cause-id")]
        public async Task<BaseResponseDto<IEnumerable<ReliefPackageDto>>> GetReliefPackages([FromQuery] Guid causeId)
        {
            var result = await _reliefPackageService.GetReliefPackagesByCauseIdAsync(causeId);
            return result;
        }

        [HttpPost("create-relief-package")]
        [Authorize(Roles = "Admin")]
        public async Task<BaseResponseDto<bool>> CreateReliefPackage(CreateReliefPackageRequestDto dto)
        {
            var result = await _reliefPackageService.CreateReliefPackageAsync(dto);
            return result;
        }

        [HttpPost("update-relief-package")]
        [Authorize(Roles = "Admin")]
        public async Task<BaseResponseDto<bool>> UpdateReliefPackage(UpdateReliefPackageRequestDto dto)
        {
            var result = await _reliefPackageService.UpdateReliefPackageAsync(dto);
            return result;
        }

        [HttpPost("delete-relief-package")]
        [Authorize(Roles = "Admin")]
        public async Task<BaseResponseDto<bool>> DeleteReliefPackage([FromBody] Guid packageId)
        {
            var result = await _reliefPackageService.DeleteReliefPackageAsync(packageId);
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("change-image")]
        public async Task<BaseResponseDto<string>> ChangeImage(string packageId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new BaseResponseDto<string> { Status = 400, Message = "File rỗng" };

            var result = await _reliefPackageService.ChangeImageAsync(Guid.Parse(packageId), file);
            return result;
        }
    }
}
