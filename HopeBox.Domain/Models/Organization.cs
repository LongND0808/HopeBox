using System;
using System.ComponentModel.DataAnnotations;

namespace HopeBox.Domain.Models
{
    public class Organization : BaseModel
    {
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }
        public string? Website { get; set; }
        public bool Verified { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual IEnumerable<User>? User { get; set; }
    }
}
