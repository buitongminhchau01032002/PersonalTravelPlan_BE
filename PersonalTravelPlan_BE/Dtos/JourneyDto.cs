using PersonalTravelPlan_BE.Models;

namespace PersonalTravelPlan_BE.Dtos {

    public record CountryInJourneyDto {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }

    public record JourneyDto {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int? DurationDay { get; set; }
        public int? DurationNight { get; set; }
        public int? Amount { get; set; }
        public string? Status { get; set; }
        public string? ImageUrl { get; set; }
        public byte[]? Image { get; set; }
        public CountryInJourneyDto? Country { get; set; }
        public Currency? Currency { get; set; }
        public ISet<Place>? Places { get; set; } = new HashSet<Place>();
    }
}
