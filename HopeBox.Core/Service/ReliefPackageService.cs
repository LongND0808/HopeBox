using HopeBox.Core.IService;
using HopeBox.Core.R2Storage;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Core.Service
{
    public class ReliefPackageService : BaseService<ReliefPackage, ReliefPackageDto>, IReliefPackageService
    {
        private readonly IRepository<ReliefPackageItem> _reliefPackageItemRepository;
        private readonly IR2StorageService _r2StorageService;
        public ReliefPackageService(IRepository<ReliefPackage> repository, IConverter<ReliefPackage, ReliefPackageDto> converter, IRepository<ReliefPackageItem> reliefPackageItemRepository, IR2StorageService r2StorageService) : base(repository, converter)
        {
            _reliefPackageItemRepository = reliefPackageItemRepository;
            _r2StorageService = r2StorageService;
        }

        public async Task<BaseResponseDto<bool>> CreateReliefPackageAsync(CreateReliefPackageRequestDto dto)
        {
            try
            {
                var package = new ReliefPackage
                {
                    Id = Guid.NewGuid(),
                    CauseId = dto.CauseId,
                    Name = dto.Name,
                    Description = dto.Description,
                    ExtraFee = dto.ExtraFee,
                    CurrentQuantity = dto.CurrentQuantity,
                    TargetQuantity = dto.TargetQuantity,
                    TotalPrice = dto.TotalPrice,
                    Image = dto.Image
                };

                await _repository.AddAsync(package);

                foreach (var item in dto.SelectedItems)
                {
                    var reliefPackageItem = new ReliefPackageItem
                    {
                        Id = Guid.NewGuid(),
                        ReliefPackageId = package.Id,
                        ReliefItemId = item.Key,
                        Quantity = item.Value
                    };
                    await _reliefPackageItemRepository.AddAsync(reliefPackageItem);
                }

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

        public async Task<BaseResponseDto<bool>> UpdateReliefPackageAsync(UpdateReliefPackageRequestDto dto)
        {
            try
            {
                var existingPackage = await _repository.GetOneAsync(filter: p => p.Id == dto.Id);

                if (existingPackage == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Relief package not found",
                        ResponseData = false
                    };
                }

                existingPackage.CauseId = dto.CauseId;
                existingPackage.Name = dto.Name;
                existingPackage.Description = dto.Description;
                existingPackage.ExtraFee = dto.ExtraFee;
                existingPackage.CurrentQuantity = dto.CurrentQuantity;
                existingPackage.TargetQuantity = dto.TargetQuantity;
                existingPackage.TotalPrice = dto.TotalPrice;
                existingPackage.Image = dto.Image;

                await _repository.UpdateAsync(existingPackage);

                var existingItems = await _reliefPackageItemRepository.GetListAsyncUntracked<ReliefPackageItem>(
                    filter: i => i.ReliefPackageId == dto.Id);

                foreach (var item in existingItems)
                {
                    await _reliefPackageItemRepository.DeleteAsync(item);
                }

                foreach (var item in dto.SelectedItems)
                {
                    var reliefPackageItem = new ReliefPackageItem
                    {
                        Id = Guid.NewGuid(),
                        ReliefPackageId = dto.Id,
                        ReliefItemId = item.Key,
                        Quantity = item.Value
                    };
                    await _reliefPackageItemRepository.AddAsync(reliefPackageItem);
                }

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

        public async Task<BaseResponseDto<IEnumerable<ReliefPackageDto>>> GetReliefPackagesByCauseIdAsync(Guid causeId)
        {
            try
            {
                var packages = await _repository.GetListAsyncUntracked<ReliefPackage>(filter: f => f.CauseId == causeId);
                var dtos = _converter.ToListDTO(packages);

                return new BaseResponseDto<IEnumerable<ReliefPackageDto>>
                {
                    Status = 200,
                    Message = "Success",
                    ResponseData = dtos
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<IEnumerable<ReliefPackageDto>>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }

        public async Task<BaseResponseDto<bool>> DeleteReliefPackageAsync(Guid packageId)
        {
            try
            {
                var package = await _repository.GetOneAsync(filter: p => p.Id == packageId);
                if (package == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Relief package not found",
                        ResponseData = false
                    };
                }

                var items = await _reliefPackageItemRepository.GetListAsyncUntracked<ReliefPackageItem>(
                    filter: i => i.ReliefPackageId == packageId);

                foreach (var item in items)
                {
                    await _reliefPackageItemRepository.DeleteAsync(item);
                }

                await _repository.DeleteAsync(package);

                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Relief package deleted successfully",
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

        public async Task<BaseResponseDto<string>> ChangeImageAsync(Guid guid, IFormFile file)
        {
            try
            {
                var package = await _repository.GetByIdAsync(guid);

                if (package == null)
                {
                    return new BaseResponseDto<string>
                    {
                        Status = 404,
                        Message = "Gói cứu trợ không tồn tại",
                        ResponseData = null
                    };
                }

                var fileUrl = await _r2StorageService.UploadFileAsync(file, "relief-packages", guid.ToString());

                package.Image = fileUrl;
                await _repository.UpdateAsync(package);

                return new BaseResponseDto<string>
                {
                    Status = 200,
                    Message = "Tải ảnh đại diện thành công",
                    ResponseData = fileUrl + "?v=" + DateTime.Now.Microsecond.ToString()
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<string>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }
    }
}
