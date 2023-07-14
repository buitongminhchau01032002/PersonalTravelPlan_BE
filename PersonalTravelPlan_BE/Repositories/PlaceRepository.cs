using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Utils;

namespace PersonalTravelPlan_BE.Repositories {
    public interface IPlaceRepository {
        IList<Place> GetPlacesByIds(IList<int> ids);
    }

    public class PlaceRepository : IPlaceRepository {
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
