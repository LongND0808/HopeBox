using System;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Dtos
{
    public class UserDto : BaseModelDto
    {
        public int Point { get; set; }
        public string? FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string? AvatarUrl { get; set; }
        public Guid UserStatusId { get; set; }
        public UserStatus UserStatus { get; set; }
    }
}