using PersonalTravelPlan_BE.Models;
using System.ComponentModel.DataAnnotations;

namespace PersonalTravelPlan_BE.Dtos {
    public record UpdateJurneyDto {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int? DurationDay { get; set; }
        public int? DurationNight { get; set; }
        [Range(0, Int32.MaxValue)]
        public int? Amount { get; set; }
        [Required]
        [RegularExpression("^Planning$|^In progress$|^Finished$", ErrorMessage = "Invalid Status")]
        public string? Status { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        public int CountryId { get; set; }
        public int? CurrencyId { get; set; }
        public IList<int>? PlaceIds { get; set; } = new List<int>();
    }
}
