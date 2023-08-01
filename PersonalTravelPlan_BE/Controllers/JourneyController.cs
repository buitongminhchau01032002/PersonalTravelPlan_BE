using Microsoft.AspNetCore.Authorization;
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
    //[Authorize]
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
        public ActionResult<JourneyDto> Post(CreateJourneyDto createJourney) {
            try {
                // Get currency
                Currency? currency = null;
                if (createJourney.CurrencyId != null) {
                    currency = _currencyRepository.GetCurrencyById(createJourney.CurrencyId);
                    if (currency == null) {
                        return BadRequest();
                    }
                }

                // Get country
                Country country = _countryRepository.GetCountryById(createJourney.CountryId);
                if (country == null) {
                    return BadRequest();
                }

                // Validate end date
                if (createJourney.EndDate != null && createJourney.EndDate <= createJourney.StartDate) {
                    return BadRequest();
                }

                // Validate duration
                if (createJourney.DurationDay != null) {
                    if (createJourney.DurationDay <= 0) {
                        return BadRequest();
                    }
                    if (createJourney.EndDate != null && createJourney.DurationDay > createJourney.EndDate?.DayNumber - createJourney.StartDate?.DayNumber + 1) {
                        return BadRequest();
                    }
                }
                if (createJourney.DurationNight != null) {
                    if (createJourney.DurationNight <= 0) {
                        return BadRequest();
                    }
                    if (createJourney.EndDate != null && createJourney.DurationNight > createJourney.EndDate?.DayNumber - createJourney.StartDate?.DayNumber + 1) {
                        return BadRequest();
                    }
                }
                if (createJourney.DurationDay != null && createJourney.DurationNight != null) {
                    if (Math.Abs((int)createJourney.DurationDay - (int)createJourney.DurationNight) > 1) {
                        return BadRequest();
                    }
                }

                // Get places
                IList<Place>? places = null;
                if (createJourney.PlaceIds != null) {
                    places = _placeRepository.GetPlacesByIds(createJourney.PlaceIds);
                }

                Journey journey = new Journey() {
                    Name = createJourney.Name,
                    Description = createJourney.Description,
                    StartDate = createJourney.StartDate == null ? null : createJourney.StartDate?.ToDateTime(TimeOnly.Parse("00:00 AM")),
                    EndDate = createJourney.EndDate == null ? null : createJourney.EndDate?.ToDateTime(TimeOnly.Parse("00:00 AM")),
                    DurationDay = createJourney.DurationDay,
                    DurationNight = createJourney.DurationNight,
                    Amount = createJourney.Amount,
                    Status = createJourney.Status,
                    ImageUrl = createJourney.ImageUrl,
                    Country = country,
                    Currency = currency,
                    Image = createJourney.Image,
                    Places = places == null ? null : new HashSet<Place>(places)
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
                Currency? currency = null;
                if (updateJourney.CurrencyId != null) {
                    currency = _currencyRepository.GetCurrencyById(updateJourney.CurrencyId);
                    if (currency == null) {
                        return BadRequest();
                    }
                }

                // Get country
                Country country = _countryRepository.GetCountryById(updateJourney.CountryId);
                if (country == null) {
                    return BadRequest();
                }

                // Validate end date
                if (updateJourney.EndDate != null && updateJourney.EndDate <= updateJourney.StartDate) {
                    return BadRequest();
                }

                // Validate duration
                if (updateJourney.DurationDay != null) {
                    if (updateJourney.DurationDay <= 0) {
                        return BadRequest();
                    }
                    if (updateJourney.EndDate != null && updateJourney.DurationDay > updateJourney.EndDate?.DayNumber - updateJourney.StartDate?.DayNumber + 1) {
                        return BadRequest();
                    }
                }
                if (updateJourney.DurationNight != null) {
                    if (updateJourney.DurationNight <= 0) {
                        return BadRequest();
                    }
                    if (updateJourney.EndDate != null && updateJourney.DurationNight > updateJourney.EndDate?.DayNumber - updateJourney.StartDate?.DayNumber + 1) {
                        return BadRequest();
                    }
                }
                if (updateJourney.DurationDay != null && updateJourney.DurationNight != null) {
                    if (Math.Abs((int)updateJourney.DurationDay - (int)updateJourney.DurationNight) > 1) {
                        return BadRequest();
                    }
                }

                // Get places
                IList<Place>? places = null;
                if (updateJourney.PlaceIds != null) {
                    places = _placeRepository.GetPlacesByIds(updateJourney.PlaceIds);
                }



                Journey journey = new Journey() {
                    Id = id,
                    Name = updateJourney.Name,
                    Description = updateJourney.Description,
                    StartDate = updateJourney.StartDate == null ? null : updateJourney.StartDate?.ToDateTime(TimeOnly.Parse("00:00 AM")),
                    EndDate = updateJourney.EndDate == null ? null : updateJourney.EndDate?.ToDateTime(TimeOnly.Parse("00:00 AM")),
                    DurationDay = updateJourney.DurationDay,
                    DurationNight = updateJourney.DurationNight,
                    Amount = updateJourney.Amount,
                    Status = updateJourney.Status,
                    ImageUrl = updateJourney.ImageUrl,
                    Country = country,
                    Currency = currency,
                    Image = updateJourney.Image,
                    Places = places == null ? null : new HashSet<Place>(places)
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

        // DELETE api/<JourneyController>/5
        [HttpDelete("many")]
        public ActionResult DeleteMany([FromBody] List<int> ids) {
            try {
                _journeyRepository.DeleteManyJourney(ids);
                return Ok();
            } catch (Exception e) {
                return StatusCode(500);
            }
        }
    }
}
