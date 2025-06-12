using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.ResponseDto
{
    public class CauseRevenueResponseDto
    {
        public int Year { get; set; }
        public IEnumerable<decimal> MonthlyRevenue { get; set; }
    }
}
