using System;

namespace HopeBox.Domain.Dtos
{
    public class ReliefItemDto : BaseModelDto
    {
        public string ItemName { get; set; } = string.Empty;
        public string Unit { get; set; } = "pcs";
        public decimal UnitPrice { get; set; }
    }
}