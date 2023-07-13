using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace PersonalTravelPlan_BE.Models {
    public class Currency {
        public virtual int Id { get; set; }
        public virtual string? Name { get; set; }
    }

    public class CurrencyMap : ClassMapping<Currency> {
        public CurrencyMap() {
            Id(x => x.Id, map => {
                map.Generator(Generators.Identity);
            });
            Property(x => x.Name);
        }
    }
}
