using PersonalTravelPlan_BE.Models;

namespace PersonalTravelPlan_BE.Dtos {
    public record CreateJourneyDto {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public int DurationDay { get; set; }
        public int DurationNight { get; set; }
        public int Amount { get; set; }
        public string? Status { get; set; }
        public string? ImageUrl { get; set; }
        public int CountryId { get; set; }
        public int CurrencyId { get; set; }
        public IList<int> PlaceIds { get; set; } = new List<int>();
    }
}
