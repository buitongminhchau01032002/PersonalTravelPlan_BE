using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace PersonalTravelPlan_BE.Models {
    public class Journey {
        public virtual int Id { get; set; }
        public virtual string? Name { get; set; }
        public virtual string? Description { get; set;}
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime ToDate { get; set; }
        public virtual int DurationDay { get; set; }
        public virtual int DurationNight { get; set;}
        public virtual int Amount { get; set; }
        public virtual string? Status { get; set; }
        public virtual string? ImageUrl { get; set; }
        public virtual Country? Country { get; set; }
        public virtual Currency? Currency { get; set; }
        public virtual ISet<Place> Places { get; set; } = new HashSet<Place>();
    }

    public class JourneyMap: ClassMapping<Journey> {
        public JourneyMap() {
            Id(x => x.Id, map => {
                map.Generator(Generators.Identity);
            });
            Property(x => x.Name);
            Property(x => x.Description);
            Property(x => x.FromDate);
            Property(x => x.ToDate);
            Property(x => x.DurationDay);
            Property(x => x.DurationNight);
            Property(x => x.Amount);
            Property(x => x.Status);
            Property(x => x.ImageUrl);
            ManyToOne(x => x.Country, map => {
                map.Column("CountryId");
                map.NotNullable(true);
            });
            ManyToOne(x => x.Currency, map => {
                map.Column("CurrencyId");
                map.NotNullable(true);
            });
            Set(x => x.Places, map => {
                map.Table("JourneyPlace");
                map.Cascade(Cascade.None);
                map.Key(k => k.Column("JourneyId"));
            }, rel => rel.ManyToMany(
                p => p.Column("PlaceId")
            )) ;
        }
    }
}
