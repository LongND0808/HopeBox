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
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public int Status { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid OrganizationId { get; set; }
    }
}
