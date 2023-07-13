using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Utils;

namespace PersonalTravelPlan_BE.Repositories {
    public interface IJourneyRepository {
        IList<Journey> GetJourneys();
    }

    public class JourneyRepository : IJourneyRepository {
        public IList<Journey> GetJourneys() {
            using (var session = NHibernateHelper.OpenSession()) {
                var journeys = session.QueryOver<Journey>()
                                      .Fetch(x => x.Currency).Eager
                                      .Fetch(x => x.Country).Eager.Fetch(x => x.Country.Places).Eager
                                      .Fetch(x => x.Places).Eager
                                      .List();
                return journeys;
            }
        }
    }

}
