using Microsoft.AspNetCore.Mvc;
using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalTravelPlan_BE.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase {

        private readonly IPlaceRepository _placeRepository;
        public PlaceController(IPlaceRepository placeRepository)
        {
            _placeRepository = placeRepository;
        }

        // POST: api/<PlaceController>/GetByIds
        [HttpPost("GetByIds")]
        public ActionResult<IEnumerable<Place>> GetPlacesByIds(IList<int> ids) {

            try {
                var places = _placeRepository.GetPlacesByIds(ids);
                return Ok(places);
            } catch(Exception e) {
                return StatusCode(500);
            }

        }

        //// GET: api/<PlaceController>
        //[HttpGet]
        //public IEnumerable<string> Get() {
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<PlaceController>/5
        //[HttpGet("{id}")]
        //public string Get(int id) {
        //    return "value";
        //}

        //// POST api/<PlaceController>
        //[HttpPost]
        //public void Post([FromBody] string value) {
        //}

        //// PUT api/<PlaceController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value) {
        //}

        //// DELETE api/<PlaceController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id) {
        //}
    }
}
