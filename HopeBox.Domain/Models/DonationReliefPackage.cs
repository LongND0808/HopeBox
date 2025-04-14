using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.Models
{
    public class DonationReliefPackage : BaseModel
    {
        public Guid DonationId { get; set; }

        public Guid ReliefPackageId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [NotMapped]
        public decimal TotalAmount => ReliefPackage != null ? ReliefPackage.TotalAmount * Quantity : 0;

        public virtual Donation? Donation { get; set; }
        public virtual ReliefPackage? ReliefPackage { get; set; }
    }

}
