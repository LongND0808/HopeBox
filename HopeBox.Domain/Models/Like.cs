using System.ComponentModel.DataAnnotations;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Models
{
    public class Like : BaseModel
    {
        public Guid UserId { get; set; }

        public LikeType TargetType { get; set; }

        public Guid TargetId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public virtual User? User { get; set; }
        public virtual Blog? Blog { get; set; }
        public virtual Comment? Comment { get; set; }

    }
}
