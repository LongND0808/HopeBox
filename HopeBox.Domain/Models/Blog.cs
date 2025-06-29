using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopeBox.Domain.Models
{
    public class Blog : BaseModel
    {
        [Required]
        public required string Title { get; set; }

        [Required, MaxLength(5000)]
        public required string Content { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        public string? Slug { get; set; }

        public int ViewCount { get; set; } = 0;

        public int LikeCount { get; set; } = 0;

        public int CommentCount { get; set; } = 0;

        public int ShareCount { get; set; } = 0;

        public string? Tags { get; set; }

        public string? MetaDescription { get; set; }

        public string? MetaKeywords { get; set; }

        public int ReadingTime { get; set; } = 0;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid CreatedBy { get; set; }

        public bool IsPublished { get; set; } = false;

        [ForeignKey(nameof(CreatedBy))]
        public virtual User? Creator { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
    }
}
