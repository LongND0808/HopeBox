using System;

namespace HopeBox.Domain.Dtos
{
    public class ReliefPackageDto : BaseModelDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}