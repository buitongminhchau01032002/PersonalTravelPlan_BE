using PersonalTravelPlan_BE.Dtos;
using PersonalTravelPlan_BE.Models;

namespace PersonalTravelPlan_BE.Utils {
    public static class Extensions {
        public static JourneyDto AsDto(this Journey journey) {
            return new JourneyDto() {
                Id = journey.Id,
                Name = journey.Name,
                Description = journey.Description,
                FromDate = DateOnly.FromDateTime(journey.FromDate),
                ToDate = DateOnly.FromDateTime(journey.ToDate),
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
