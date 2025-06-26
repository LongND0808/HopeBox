namespace HopeBox.Domain.RequestDto
{
    public class BlogFilterRequestDto
    {
        public string? Title { get; set; }
        public string? Tags { get; set; }
        public bool? IsPublished { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }
}
