using HopeBox.Domain.Dtos;
using HopeBox.Domain.DTOs;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Core.IService
{
    public interface IBlogService
    {
        Task<BaseResponseDto<BasePagingResponseDto<BlogDto>>> GetBlogsByFilterAsync(BlogFilterRequestDto request);
        Task<BaseResponseDto<BlogWithDetailsDto>> GetBlogDetailAsync(BlogDetailRequestDto request);
        Task<BaseResponseDto<bool>> LikeBlogAsync(Guid blogId, Guid userId);
        Task<BaseResponseDto<bool>> UnlikeBlogAsync(Guid blogId, Guid userId);
        Task<BaseResponseDto<ShareDto>> ShareBlogAsync(Guid blogId, Guid userId, SharePlatform platform, string? caption = null);
        Task<BaseResponseDto<CommentDto>> AddCommentAsync(Guid blogId, Guid userId, string content, Guid? parentCommentId = null);
        Task<BaseResponseDto<List<CommentDto>>> GetBlogCommentsAsync(Guid blogId, int pageIndex = 1, int pageSize = 6);
        Task<BaseResponseDto<BlogDto>> IncrementViewCountAsync(Guid blogId);
    }
}
