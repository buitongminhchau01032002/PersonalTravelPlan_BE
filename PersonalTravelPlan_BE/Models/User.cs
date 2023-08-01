using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace PersonalTravelPlan_BE.Models {
    public class User {
        public virtual int Id { get; set; }
        public virtual string? Username { get; set; }
        public virtual string? Password { get; set;}
    }

    public class UserMap : ClassMapping<User> {
        public UserMap() {
            Table("[User]");
            Id(x => x.Id, map => {
                map.Generator(Generators.Identity);
            });
            Property(x => x.Username);
            Property(x => x.Password);
        }
    }
}
