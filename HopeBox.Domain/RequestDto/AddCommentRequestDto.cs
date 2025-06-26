namespace HopeBox.Domain.RequestDto
{
    public class AddCommentRequestDto
    {
        public Guid UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid? ParentCommentId { get; set; }
    }
}
