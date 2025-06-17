using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.RequestDto
{
    public class CreateReliefPackageRequestDto
    {
        public Guid CauseId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal ExtraFee { get; set; }

        public decimal TotalPrice { get; set; }

        public string? Image { get; set; }

        public int CurrentQuantity { get; set; }

        public int TargetQuantity { get; set; }
        public Dictionary<Guid,int> SelectedItems { get; set; }
    }
}
