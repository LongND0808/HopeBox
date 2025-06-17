using HopeBox.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.RequestDto
{
    public class CreateDonationRequestDto
    {
        public Guid CauseId { get; set; }

        public decimal DonationAmount { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public bool IsAnonymous { get; set; }

        public string? Message { get; set; }

        public Dictionary<Guid, int> ReliefPackages { get; set; } = new();
    }
}
