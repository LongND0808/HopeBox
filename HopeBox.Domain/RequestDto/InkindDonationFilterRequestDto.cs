using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.RequestDto
{
    public class InkindDonationFilterRequestDto
    {
        public InkindDonationStatus? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsAnonymous { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }
}
