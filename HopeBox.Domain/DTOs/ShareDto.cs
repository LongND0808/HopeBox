using HopeBox.Domain.Dtos;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.DTOs
{
    public class ShareDto : BaseModelDto
    {
        public Guid UserId { get; set; }

        public Guid BlogId { get; set; }

        public SharePlatform Platform { get; set; }

        public string? ShareUrl { get; set; }

        public string? ShareCaption { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
