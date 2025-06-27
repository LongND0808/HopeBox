using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.Models
{
    public class InKindDonation : BaseModel
    {
        public Guid UserId { get; set; }

        public Guid EventId { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public string SenderName { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public virtual User? User { get; set; }
        public virtual Event? Event { get; set; }
    }
}
