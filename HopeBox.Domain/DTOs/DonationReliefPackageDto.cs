using System;

namespace HopeBox.Domain.Dtos
{
    public class DonationReliefPackageDto : BaseModelDto
    {
        public Guid DonationId { get; set; }
        public Guid ReliefPackageId { get; set; }
        public int Quantity { get; set; }
    }
}