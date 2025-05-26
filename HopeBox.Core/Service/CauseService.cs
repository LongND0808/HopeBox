using HopeBox.Core.IService;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace HopeBox.Core.Service
{
    public class CauseService : BaseService<Cause, CauseDto>, ICauseService
    {
        public CauseService(IRepository<Cause> repository, IConverter<Cause, CauseDto> converter)
            : base(repository, converter)
        {
        }

        public async Task<BaseResponseDto<BasePagingResponseDto<CauseDto>>> GetCauseByFilter(
            CauseFilterRequestDto request)
        {
            try
            {
                Expression<Func<Cause, bool>> filter = PredicateBuilder.New<Cause>(true);

                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    filter = filter.And(f => EF.Functions.Like(f.Title, $"%{request.Name}%"));
                }

                if (request.CauseType.HasValue)
                {
                    filter = filter.And(f => (int) f.Type == request.CauseType.Value);
                }

                var totalRecords = await _repository.GetCount(filter);

                var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);

                var query = await _repository.GetListAsyncUntracked<Cause>(
                    filter: filter,
                    pageSize: request.PageSize,
                    pageNumber: request.PageIndex
                );

                var dtos = _converter.ToListDTO(query);

                var pagingResponse = new BasePagingResponseDto<CauseDto>
                {
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    PagedData = dtos
                };

                return new BaseResponseDto<BasePagingResponseDto<CauseDto>>
                {
                    Status = 200,
                    Message = "Lấy danh sách thành công",
                    ResponseData = pagingResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<BasePagingResponseDto<CauseDto>>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }


        public async Task<BaseResponseDto<CauseDto>> GetCauseHighestTargetAsync()
        {
            try
            {
                var entity = await base._repository.GetOneAsyncUntracked<Cause>(
                    orderBy: o => o.OrderByDescending(od => od.TargetAmount)
                );
                if (entity == null)
                {
                    return new BaseResponseDto<CauseDto>
                    {
                        Status = 404,
                        Message = "Entity not found.",
                        ResponseData = null
                    };
                }
                var dto = _converter.ToDTO(entity);
                return new BaseResponseDto<CauseDto> { Status = 200, Message = "Success", ResponseData = dto };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<CauseDto> { Status = 500, Message = ex.Message, ResponseData = null };
            }
        }

        public async Task<BaseResponseDto<IEnumerable<CauseDto>>> GetCauseOneAsync()
        {
            try
            {
                var entities = await base._repository.GetListAsyncUntracked<Cause>(
                    orderBy: o => o.OrderByDescending(od => od.TargetAmount),
                    pageSize: 3,
                    pageNumber: 1
                );
                var dtos = _converter.ToListDTO(entities);
                return new BaseResponseDto<IEnumerable<CauseDto>> { Status = 200, Message = "Success", ResponseData = dtos };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<IEnumerable<CauseDto>> { Status = 500, Message = ex.Message, ResponseData = null };
            }
        }
    }
}
