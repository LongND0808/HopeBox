using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopeBox.Domain.ResponseDto
{
    public class TopDonorResponseDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public decimal TotalAmount { get; set; }
        public int DonationCount { get; set; }
    }
}
