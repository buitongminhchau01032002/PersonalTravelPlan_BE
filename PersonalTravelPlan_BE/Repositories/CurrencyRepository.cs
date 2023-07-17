using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Utils;

namespace PersonalTravelPlan_BE.Repositories {
    public interface ICurrencyRepository {
        IList<Currency> GetCurrencies();
        Currency GetCurrencyById(int? id);
    }

    public class CurrencyRepository : ICurrencyRepository {
        public IList<Currency> GetCurrencies() {
            using (var session = NHibernateHelper.OpenSession()) {
                var currencies = session.QueryOver<Currency>().List();
                return currencies;
            }
        }

        public Currency GetCurrencyById(int? id) {
            using (var session = NHibernateHelper.OpenSession()) {
                Currency currency = session.Get<Currency>(id);
                return currency;
            }
        }
    }
}
