using HopeBox.Domain.Dtos;

namespace HopeBox.Domain.DTOs
{
    public class BlogWithDetailsDto : BlogDto
    {
        public UserDto? Creator { get; set; }
        public List<CommentDto>? Comments { get; set; }
        public bool IsLikedByCurrentUser { get; set; } = false;
        public bool IsSharedByCurrentUser { get; set; } = false;
    }
}
