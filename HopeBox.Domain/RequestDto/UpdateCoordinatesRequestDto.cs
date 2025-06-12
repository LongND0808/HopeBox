using System.ComponentModel.DataAnnotations;

namespace HopeBox.Domain.RequestDto
{
    public class UpdateCoordinatesRequestDto
    {
        [Required]
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public double Latitude { get; set; }

        [Required]
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public double Longitude { get; set; }
    }
}
