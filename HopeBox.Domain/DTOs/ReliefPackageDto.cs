using System;

namespace HopeBox.Domain.Dtos
{
    public class ReliefPackageDto : BaseModelDto
    {
        public Guid CauseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal TotalAmount { get; set; }

        // Thêm navigation properties
        public List<ReliefPackageItemDto> PackageItems { get; set; } = new();
    }
}