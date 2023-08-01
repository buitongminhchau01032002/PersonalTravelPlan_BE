using System.ComponentModel.DataAnnotations;

namespace PersonalTravelPlan_BE.Dtos {
    public record LoginDto {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
