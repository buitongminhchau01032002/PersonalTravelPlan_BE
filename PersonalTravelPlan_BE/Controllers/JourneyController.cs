using Microsoft.AspNetCore.Mvc;
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

        public JourneyController(IJourneyRepository journeyRepository)
        {
            _journeyRepository = journeyRepository;
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
            return CreatedAtAction(nameof(Get), new { id = 1 }, new Journey());
        }

        // PUT api/<JourneyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {
        }

        // DELETE api/<JourneyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
