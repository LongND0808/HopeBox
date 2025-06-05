using HopeBox.Common.Enum;

namespace HopeBox.Domain.RequestDto
{
    public class RegisterRequestDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Enumerate.Gender Gender { get; set; }
    }
}
