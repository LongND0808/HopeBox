using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Models
{
    public class Volunteer : BaseModel
    {
        public Guid UserId { get; set; }

        public Guid CausesId { get; set; }

        public DateTime JoinDate { get; set; }

        public VolunteerStatus Status { get; set; }

        public virtual User? User { get; set; }
        public virtual Cause? Causes { get; set; }
    }
}
