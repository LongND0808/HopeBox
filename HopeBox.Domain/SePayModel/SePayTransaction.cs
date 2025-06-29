using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.SePayModel
{
    public class SePayTransaction
    {
        public string id { get; set; }
        public string bank_brand_name { get; set; }
        public string account_number { get; set; }
        public string transaction_date { get; set; }
        public string amount_in { get; set; }
        public string amount_out { get; set; }
        public string accumulated { get; set; }
        public string transaction_content { get; set; }
        public string reference_number { get; set; }
        public string code { get; set; }
        public string sub_account { get; set; }
        public string bank_account_id { get; set; }
    }
}
