using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.RequestDto
{
    public class ShareBlogRequestDto
    {
        public Guid UserId { get; set; }
        public SharePlatform Platform { get; set; }
        public string? Caption { get; set; }
    }
}
