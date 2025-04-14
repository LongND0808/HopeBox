using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.Models
{
    public class Notification : BaseModel
    {
        public Guid UserId { get; set; }

        public string? Title { get; set; }

        public string? Message { get; set; }

        public bool IsRead { get; set; }

        public DateTime SentAt { get; set; }

        public virtual User? User { get; set; }
    }

}
