namespace HopeBox.Domain.RequestDto
{
    public class CauseFilterWithUserRequestDto : CauseFilterRequestDto
    {
        public Guid? UserId { get; set; }
    }
}
