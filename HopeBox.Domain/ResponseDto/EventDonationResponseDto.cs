namespace HopeBox.Domain.ResponseDto
{
    public class EventDonationResponseDto
    {
        public Guid DonationId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentUrl { get; set; } = string.Empty;
        public bool IsPackageDonation { get; set; }
        public List<DonatedPackageInfo>? PackageDetails { get; set; }
        public DeliveryInfo? DeliveryInfo { get; set; }
    }

    public class DonatedPackageInfo
    {
        public string PackageName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class DeliveryInfo
    {
        public string SenderAddress { get; set; } = string.Empty;
        public string ReceiverAddress { get; set; } = string.Empty;
        public List<Guid> EventReliefPackageIds { get; set; } = new();
    }
}
