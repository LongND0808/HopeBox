using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Models
{
    public class ReliefItem : BaseModel
    {
        [Required]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        public string Unit { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        public virtual ICollection<ReliefPackageItem>? PackageItems { get; set; }
    }

}
