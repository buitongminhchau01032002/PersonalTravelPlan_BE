using NHibernate;
using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Queries;
using PersonalTravelPlan_BE.Utils;

namespace PersonalTravelPlan_BE.Repositories {
    public interface IJourneyRepository {
        IList<Journey> GetJourneys(PaginationQuery paginationQuery);
        Journey GetJourneyById(int id);
        int GetJourneyCount();
        Journey CreateJourney(Journey journey);
        Journey UpdateJourney(Journey journey);
        public void DeleteJourney(int id);
    }

    public class JourneyRepository : IJourneyRepository {
        public IList<Journey> GetJourneys(PaginationQuery paginationQuery) {
            int _page = paginationQuery.page ?? 1;
            int _pageSize = paginationQuery.pageSize ?? 5;
            using (var session = NHibernateHelper.OpenSession()) {
                var journeys = session.QueryOver<Journey>()
                                      .Fetch(x => x.Currency).Eager
                                      .Fetch(x => x.Country).Eager.Fetch(x => x.Country.Places).Eager
                                      .Fetch(x => x.Places).Eager
                                      .Future<Journey>()
                                      .Distinct()
                                      .Skip((_page-1) * _pageSize)
                                      .Take(_pageSize)
                                      .ToList();
                return journeys;
            }
        }

        public int GetJourneyCount() {
            using (var session = NHibernateHelper.OpenSession()) {
                int count = session.QueryOver<Journey>().RowCount();
                return count;
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

        public Journey UpdateJourney(Journey journey) {
            using (var session = NHibernateHelper.OpenSession()) {
                using (var transaction = session.BeginTransaction()) {
                    session.Update(journey);
                    transaction.Commit();
                    return journey;
                }
            }
        }

        public void DeleteJourney(int id) {
            using (var session = NHibernateHelper.OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    Journey journey = session.Get<Journey>(id);
                    session.Delete(journey);
                    transaction.Commit();
                }
            }
        }

    }

}
