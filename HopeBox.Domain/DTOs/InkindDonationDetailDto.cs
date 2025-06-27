namespace HopeBox.Domain.DTOs
{
    public class InkindDonationDetailDto : InkindDonationDto
    {
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? EventTitle { get; set; }
        public string? EventLocation { get; set; }
        public string? ApproverName { get; set; }
        public string? StatusDescription { get; set; }
        public int DaysUntilDelivery { get; set; }
    }
}
