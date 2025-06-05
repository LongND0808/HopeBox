using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.RequestDto
{
    public class ConfirmEmailRequestDto
    {
        public string ConfirmEmail { get; set; }
        public string ConfirmCode { get; set; }
    }
}
