using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.ResponseDto
{
    public class RefreshTokenResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
