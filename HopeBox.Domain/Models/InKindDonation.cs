using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Models
{
    public class InkindDonation : BaseModel
    {
        public Guid UserId { get; set; }

        public Guid? ApprovedBy { get; set; }

        public Guid EventId { get; set; }

        public string InKind { get; set; } = String.Empty;

        public DateTime DonationDate { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public DateTime? DeliveredDate { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public InkindDonationStatus Status { get; set; }

        public bool IsAnonymous { get; set; }

        [MaxLength(500)]
        public string? Message { get; set; }

        [Required, MaxLength(200)]
        public required string SenderAddress { get; set; }

        [Required, MaxLength(200)]
        public required string ReceiverAddress { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        [ForeignKey(nameof(ApprovedBy))]
        public virtual User? Approver { get; set; }

        public virtual Event? Event { get; set; }
    }
}
