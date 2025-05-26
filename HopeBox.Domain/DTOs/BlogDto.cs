using System;

namespace HopeBox.Domain.Dtos
{
    public class BlogDto : BaseModelDto
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public bool IsPublished { get; set; } = false;
    }
}