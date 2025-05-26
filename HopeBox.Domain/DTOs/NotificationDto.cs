using System;

namespace HopeBox.Domain.Dtos
{
    public class NotificationDto : BaseModelDto
    {
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
    }
}