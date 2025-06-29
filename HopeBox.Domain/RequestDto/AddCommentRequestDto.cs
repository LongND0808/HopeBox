namespace HopeBox.Domain.RequestDto
{
    public class AddCommentRequestDto
    {
        public string Content { get; set; } = string.Empty;
        public Guid? ParentCommentId { get; set; }
    }
}
