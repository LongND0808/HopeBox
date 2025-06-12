using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.RequestDto
{
    public class ConfirmEmailRequestDto
    {
        [Required]
        public string ConfirmEmail { get; set; }
        [Required]
        public string ConfirmCode { get; set; }
    }
}
