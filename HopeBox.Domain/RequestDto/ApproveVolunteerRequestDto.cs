using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.RequestDto
{
    public class ApproveVolunteerRequestDto
    {
        public Guid VolunteerId { get; set; }
        public Guid CauseId { get; set; }
    }
}
