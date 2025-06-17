using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.RequestDto
{
    public class UpdateUserInfoRequestDto
    {
        public Guid UserId { get;set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DataOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
    }
}
