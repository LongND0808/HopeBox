using System.ComponentModel.DataAnnotations;

namespace HopeBox.Domain.Models
{
    public class Comment : BaseModel
    {
        public Guid BlogId { get; set; }

        public Guid UserId { get; set; }

        public Guid? ParentCommentId { get; set; }

        [Required, MaxLength(2000)]
        public required string Content { get; set; }

        public int LikeCount { get; set; } = 0;

        public int ReplyCount { get; set; } = 0;

        public int Level { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;

        public virtual Blog? Blog { get; set; }
        public virtual User? User { get; set; }
        public virtual Comment? ParentComment { get; set; }
        public virtual ICollection<Comment>? Replies { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
    }
}
