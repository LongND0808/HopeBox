using System;
using static HopeBox.Common.Enum.Enumerate;

namespace HopeBox.Domain.Dtos
{
    public class MediaDto : BaseModelDto
    {
        public string? Url { get; set; }
        public MediaType Type { get; set; }
        public Guid? CauseId { get; set; }
    }
}