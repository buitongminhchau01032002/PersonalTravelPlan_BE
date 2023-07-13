using Microsoft.AspNetCore.Mvc;
using NHibernate;
using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Repositories;
using PersonalTravelPlan_BE.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalTravelPlan_BE.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase {

        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyController(ICurrencyRepository currencyRepository) {
            _currencyRepository = currencyRepository;
        }

        // GET: api/<Currency>
        [HttpGet]
        public ActionResult Get() {
            try {
                return Ok(_currencyRepository.GetCurrencies());
            } catch(Exception) {
                return StatusCode(500);
            }
        }

        // GET api/<Currency>/5
        [HttpGet("{id}")]
        public ActionResult GetById(int id) {
            try {
                Currency currency = _currencyRepository.GetCurrencyById(id);
                if (currency != null) {
                    return Ok(currency);
                } else {
                    return NotFound();
                }
            } catch(Exception) {
                return StatusCode(500);
            }
        }

        //// POST api/<Currency>
        //[HttpPost]
        //public ActionResult<Currency> Post(Currency currency) {
        //    using (var session = NHibernateHelper.OpenSession()) {
        //        using (ITransaction transaction = session.BeginTransaction()) {
        //            // To be implemented
        //            session.Save(currency);
        //            transaction.Commit();
        //            return CreatedAtAction(nameof(GetById), new { id = currency.Id }, currency);
        //        }
        //    }
        //}

        //// PUT api/<Currency>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value) {
        //}

        //// DELETE api/<Currency>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id) {
        //}
    }
}
