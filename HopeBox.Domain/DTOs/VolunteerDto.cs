using System;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Dtos
{
    public class VolunteerDto
    {
        public Guid UserId { get; set; }
        public Guid CauseId { get; set; }
        public DateTime JoinDate { get; set; }
        public VolunteerStatus Status { get; set; }
    }
}