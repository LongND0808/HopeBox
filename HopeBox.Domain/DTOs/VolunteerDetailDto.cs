using HopeBox.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.DTOs
{
    public class VolunteerDetailDto
    {
        public VolunteerDto Volunteer { get; set; }
        public UserDto User { get; set; }
        public CauseDto Cause { get; set; }
    }
}
