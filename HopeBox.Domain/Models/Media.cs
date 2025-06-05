using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Models
{
    public class Media : BaseModel
    {
        public string? Url { get; set; }

        public MediaType Type { get; set; } 

        public Guid? CauseId { get; set; }

        public virtual Cause? Cause { get; set; }
    }

}
