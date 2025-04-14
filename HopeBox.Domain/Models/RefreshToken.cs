namespace HopeBox.Domain.Models
{
    public class RefreshToken : BaseModel
    {
        public string? Token { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ExpiredTime { get; set; }

        public Guid UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
