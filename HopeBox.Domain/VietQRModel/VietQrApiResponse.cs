using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.VietQRModel
{
    public class VietQrApiResponse
    {
        public VietQrData data { get; set; }
        public string code { get; set; }
        public string? desc { get; set; }
    }
}
