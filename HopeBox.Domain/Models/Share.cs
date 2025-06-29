using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Models
{
    public class Share : BaseModel
    {
        public Guid UserId { get; set; }

        public Guid BlogId { get; set; }

        public SharePlatform Platform { get; set; }

        public string? ShareUrl { get; set; }

        public string? ShareCaption { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual User? User { get; set; }
        public virtual Blog? Blog { get; set; }
    }
}
