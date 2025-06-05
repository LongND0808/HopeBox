using System;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Dtos
{
    public class CauseDto : BaseModelDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Detail { get; set; }
        public string? HeroImage { get; set; }
        public string? Challenge { get; set; }
        public string? ChallengeImage { get; set; }
        public string? Summary { get; set; }
        public string? SummaryImage { get; set; }
        public CauseType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public CauseStatus Status { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid OrganizationId { get; set; }
    }
}