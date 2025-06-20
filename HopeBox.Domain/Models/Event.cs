using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Models
{
    public class Event : BaseModel
    {
        [Required, MaxLength(200)]
        public string? Title { get; set; }

        [Required, MaxLength(2000)]
        public string? Description { get; set; }

        [Required, MaxLength(10000)]
        public string? Detail { get; set; }

        public string? BannerImage { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [MaxLength(1000)]
        public string? Location { get; set; }

        // Thêm các property tọa độ mới
        [Column(TypeName = "decimal(10,8)")]
        public double? Latitude { get; set; }

        [Column(TypeName = "decimal(11,8)")]
        public double? Longitude { get; set; }

        [MaxLength(500)]
        public string? FormattedAddress { get; set; }

        public decimal TargetAmount { get; set; }

        public decimal CurrentAmount { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventStatus Status { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid CauseId { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual User? Creator { get; set; }

        public virtual Organization? Organization { get; set; }

        public virtual ICollection<Media>? Photos { get; set; }
        public virtual Cause Cause { get; set; }
    }
}
