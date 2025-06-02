namespace HopeBox.Domain.RequestDto
{
    public class VolunteerRequestDto
    {
        public Guid UserId { get; set; }
        public Guid CauseId { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
