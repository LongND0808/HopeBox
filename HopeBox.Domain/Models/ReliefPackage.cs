using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HopeBox.Domain.Models
{
    public class ReliefPackage : BaseModel
    {
        public Guid CauseId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal ExtraFee { get; set; }

        public decimal TotalPrice { get; set; }

        public string? Image { get; set; }

        public int CurrentQuantity { get; set; }

        public int TargetQuantity { get; set; }

        public virtual Cause? Cause { get; set; }
        public virtual ICollection<ReliefPackageItem>? PackageItems { get; set; }
    }
}
