namespace HopeBox.Domain.RequestDto
{
    public class BlogDetailRequestDto
    {
        public Guid BlogId { get; set; }
        public Guid? UserId { get; set; }
    }
}
