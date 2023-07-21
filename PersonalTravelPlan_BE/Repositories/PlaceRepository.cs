using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Utils;

namespace PersonalTravelPlan_BE.Repositories {
    public interface IPlaceRepository {
        IList<Place> GetPlacesByIds(IList<int> ids);
        IList<Place> GetPlacesByCountryId(int countryId);
    }

    public class PlaceRepository : IPlaceRepository {
        public IList<Place> GetPlacesByCountryId(int countryId) {
            using (var session = NHibernateHelper.OpenSession()) {
                var places = session.QueryOver<Place>()
                                    .Where(p => p.CountryId == countryId)
                                    .List();
                return places;
            }
        }

        public IList<Place> GetPlacesByIds(IList<int> ids) {
            using (var session = NHibernateHelper.OpenSession()) {
                var places = session.QueryOver<Place>()
                                    .WhereRestrictionOn(place => place.Id)
                                    .IsIn(ids.ToArray())
                                    .List();
                return places;
            }
        }
    }
}
