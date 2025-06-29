using HopeBox.Domain.Dtos;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.ResponseDto
{
    public class CauseWithVolunteerStatusDto : CauseDto
    {
        public bool IsVolunteerRegistered { get; set; }
        public VolunteerStatus? VolunteerStatus { get; set; }
        public DateTime? VolunteerJoinDate { get; set; }
    }
}
