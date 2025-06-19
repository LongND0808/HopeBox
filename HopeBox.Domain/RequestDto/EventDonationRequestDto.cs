using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.RequestDto
{
    public class EventDonationRequestDto
    {
        public Guid EventId { get; set; }
        public bool IsPackageDonation { get; set; }
        public decimal? CashAmount { get; set; }
        public List<PackageDonationItem>? Packages { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.VNPay;
        public bool IsAnonymous { get; set; } = false;
        public string? Message { get; set; }
    }

    public class PackageDonationItem
    {
        public Guid ReliefPackageId { get; set; }
        public int Quantity { get; set; }
    }
}
