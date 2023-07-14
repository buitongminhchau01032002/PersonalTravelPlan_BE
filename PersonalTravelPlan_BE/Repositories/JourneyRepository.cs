using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Utils;

namespace PersonalTravelPlan_BE.Repositories {
    public interface IJourneyRepository {
        IList<Journey> GetJourneys();
        Journey GetJourneyById(int id);
        Journey CreateJourney(Journey journey);
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

        public Journey GetJourneyById(int id) {
            using (var session = NHibernateHelper.OpenSession()) {
                var journey = session.QueryOver<Journey>()
                                      .Where(x => x.Id == id)
                                      .Fetch(x => x.Currency).Eager
                                      .Fetch(x => x.Country).Eager.Fetch(x => x.Country.Places).Eager
                                      .Fetch(x => x.Places).Eager
                                      .SingleOrDefault<Journey>();
                return journey;
            }
        }

        public Journey CreateJourney(Journey journey) {
            using (var session = NHibernateHelper.OpenSession()) {
                using (var transaction =  session.BeginTransaction()) {
                    session.Save(journey);
                    transaction.Commit();
                    return journey;
                }
            }
        }
    }

}
