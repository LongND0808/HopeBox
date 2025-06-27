using System.ComponentModel.DataAnnotations;

namespace HopeBox.Domain.RequestDto
{
    public class CreateInkindDonationRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid EventId { get; set; }

        [Required, MaxLength(2000)]
        public string InKind { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string SenderAddress { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Message { get; set; }

        public bool IsAnonymous { get; set; } = false;

        public DateTime? EstimatedDeliveryDate { get; set; }
    }
}
