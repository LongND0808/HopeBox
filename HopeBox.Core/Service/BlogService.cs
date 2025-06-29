using HopeBox.Core.IService;
using HopeBox.Core.R2Storage;
using HopeBox.Domain.Converter;
using HopeBox.Domain.Dtos;
using HopeBox.Domain.DTOs;
using HopeBox.Domain.Models;
using HopeBox.Domain.RequestDto;
using HopeBox.Domain.ResponseDto;
using HopeBox.Infrastructure.Repository;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Core.Service
{
    public class BlogService : IBlogService
    {
        protected readonly IRepository<Blog> _repository;
        protected readonly IRepository<User> _userRepository;
        protected readonly IRepository<Like> _likeRepository;
        protected readonly IRepository<Share> _shareRepository;
        protected readonly IRepository<Comment> _commentRepository;
        protected readonly IConverter<Blog, BlogDto> _converter;
        protected readonly IConverter<User, UserDto> _userConverter;
        protected readonly IConverter<Comment, CommentDto> _commentConverter;
        protected readonly IConverter<Share, ShareDto> _shareConverter;
        protected readonly IR2StorageService _r2StorageService;
        public BlogService(
            IRepository<Blog> repository,
            IRepository<User> userRepository,
            IRepository<Like> likeRepository,
            IRepository<Share> shareRepository,
            IRepository<Comment> commentRepository,
            IConverter<Blog, BlogDto> converter,
            IConverter<User, UserDto> userConverter,
            IConverter<Comment, CommentDto> commentConverter,
            IConverter<Share, ShareDto> shareConverter,
            IR2StorageService r2StorageService)
        {
            _repository = repository;
            _userRepository = userRepository;
            _likeRepository = likeRepository;
            _shareRepository = shareRepository;
            _commentRepository = commentRepository;
            _converter = converter;
            _userConverter = userConverter;
            _commentConverter = commentConverter;
            _shareConverter = shareConverter;
            _r2StorageService = r2StorageService;
        }

        public async Task<BaseResponseDto<BasePagingResponseDto<BlogDto>>> GetBlogsByFilterAsync(BlogFilterRequestDto request)
        {
            try
            {
                Expression<Func<Blog, bool>> filter = PredicateBuilder.New<Blog>(true);

                if (!string.IsNullOrWhiteSpace(request.Title))
                {
                    filter = filter.And(b => EF.Functions.Like(b.Title, $"%{request.Title}%"));
                }

                if (!string.IsNullOrWhiteSpace(request.Tags))
                {
                    filter = filter.And(b => EF.Functions.Like(b.Tags, $"%{request.Tags}%"));
                }

                if (request.IsPublished.HasValue)
                {
                    filter = filter.And(b => b.IsPublished == request.IsPublished.Value);
                }

                if (request.CreatedBy.HasValue)
                {
                    filter = filter.And(b => b.CreatedBy == request.CreatedBy.Value);
                }

                if (request.FromDate.HasValue)
                {
                    filter = filter.And(b => b.CreatedAt >= request.FromDate.Value);
                }

                if (request.ToDate.HasValue)
                {
                    filter = filter.And(b => b.CreatedAt <= request.ToDate.Value);
                }

                var totalRecords = await _repository.GetCount(filter);
                var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);

                var blogsQuery = await _repository.GetListAsyncUntracked<Blog>(
                    filter: filter,
                    orderBy: o => o.OrderByDescending(b => b.CreatedAt),
                    pageSize: request.PageSize,
                    pageNumber: request.PageIndex
                );

                var blogDtos = new List<BlogDto>();

                foreach (var blog in blogsQuery)
                {
                    var blogDto = _converter.ToDTO(blog);

                    var creator = await _userRepository.GetByIdAsync(blog.CreatedBy);
                    if (creator != null)
                    {
                        blogDto.CreatorName = creator.FullName ?? creator.UserName;
                    }

                    blogDtos.Add(blogDto);
                }

                var pagingResponse = new BasePagingResponseDto<BlogDto>
                {
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    PagedData = blogDtos
                };

                return new BaseResponseDto<BasePagingResponseDto<BlogDto>>
                {
                    Status = 200,
                    Message = "Lấy danh sách blog thành công",
                    ResponseData = pagingResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<BasePagingResponseDto<BlogDto>>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }

        public async Task<BaseResponseDto<BlogWithDetailsDto>> GetBlogDetailAsync(BlogDetailRequestDto request)
        {
            try
            {
                var blog = await _repository.GetByIdAsync(request.BlogId);
                if (blog == null)
                {
                    return new BaseResponseDto<BlogWithDetailsDto>
                    {
                        Status = 404,
                        Message = "Không tìm thấy blog",
                        ResponseData = null
                    };
                }

                var blogDto = _converter.ToDTO(blog);
                var blogWithDetails = new BlogWithDetailsDto
                {
                    Id = blogDto.Id,
                    Title = blogDto.Title,
                    Content = blogDto.Content,
                    Description = blogDto.Description,
                    ImageUrl = blogDto.ImageUrl,
                    Slug = blogDto.Slug,
                    ViewCount = blogDto.ViewCount,
                    LikeCount = blogDto.LikeCount,
                    CommentCount = blogDto.CommentCount,
                    ShareCount = blogDto.ShareCount,
                    Tags = blogDto.Tags,
                    MetaDescription = blogDto.MetaDescription,
                    MetaKeywords = blogDto.MetaKeywords,
                    ReadingTime = blogDto.ReadingTime,
                    CreatedAt = blogDto.CreatedAt,
                    UpdatedAt = blogDto.UpdatedAt,
                    CreatedBy = blogDto.CreatedBy,
                    IsPublished = blogDto.IsPublished
                };

                var creator = await _userRepository.GetByIdAsync(blog.CreatedBy);
                if (creator != null)
                {
                    blogWithDetails.Creator = _userConverter.ToDTO(creator);
                }

                if (request.UserId.HasValue)
                {
                    var existingLike = await _likeRepository.GetOneAsyncUntracked<Like>(
                        filter: l => l.UserId == request.UserId.Value &&
                                   l.TargetType == LikeType.Blog &&
                                   l.TargetId == request.BlogId
                    );
                    blogWithDetails.IsLikedByCurrentUser = existingLike != null;

                    var existingShare = await _shareRepository.GetOneAsyncUntracked<Share>(
                        filter: s => s.UserId == request.UserId.Value && s.BlogId == request.BlogId
                    );
                    blogWithDetails.IsSharedByCurrentUser = existingShare != null;
                }

                var comments = await _commentRepository.GetListAsyncUntracked<Comment>(
                    filter: c => c.BlogId == request.BlogId && !c.IsDeleted && c.ParentCommentId == null,
                    orderBy: o => o.OrderByDescending(c => c.CreatedAt),
                    pageSize: 5,
                    pageNumber: 1
                );

                blogWithDetails.Comments = _commentConverter.ToListDTO(comments).ToList();

                return new BaseResponseDto<BlogWithDetailsDto>
                {
                    Status = 200,
                    Message = "Lấy chi tiết blog thành công",
                    ResponseData = blogWithDetails
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<BlogWithDetailsDto>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }

        public async Task<BaseResponseDto<bool>> LikeBlogAsync(Guid blogId, Guid userId)
        {
            try
            {
                var blog = await _repository.GetByIdAsync(blogId);
                if (blog == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Không tìm thấy blog",
                        ResponseData = false
                    };
                }

                var existingLike = await _likeRepository.GetOneAsyncUntracked<Like>(
                    filter: l => l.UserId == userId && l.TargetType == LikeType.Blog && l.TargetId == blogId
                );

                if (existingLike != null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 400,
                        Message = "Bạn đã like blog này rồi",
                        ResponseData = false
                    };
                }

                var newLike = new Like
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    TargetType = LikeType.Blog,
                    TargetId = blogId,
                    CreatedAt = DateTime.UtcNow
                };

                await _likeRepository.AddAsync(newLike);

                blog.LikeCount++;
                await _repository.UpdateAsync(blog);

                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Like blog thành công",
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

        public async Task<BaseResponseDto<bool>> UnlikeBlogAsync(Guid blogId, Guid userId)
        {
            using var transaction = await _repository.BeginTransactionAsync();
            try
            {
                var blog = await _repository.GetByIdAsync(blogId);
                if (blog == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Không tìm thấy blog",
                        ResponseData = false
                    };
                }

                var existingLike = await _likeRepository.GetOneAsync(
                    filter: l => l.UserId == userId && l.TargetType == LikeType.Blog && l.TargetId == blogId
                );

                if (existingLike == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 400,
                        Message = "Bạn chưa like blog này",
                        ResponseData = false
                    };
                }

                await _likeRepository.DeleteAsync(existingLike);

                blog.LikeCount = Math.Max(0, blog.LikeCount - 1);
                await _repository.UpdateAsync(blog);

                await _repository.CommitTransactionAsync(transaction);

                return new BaseResponseDto<bool>
                {
                    Status = 200,
                    Message = "Unlike blog thành công",
                    ResponseData = true
                };
            }
            catch (Exception ex)
            {
                await _repository.RollbackTransactionAsync(transaction);
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = false
                };
            }
        }

        public async Task<BaseResponseDto<ShareDto>> ShareBlogAsync(Guid blogId, Guid userId, SharePlatform platform, string? caption = null)
        {
            try
            {
                var blog = await _repository.GetByIdAsync(blogId);
                if (blog == null)
                {
                    return new BaseResponseDto<ShareDto>
                    {
                        Status = 404,
                        Message = "Không tìm thấy blog",
                        ResponseData = null
                    };
                }

                var shareUrl = $"https://hopebox.long2003-2014.workers.dev/blogs/{blog.Slug}";

                var newShare = new Share
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    BlogId = blogId,
                    Platform = platform,
                    ShareUrl = shareUrl,
                    ShareCaption = caption ?? $"Chia sẻ bài viết: {blog.Title}",
                    CreatedAt = DateTime.UtcNow
                };

                await _shareRepository.AddAsync(newShare);

                blog.ShareCount++;
                await _repository.UpdateAsync(blog);

                var shareDto = _shareConverter.ToDTO(newShare);

                return new BaseResponseDto<ShareDto>
                {
                    Status = 200,
                    Message = "Chia sẻ blog thành công",
                    ResponseData = shareDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<ShareDto>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }

        public async Task<BaseResponseDto<CommentDto>> AddCommentAsync(Guid blogId, Guid userId, string content, Guid? parentCommentId = null)
        {
            try
            {
                var blog = await _repository.GetByIdAsync(blogId);
                if (blog == null)
                {
                    return new BaseResponseDto<CommentDto>
                    {
                        Status = 404,
                        Message = "Không tìm thấy blog",
                        ResponseData = null
                    };
                }

                int level = 0;
                if (parentCommentId.HasValue)
                {
                    var parentComment = await _commentRepository.GetByIdAsync(parentCommentId.Value);
                    if (parentComment == null)
                    {
                        return new BaseResponseDto<CommentDto>
                        {
                            Status = 404,
                            Message = "Không tìm thấy comment cha",
                            ResponseData = null
                        };
                    }
                    level = parentComment.Level + 1;
                }

                var newComment = new Comment
                {
                    Id = Guid.NewGuid(),
                    BlogId = blogId,
                    UserId = userId,
                    ParentCommentId = parentCommentId,
                    Content = content,
                    Level = level,
                    CreatedAt = DateTime.UtcNow
                };

                await _commentRepository.AddAsync(newComment);

                blog.CommentCount++;
                await _repository.UpdateAsync(blog);

                if (parentCommentId.HasValue)
                {
                    var parentComment = await _commentRepository.GetByIdAsync(parentCommentId.Value);
                    if (parentComment != null)
                    {
                        parentComment.ReplyCount++;
                        await _commentRepository.UpdateAsync(parentComment);
                    }
                }

                var commentDto = _commentConverter.ToDTO(newComment);

                return new BaseResponseDto<CommentDto>
                {
                    Status = 200,
                    Message = "Thêm comment thành công",
                    ResponseData = commentDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<CommentDto>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }

        public async Task<BaseResponseDto<List<CommentDto>>> GetBlogCommentsAsync(Guid blogId, int pageIndex = 1, int pageSize = 6)
        {
            try
            {
                var comments = await _commentRepository.GetListAsyncUntracked<Comment>(
                    filter: c => c.BlogId == blogId && !c.IsDeleted,
                    orderBy: o => o.OrderByDescending(c => c.CreatedAt),
                    pageSize: pageSize,
                    pageNumber: pageIndex
                );

                var commentDtos = _commentConverter.ToListDTO(comments).ToList();

                return new BaseResponseDto<List<CommentDto>>
                {
                    Status = 200,
                    Message = "Lấy danh sách comment thành công",
                    ResponseData = commentDtos
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<List<CommentDto>>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }

        public async Task<BaseResponseDto<BlogDto>> IncrementViewCountAsync(Guid blogId)
        {
            try
            {
                var blog = await _repository.GetByIdAsync(blogId);
                if (blog == null)
                {
                    return new BaseResponseDto<BlogDto>
                    {
                        Status = 404,
                        Message = "Không tìm thấy blog",
                        ResponseData = null
                    };
                }

                blog.ViewCount++;
                await _repository.UpdateAsync(blog);

                var blogDto = _converter.ToDTO(blog);

                return new BaseResponseDto<BlogDto>
                {
                    Status = 200,
                    Message = "Cập nhật lượt xem thành công",
                    ResponseData = blogDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<BlogDto>
                {
                    Status = 500,
                    Message = ex.Message,
                    ResponseData = null
                };
            }
        }

        public async Task<BaseResponseDto<string>> ChangeImageAsync(Guid blogId, IFormFile file)
        {
            try
            {
                var blog = await _repository.GetByIdAsync(blogId);
                if (blog == null)
                {
                    return new BaseResponseDto<string>
                    {
                        Status = 404,
                        Message = "Không tìm thấy blog",
                        ResponseData = null
                    };
                }

                var fileUrl = await _r2StorageService.UploadFileAsync(file, "blog-images", blogId.ToString());
                blog.ImageUrl = fileUrl;
                await _repository.UpdateAsync(blog);

                return new BaseResponseDto<string>
                {
                    Status = 200,
                    Message = "Tải ảnh blog thành công",
                    ResponseData = fileUrl + "?v=" + DateTime.Now.Ticks.ToString()
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
