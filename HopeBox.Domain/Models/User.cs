using Microsoft.AspNetCore.Identity;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Models
{
    public class User : IdentityUser<Guid>
    {
        public int Point { get; set; }

        public string? FullName { get; set; }

        public string? Address { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public string? AvatarUrl { get; set; }

        public UserStatus UserStatus { get; set; }

        public Guid? OrganizationId { get; set; }

        public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }
        public virtual ICollection<ConfirmEmail>? ConfirmEmails { get; set; }
        public virtual Organization? Organization { get; set; }
    }
}
