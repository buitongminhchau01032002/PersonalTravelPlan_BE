using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;

namespace PersonalTravelPlan_BE.Models {
    public class Country {
        public virtual int Id { get; set; }
        public virtual string? Name { get; set; }
        public virtual string? Code { get; set;}
        public virtual ISet<Place> Places { get; set; } = new HashSet<Place>();

    }

    public class CountryMap : ClassMapping<Country> {
        public CountryMap() {
            Id(x => x.Id, map => {
                map.Generator(Generators.Identity);
            });
            Property(x => x.Name);
            Property(x => x.Code);
            Set(x => x.Places, map => {
                map.Cascade(Cascade.All | Cascade.DeleteOrphans);
                map.Inverse(true);
                map.Key(k => k.Column("CountryId"));
            }, rel => rel.OneToMany());
        }
    }
}
