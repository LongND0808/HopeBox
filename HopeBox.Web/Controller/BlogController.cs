using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using HopeBox.Domain.Dtos;
using HopeBox.Core.IService;
using HopeBox.Domain.RequestDto;

namespace HopeBox.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : BaseController<Blog, BlogDto>
    {
        private readonly IBlogService _blogService;

        public BlogController(IBaseService<Blog, BlogDto> service, IBlogService blogService) : base(service)
        {
            _blogService = blogService;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetBlogsByFilter([FromQuery] BlogFilterRequestDto request)
        {
            var result = await _blogService.GetBlogsByFilterAsync(request);
            return StatusCode(result.Status, result);
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetBlogDetail([FromQuery] BlogDetailRequestDto request)
        {
            var result = await _blogService.GetBlogDetailAsync(request);
            return StatusCode(result.Status, result);
        }

        [HttpPost("{blogId}/like")]
        public async Task<IActionResult> LikeBlog(Guid blogId, [FromBody] Guid userId)
        {
            var result = await _blogService.LikeBlogAsync(blogId, userId);
            return StatusCode(result.Status, result);
        }

        [HttpDelete("{blogId}/like")]
        public async Task<IActionResult> UnlikeBlog(Guid blogId, [FromBody] Guid userId)
        {
            var result = await _blogService.UnlikeBlogAsync(blogId, userId);
            return StatusCode(result.Status, result);
        }

        [HttpPost("{blogId}/share")]
        public async Task<IActionResult> ShareBlog(Guid blogId, [FromBody] ShareBlogRequestDto request)
        {
            var result = await _blogService.ShareBlogAsync(blogId, request.UserId, request.Platform, request.Caption);
            return StatusCode(result.Status, result);
        }

        [HttpPost("{blogId}/comment")]
        public async Task<IActionResult> AddComment(Guid blogId, [FromBody] AddCommentRequestDto request)
        {
            var result = await _blogService.AddCommentAsync(blogId, request.UserId, request.Content, request.ParentCommentId);
            return StatusCode(result.Status, result);
        }

        [HttpGet("{blogId}/comments")]
        public async Task<IActionResult> GetBlogComments(Guid blogId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _blogService.GetBlogCommentsAsync(blogId, pageIndex, pageSize);
            return StatusCode(result.Status, result);
        }

        [HttpPut("{blogId}/view")]
        public async Task<IActionResult> IncrementViewCount(Guid blogId)
        {
            var result = await _blogService.IncrementViewCountAsync(blogId);
            return StatusCode(result.Status, result);
        }
    }
}



