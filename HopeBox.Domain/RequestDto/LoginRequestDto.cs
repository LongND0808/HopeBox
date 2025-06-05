using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.RequestDto
{
    public class LoginRequestDto
    {
        public string LoginEmail { get; set; }
        public string Password { get; set; }
    }
}
