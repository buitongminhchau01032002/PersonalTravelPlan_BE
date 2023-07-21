using NHibernate;
using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Utils;

namespace PersonalTravelPlan_BE.Repositories {
    public interface ICountryRepository {
        IList<Country> GetCountries();
        Country? GetCountryById(int? id);
    }

    public class CountryRepository : ICountryRepository {
        public IList<Country> GetCountries() {
            using (var session = NHibernateHelper.OpenSession()) {
                var currencies = session.QueryOver<Country>()
                                        .Fetch(x => x.Places).Eager
                                        .Future<Country>()
                                        .Distinct()
                                        .ToList();
                return currencies;
            }
        }

        public Country? GetCountryById(int? id) {
            if (id == null) {
                return null;
            }
            using (var session = NHibernateHelper.OpenSession()) {
                Country currency = session.QueryOver<Country>()
                                          .Where(x => x.Id == id)
                                          .Fetch(x => x.Places).Eager
                                          .SingleOrDefault<Country>();
                return currency;
            }
        }
    }
}
