using System;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Dtos
{
    public class ReliefItemDto : BaseModelDto
    {
        public string ItemName { get; set; } = string.Empty;
        public Unit Unit { get; set; }
        public decimal UnitPrice { get; set; }
    }
}