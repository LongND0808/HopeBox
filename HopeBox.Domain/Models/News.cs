using System;
using System.ComponentModel.DataAnnotations;

namespace HopeBox.Domain.Models
{
    public class News : BaseModel
    {
        public required string Title { get; set; }

        [MaxLength(5000)]
        public required string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid CreatedBy { get; set; } 

        public virtual User? Creator { get; set; }
        public virtual Organization? Organization { get; set; }
    }
}
