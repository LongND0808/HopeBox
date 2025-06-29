using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.SePayModel
{
    public class SePayApiResponse
    {
        public int status { get; set; }
        public string error { get; set; }
        public SePayMessages messages { get; set; }
        public List<SePayTransaction> transactions { get; set; }
    }
}
