namespace HopeBox.Domain.Dtos
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Detail { get; set; }
        public string? BannerImage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Location { get; set; }

        // Thêm các trường tọa độ
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? FormattedAddress { get; set; }

        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public int Status { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid OrganizationId { get; set; }
        public string? CreatedByName { get; set; }
        public string? OrganizationName { get; set; }
    }
}
