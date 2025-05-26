using System;

namespace HopeBox.Domain.Dtos
{
    public class RefreshTokenDto : BaseModelDto
    {
        public string? Token { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ExpiredTime { get; set; }
        public Guid UserId { get; set; }
    }
}