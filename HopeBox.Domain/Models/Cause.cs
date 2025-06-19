using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Models
{
    public class Cause : BaseModel
    {
        [Required, MaxLength(200)]
        public string? Title { get; set; }

        [Required, MaxLength(500)]
        public string? Description { get; set; }

        [Required, MaxLength(5000)]
        public string? Detail { get; set; }

        public string? HeroImage { get; set; }

        [Required, MaxLength(5000)]
        public string? Challenge { get; set; }

        public string? ChallengeImage { get; set; }

        [Required, MaxLength(5000)]
        public string? Summary { get; set; }

        public string? SummaryImage { get; set; }

        public CauseType Type { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal TargetAmount { get; set; }

        public decimal CurrentAmount { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CauseStatus Status { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid OrganizationId { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual User? Creator { get; set; }
        public virtual Organization? Organization { get; set; }
        public virtual ICollection<ReliefPackage>? ReliefPackages { get; set; }
        public virtual ICollection<Donation>? Donations { get; set; }
        public virtual ICollection<Volunteer>? Volunteers { get; set; }
        public virtual ICollection<Event>? Events { get; set; }
        public virtual ICollection<Media>? Documents { get; set; }
    }
}
