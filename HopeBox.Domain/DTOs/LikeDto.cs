using HopeBox.Domain.Dtos;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.DTOs
{
    internal class LikeDto : BaseModelDto
    {
        public Guid UserId { get; set; }

        public LikeType TargetType { get; set; }

        public Guid TargetId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
