using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.SePayModel
{
    public class SePayQRResponse
    {
        public string ImageUrl { get; set; }
        public string BankName { get; set; }
        public string TradingCode { get; set; }
        public string AccountName { get; set; }
    }
}
