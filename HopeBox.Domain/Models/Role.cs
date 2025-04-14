using Microsoft.AspNetCore.Identity;

namespace HopeBox.Domain.Models
{
    public class Role : IdentityRole<Guid>
    {
        public string? Code { get; set; }
    }
}
