using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalTravelPlan_BE.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase {

        private readonly ICountryRepository _countryRepository;

        public CountryController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        // GET: api/<CountryController>
        [HttpGet]
        public ActionResult Get() {
            try {
                return Ok(_countryRepository.GetCountries());
            } catch (Exception) {
                return StatusCode(500);
            }
        }

        // GET api/<CountryController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id) {
            try {
                Country country = _countryRepository.GetCountryById(id);
                if (country != null) {
                    return Ok(country);
                } else {
                    return NotFound();
                }
            } catch (Exception) {
                return StatusCode(500);
            }
        }

        //// POST api/<CountryController>
        //[HttpPost]
        //public void Post([FromBody] string value) {
        //}

        //// PUT api/<CountryController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value) {
        //}

        //// DELETE api/<CountryController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id) {
        //}
    }
}
