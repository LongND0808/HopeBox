using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Models
{
    public class Campaign : BaseModel
    {
        [Required, MaxLength(200)]
        public string? Title { get; set; }

        [Required, MaxLength(5000)]
        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal TargetAmount { get; set; }

        public decimal CurrentAmount { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CampaignStatus Status { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid OrganizationId { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual User? Creator { get; set; }
        public virtual Organization? Organization { get; set; }
        public virtual ICollection<Donation>? Donations { get; set; }
        public virtual ICollection<Volunteer>? Volunteers { get; set; }
    }
}
