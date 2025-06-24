using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.DTOs
{
    public class EventDonationDetailDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Detail { get; set; }
        public string? BannerImage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? FormattedAddress { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public int Status { get; set; }
        public string? OrganizationName { get; set; }
        public string? CauseTitle { get; set; }
        public string? CreatedByName { get; set; }
        public CauseType? CauseType { get; set; }
    }
}
