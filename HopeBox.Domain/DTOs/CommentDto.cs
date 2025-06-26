using HopeBox.Domain.Dtos;

namespace HopeBox.Domain.DTOs
{
    public class CommentDto : BaseModelDto
    {
        public Guid BlogId { get; set; }

        public Guid UserId { get; set; }

        public Guid? ParentCommentId { get; set; }

        public required string Content { get; set; }

        public int LikeCount { get; set; }

        public int ReplyCount { get; set; }

        public int Level { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
