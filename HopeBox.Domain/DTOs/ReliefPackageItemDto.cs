using System;

namespace HopeBox.Domain.Dtos
{
    public class ReliefPackageItemDto : BaseModelDto
    {
        public Guid ReliefPackageId { get; set; }
        public Guid ReliefItemId { get; set; }
        public double Quantity { get; set; }
    }
}