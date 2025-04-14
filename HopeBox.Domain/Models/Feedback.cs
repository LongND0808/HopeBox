using System;
using System.ComponentModel.DataAnnotations;

namespace HopeBox.Domain.Models
{
    public class Feedback : BaseModel
    {
        public Guid UserId { get; set; }

        public Guid? CampaignId { get; set; } 

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual User? User { get; set; }
        public virtual Campaign? Campaign { get; set; }
    }
}
