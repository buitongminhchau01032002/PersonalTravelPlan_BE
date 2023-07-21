using Microsoft.AspNetCore.Mvc;
using PersonalTravelPlan_BE.Dtos;
using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Queries;
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
        public ActionResult<IEnumerable<JourneyDto>> Get([FromQuery]PaginationQuery paginationQuery, [FromQuery]FilterQuery filterQuery) {
            try {

                // get from db
                List<JourneyDto> journeys = _journeyRepository
                    .GetJourneys(paginationQuery, filterQuery)
                    .Select(journey => journey.AsDto())
                    .ToList();

                // pagination
                int _total = journeys.Count;
                int _page = paginationQuery.page ?? 1;
                int _pageSize = paginationQuery.pageSize ?? 5;
                journeys = journeys.Skip((_page - 1) * _pageSize).Take(_pageSize).ToList();

                // transfrom data to response
                JourneyListDtos journeyListDtos = new JourneyListDtos() { 
                    page = paginationQuery.page ?? 1, 
                    pageSize = paginationQuery.pageSize ?? 5, 
                    total = _total,
                    totalPage = (int)Math.Ceiling((float)_total / (paginationQuery.pageSize ?? 5)),
                    data = journeys
                };

                return Ok(journeyListDtos);
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
                Currency? currency = null;
                if (createJouney.CurrencyId != null) {
                    currency = _currencyRepository.GetCurrencyById(createJouney.CurrencyId);
                    if (currency == null) {
                        return BadRequest();
                    }
                }

                // Get country
                Country country = _countryRepository.GetCountryById(createJouney.CountryId);
                if (country == null) {
                    return BadRequest();
                }

                // Validate end date
                if (createJouney.EndDate != null && createJouney.EndDate <= createJouney.StartDate) {
                    return BadRequest();
                }

                // Validate duration
                if (createJouney.DurationDay != null) {
                    if (createJouney.DurationDay <= 0) {
                        return BadRequest();
                    }
                    if (createJouney.EndDate != null && createJouney.DurationDay > createJouney.EndDate?.DayNumber - createJouney.StartDate?.DayNumber + 1) {
                        return BadRequest();
                    }
                }
                if (createJouney.DurationNight != null) {
                    if (createJouney.DurationNight <= 0) {
                        return BadRequest();
                    }
                    if (createJouney.EndDate != null && createJouney.DurationNight > createJouney.EndDate?.DayNumber - createJouney.StartDate?.DayNumber + 1) {
                        return BadRequest();
                    }
                }
                if (createJouney.DurationDay != null && createJouney.DurationNight != null) {
                    if (Math.Abs((int)createJouney.DurationDay - (int)createJouney.DurationNight) > 1) {
                        return BadRequest();
                    }
                }

                // Get places
                IList<Place> places = _placeRepository.GetPlacesByIds(createJouney.PlaceIds);

                Journey journey = new Journey() {
                    Name = createJouney.Name,
                    Description = createJouney.Description,
                    StartDate = createJouney.StartDate == null ? null : createJouney.StartDate?.ToDateTime(TimeOnly.Parse("00:00 AM")),
                    EndDate = createJouney.EndDate == null ? null : createJouney.EndDate?.ToDateTime(TimeOnly.Parse("00:00 AM")),
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
        public ActionResult<JourneyDto> Put(int id, UpdateJurneyDto updateJourney) {
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
                    StartDate = updateJourney.StartDate.ToDateTime(TimeOnly.Parse("00:00 AM")),
                    EndDate = (DateTime)(updateJourney.EndDate?.ToDateTime(TimeOnly.Parse("00:00 AM"))),
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
