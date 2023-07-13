using Microsoft.AspNetCore.Mvc;
using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Repositories;

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
        public ActionResult Get() {
            try {
                return Ok(_journeyRepository.GetJourneys());
            } catch (Exception e) {
                return StatusCode(500);
            }
        }

        // GET api/<JourneyController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id) {
            try {
                Journey journey = _journeyRepository.GetJourneyById(id);
                if (journey != null) {
                    return Ok(journey);
                } else {
                    return NotFound();
                }
            } catch (Exception e) {
                return StatusCode(500);
            }
        }

        // POST api/<JourneyController>
        [HttpPost]
        public ActionResult Post(Journey journey) {
            return CreatedAtAction(nameof(Get), new { id = 1 }, journey);
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
