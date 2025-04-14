using System;
using System.ComponentModel.DataAnnotations;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Models
{
    public class Donation : BaseModel
    {
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }

        public Guid CampaignId { get; set; }

        public decimal Amount { get; set; }

        public DateTime DonationDate { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public string? TransactionId { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual ICollection<DonationReliefPackage>? ReliefPackages { get; set; }
    }

}
