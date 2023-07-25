using NHibernate;
using NHibernate.Criterion;
using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Queries;
using PersonalTravelPlan_BE.Utils;

namespace PersonalTravelPlan_BE.Repositories {
    public interface IJourneyRepository {
        IList<Journey> GetJourneys(PaginationQuery paginationQuery, FilterQuery filterQuery);
        Journey GetJourneyById(int id);
        //int GetJourneyCount();
        Journey CreateJourney(Journey journey);
        Journey UpdateJourney(Journey journey);
        void DeleteJourney(int id);
        void DeleteManyJourney(List<int> ids);
    }

    public class JourneyRepository : IJourneyRepository {
        public IList<Journey> GetJourneys(PaginationQuery paginationQuery, FilterQuery filterQuery) {
            int _page = paginationQuery.page ?? 1;
            int _pageSize = paginationQuery.pageSize ?? 5;
            using (var session = NHibernateHelper.OpenSession()) {
                var holdConjunction = Restrictions.Conjunction();
                if (filterQuery.status != null) {
                    holdConjunction.Add(Restrictions.Eq("Status", filterQuery.status));
                }
                if (filterQuery.countryId != null) {
                    holdConjunction.Add(Restrictions.Eq("Country.Id", filterQuery.countryId));
                }
                if (filterQuery.currencyId != null) {
                    holdConjunction.Add(Restrictions.Eq("Currency.Id", filterQuery.currencyId));
                }
                if (filterQuery.amountFrom != null) {
                    holdConjunction.Add(Restrictions.Ge("Amount", filterQuery.amountFrom));
                }
                if (filterQuery.amountTo != null) {
                    holdConjunction.Add(Restrictions.Le("Amount", filterQuery.amountTo));
                }
                if (filterQuery.startDateFrom != null) {
                    holdConjunction.Add(Restrictions.Ge("StartDate", filterQuery.startDateFrom?.ToDateTime(TimeOnly.Parse("00:00 AM"))));
                }
                if (filterQuery.startDateTo != null) {
                    holdConjunction.Add(Restrictions.Le("StartDate", filterQuery.startDateTo?.ToDateTime(TimeOnly.Parse("00:00 AM"))));
                }
                if (filterQuery.endDateFrom != null) {
                    holdConjunction.Add(Restrictions.Ge("EndDate", filterQuery.endDateFrom?.ToDateTime(TimeOnly.Parse("00:00 AM"))));
                }
                if (filterQuery.endDateTo != null) {
                    holdConjunction.Add(Restrictions.Le("EndDate", filterQuery.endDateTo?.ToDateTime(TimeOnly.Parse("00:00 AM"))));
                }
                if (filterQuery.search != null) {
                    holdConjunction.Add(Restrictions.Disjunction()
                        .Add(Restrictions.Like("Name", filterQuery.search, MatchMode.Anywhere))
                        .Add(Restrictions.Like("Description", filterQuery.search, MatchMode.Anywhere))
                    );
                }

                var journeys = session.QueryOver<Journey>()
                                      .Fetch(x => x.Currency).Eager
                                      .Fetch(x => x.Country).Eager.Fetch(x => x.Country.Places).Eager
                                      .Fetch(x => x.Places).Eager
                                      .Where(holdConjunction)
                                      .Future<Journey>()
                                      .Distinct()
                                      //.Skip((_page-1) * _pageSize)
                                      //.Take(_pageSize)
                                      .ToList();
                return journeys;
            }
        }

        //public int GetJourneyCount() {
        //    using (var session = NHibernateHelper.OpenSession()) {
        //        int count = session.QueryOver<Journey>().RowCount();
        //        return count;
        //    }
        //}


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

        public void DeleteManyJourney(List<int> ids) {
            using (var session = NHibernateHelper.OpenSession()) {
                using (ITransaction transaction = session.BeginTransaction()) {
                    session.CreateQuery("DELETE JourneyPlace jp WHERE jp.JourneyId IN (:idList)")
                        .SetParameterList("idList", ids)
                        .ExecuteUpdate();
                    session.CreateQuery("DELETE Journey journey WHERE journey.Id IN (:idList)")
                        .SetParameterList("idList", ids)
                        .ExecuteUpdate();
                    transaction.Commit();
                }
            }
        }

    }

}
