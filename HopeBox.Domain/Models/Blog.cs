using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopeBox.Domain.Models
{
    public class Blog : BaseModel
    {
        [Required]
        public required string Title { get; set; }

        [MaxLength(5000)]
        [Required]
        public required string Content { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid CreatedBy { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual User? Creator { get; set; }

        public bool IsPublished { get; set; } = false;
    }
}
