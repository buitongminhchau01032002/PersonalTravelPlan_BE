using PersonalTravelPlan_BE.Models;
using PersonalTravelPlan_BE.Utils;

namespace PersonalTravelPlan_BE.Repositories {
    public interface IUserRepository {
        User GetUserByUsernameAndPassword(string? username, string? password);
    }
    public class UserRepository : IUserRepository {
        public User GetUserByUsernameAndPassword(string? username, string? password) {
            using (var session = NHibernateHelper.OpenSession()) {
                var user = session.QueryOver<User>()
                                  .Where(x => x.Username == username && x.Password == password)
                                  .SingleOrDefault();
                return user;
            }
        }
    }
}
