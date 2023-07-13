using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;

namespace PersonalTravelPlan_BE.Models {
    public class Place {
        public virtual int Id { get; set; }
        public virtual string? Name { get; set; }
        public virtual int CountryId { get; set;}
    }

    public class PlaceMap : ClassMapping<Place> {
        public PlaceMap() {
            Id(x => x.Id, map => {
                map.Generator(Generators.Identity);
            });
            Property(x => x.Name);
            Property(x => x.CountryId);
        }
    }
}
