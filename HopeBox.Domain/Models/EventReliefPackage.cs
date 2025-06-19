using System.ComponentModel.DataAnnotations;

namespace HopeBox.Domain.Models
{
    public class EventReliefPackage : BaseModel
    {
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public Guid ReliefPackageId { get; set; }
        [Required]
        public Guid DonationId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [MaxLength(500)]
        public string? SenderAddress { get; set; }
        [MaxLength(500)]
        public string? ReceiverAddress { get; set; }
        public DateTime DonationDate { get; set; }     
        
        public virtual Event? Event { get; set; }        
        public virtual ReliefPackage? ReliefPackage { get; set; }
        public virtual Donation? Donation { get; set; }
    }
}
