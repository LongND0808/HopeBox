using System;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Dtos
{
    public class DonationDto : BaseModelDto
    {
        public Guid UserId { get; set; }
        public Guid CausesId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DonationDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
    }
}