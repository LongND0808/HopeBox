using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HopeBox.Domain.Models
{
    public class ReliefPackage : BaseModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [NotMapped]
        public decimal TotalAmount => PackageItems?.Sum(i => i.TotalPrice) ?? 0;

        public virtual ICollection<ReliefPackageItem>? PackageItems { get; set; }
    }
}
