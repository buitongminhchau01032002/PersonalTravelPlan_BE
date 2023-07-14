using Microsoft.AspNetCore.Mvc;
using NHibernate.Engine;
using PersonalTravelPlan_BE.Dtos;
using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Repositories;
using PersonalTravelPlan_BE.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalTravelPlan_BE.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class JourneyController : ControllerBase {

        private readonly IJourneyRepository _journeyRepository;
        private readonly IPlaceRepository _placeRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICountryRepository _countryRepository;

        public JourneyController(
            IJourneyRepository journeyRepository, 
            IPlaceRepository placeRepository, 
            ICurrencyRepository currencyRepository,
            ICountryRepository countryRepository)
        {
            _journeyRepository = journeyRepository;
            _placeRepository = placeRepository;
            _currencyRepository = currencyRepository;
            _countryRepository = countryRepository;
        }

        // GET: api/<JourneyController>
        [HttpGet]
        public ActionResult<IEnumerable<JourneyDto>> Get() {
            try {
                List<JourneyDto> journeys = _journeyRepository
                    .GetJourneys()
                    .Select(journey => journey.AsDto())
                    .ToList();
                return Ok(journeys);
            } catch (Exception e) {
                return StatusCode(500);
            }
        }

        // GET api/<JourneyController>/5
        [HttpGet("{id}")]
        public ActionResult<JourneyDto> Get(int id) {
            try {
                Journey journey = _journeyRepository.GetJourneyById(id);
                if (journey != null) {
                    return Ok(journey.AsDto());
                } else {
                    return NotFound();
                }
            } catch (Exception e) {
                return StatusCode(500);
            }
        }

        // POST api/<JourneyController>
        [HttpPost]
        public ActionResult<JourneyDto> Post(CreateJourneyDto createJouney) {
            try {
                // Get currency
                Currency currency = _currencyRepository.GetCurrencyById(createJouney.CurrencyId);
                if (currency == null) {
                    throw new Exception();
                }

                // Get country
                Country country = _countryRepository.GetCountryById(createJouney.CountryId);
                if (country == null) {
                    throw new Exception();
                }

                // Get places
                IList<Place> places = _placeRepository.GetPlacesByIds(createJouney.PlaceIds);

                Journey journey = new Journey() {
                    Name = createJouney.Name,
                    Description = createJouney.Description,
                    FromDate = createJouney.FromDate.ToDateTime(TimeOnly.Parse("00:00 AM")),
                    ToDate = createJouney.ToDate.ToDateTime(TimeOnly.Parse("00:00 AM")),
                    DurationDay = createJouney.DurationDay,
                    DurationNight = createJouney.DurationNight,
                    Amount = createJouney.Amount,
                    Status = createJouney.Status,
                    ImageUrl = createJouney.ImageUrl,
                    Country = country,
                    Currency = currency,
                    Places = new HashSet<Place>(places)
                };

                journey = _journeyRepository.CreateJourney(journey);

                return CreatedAtAction(nameof(Get), new { id = 1 }, journey.AsDto());
            } catch (Exception e) {
                return StatusCode(500);
            }
        }

        // PUT api/<JourneyController>/5
        [HttpPut("{id}")]
        public ActionResult<JourneyDto> Put(int id, CreateJourneyDto updateJourney) {
            try {
                // Get currency
                Currency currency = _currencyRepository.GetCurrencyById(updateJourney.CurrencyId);
                if (currency == null) {
                    throw new Exception();
                }

                // Get country
                Country country = _countryRepository.GetCountryById(updateJourney.CountryId);
                if (country == null) {
                    throw new Exception();
                }

                // Get places
                IList<Place> places = _placeRepository.GetPlacesByIds(updateJourney.PlaceIds);

                Journey journey = new Journey() {
                    Id = id,
                    Name = updateJourney.Name,
                    Description = updateJourney.Description,
                    FromDate = updateJourney.FromDate.ToDateTime(TimeOnly.Parse("00:00 AM")),
                    ToDate = updateJourney.ToDate.ToDateTime(TimeOnly.Parse("00:00 AM")),
                    DurationDay = updateJourney.DurationDay,
                    DurationNight = updateJourney.DurationNight,
                    Amount = updateJourney.Amount,
                    Status = updateJourney.Status,
                    ImageUrl = updateJourney.ImageUrl,
                    Country = country,
                    Currency = currency,
                    Places = new HashSet<Place>(places)
                };

                journey = _journeyRepository.UpdateJourney(journey);

                return Ok(journey.AsDto());
            } catch (Exception e) {
                return StatusCode(500);
            }
        }

        // DELETE api/<JourneyController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) {
            try {
                _journeyRepository.DeleteJourney(id);
                return Ok();
            } catch (Exception e) {
                return StatusCode(500);
            }
        }
    }
}
