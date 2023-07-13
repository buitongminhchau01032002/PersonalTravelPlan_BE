using NHibernate.Mapping.ByCode;
using NHibernate;
using System.Reflection;
using NHibernate.Cfg;
using PersonalTravelPlan_BE.Models;

namespace PersonalTravelPlan_BE.Utils {
    public class NHibernateHelper {
        private static ISessionFactory? _sessionFactory;

        private static ISessionFactory SessionFactory {
            get {
                if (_sessionFactory == null) {
                    try {
                        var configuration = new Configuration();
                        configuration.DataBaseIntegration((c) => { c.LogFormattedSql = true; c.LogSqlInConsole = true; });
                        configuration.Configure();
                        configuration.AddAssembly(Assembly.GetExecutingAssembly());

                        var modelMapper = new ModelMapper();
                        modelMapper.AddMappings(typeof(CurrencyMap).Assembly.GetExportedTypes());

                        var mapping = modelMapper.CompileMappingForAllExplicitlyAddedEntities();
                        configuration.AddDeserializedMapping(mapping, null);

                        _sessionFactory = configuration.BuildSessionFactory();
                    } catch (Exception ex) {
                        throw new Exception("NHibernate initialization failed", ex);
                    }
                }
                return _sessionFactory;
            }
        }

        public static NHibernate.ISession OpenSession() {
            return SessionFactory.OpenSession();
        }
    }
}
