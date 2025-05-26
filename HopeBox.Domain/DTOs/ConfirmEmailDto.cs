using System;

namespace HopeBox.Domain.Dtos
{
    public class ConfirmEmailDto : BaseModelDto
    {
        public Guid UserId { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string? ConfirmCode { get; set; }
        public bool IsConfirmed { get; set; }
    }
}