using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace PersonalTravelPlan_BE.Models {
    public class JourneyPlace {
        public virtual int JourneyId { get; set; }
        public virtual int PlaceId { get; set; }
    }

    public class JourneyPlaceMap : ClassMapping<JourneyPlace> {
        public JourneyPlaceMap() {
            Property(x => x.JourneyId);
            Property(x => x.PlaceId);
            
        }
    }
}
