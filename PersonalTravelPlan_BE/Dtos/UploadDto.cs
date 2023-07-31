using System.ComponentModel.DataAnnotations;

namespace PersonalTravelPlan_BE.Dtos
{
    public class UploadDto
    {
        [Required]
        public IFormFile? file { get; set; }
    }
}
