using HopeBox.Domain.Dtos;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.DTOs
{
    public class InkindDonationDto : BaseModelDto
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

        public string? Message { get; set; }

        public required string SenderAddress { get; set; }

        public required string ReceiverAddress { get; set; }
    }
}
