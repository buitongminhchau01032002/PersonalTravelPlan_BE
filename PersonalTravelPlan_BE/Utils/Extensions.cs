using PersonalTravelPlan_BE.Dtos;
using PersonalTravelPlan_BE.Models;

namespace PersonalTravelPlan_BE.Utils {
    public static class Extensions {
        public static JourneyDto AsDto(this Journey journey) {
            return new JourneyDto() {
                Id = journey.Id,
                Name = journey.Name,
                Description = journey.Description,
                StartDate = journey.StartDate == null ? null : DateOnly.FromDateTime((DateTime)journey.StartDate),
                EndDate = journey.EndDate == null ? null : DateOnly.FromDateTime((DateTime)journey.EndDate),
                DurationDay = journey.DurationDay,
                DurationNight = journey.DurationNight,
                Amount = journey.Amount,
                Status = journey.Status,
                ImageUrl = journey.ImageUrl,
                Country = new CountryInJourneyDto() {
                    Id = journey.Country.Id,
                    Name = journey.Country.Name,
                    Code = journey.Country.Code,
                },
                Currency = journey.Currency,
                Places = journey.Places,
            };
        }
    }
}
