using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.Models
{
    public class ReliefPackageItem : BaseModel
    {
        public Guid ReliefPackageId { get; set; }

        public Guid ReliefItemId { get; set; }

        [Range(1, double.MaxValue)]
        public double Quantity { get; set; }

        public virtual ReliefPackage? ReliefPackage { get; set; }
        public virtual ReliefItem? ReliefItem { get; set; }
    }

}
