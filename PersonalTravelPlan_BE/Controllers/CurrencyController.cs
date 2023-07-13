using Microsoft.AspNetCore.Mvc;
using NHibernate;
using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalTravelPlan_BE.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase {
        // GET: api/<Currency>
        [HttpGet]
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        // GET api/<Currency>/5
        [HttpGet("{id}")]
        public ActionResult<Currency> GetById(int id) {
            using (var session = NHibernateHelper.OpenSession()) {
                Currency currency = session.Get<Currency>(id);
                return currency;
            }
        }

        // POST api/<Currency>
        [HttpPost]
        public ActionResult<Currency> Post(Currency currency) {
            using (var session = NHibernateHelper.OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    // To be implemented
                    session.Save(currency);
                    transaction.Commit();
                    return CreatedAtAction(nameof(GetById), new { id = currency.Id }, currency);
                }
            }
        }

        // PUT api/<Currency>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {
        }

        // DELETE api/<Currency>/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
