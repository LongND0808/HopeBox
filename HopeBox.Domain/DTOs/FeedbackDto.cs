using System;

namespace HopeBox.Domain.Dtos
{
    public class FeedbackDto : BaseModelDto
    {
        public Guid UserId { get; set; }
        public Guid? CauseId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}