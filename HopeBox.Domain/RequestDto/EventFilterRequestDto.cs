namespace HopeBox.Domain.RequestDto
{
    public class EventFilterRequestDto
    {
        public string? Title { get; set; }
        public string? CauseTitle { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 3;
    }
}
