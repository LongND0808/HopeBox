using System.ComponentModel.DataAnnotations;

namespace HopeBox.Domain.RequestDto
{
    public class UpdateLocationByPlaceRequestDto
    {
        [Required]
        public string PlaceId { get; set; } = string.Empty;
    }
}
