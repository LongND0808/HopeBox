using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.RequestDto
{
    public class VNPayReturnRequestDto
    {
        public string vnp_TxnRef { get; set; }
        public string vnp_ResponseCode { get; set; }
        public string vnp_TransactionNo { get; set; }
    }
}
