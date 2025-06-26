namespace HopeBox.Domain.Dtos
{
    public class BlogDto : BaseModelDto
    {
        public required string Title { get; set; }

        public required string Content { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public string? Slug { get; set; }

        public int ViewCount { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }

        public int ShareCount { get; set; }

        public string? Tags { get; set; }

        public string? MetaDescription { get; set; }

        public string? MetaKeywords { get; set; }

        public int ReadingTime { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid CreatedBy { get; set; }

        public string? CreatorName { get; set; }

        public bool IsPublished { get; set; }
    }
}