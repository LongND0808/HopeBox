﻿using System;
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

        public Guid? CampaignId { get; set; }

        public virtual Campaign? Campaign { get; set; }
    }

}
