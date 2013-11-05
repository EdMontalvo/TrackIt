using System.Data.Entity;

namespace TrackIt.Web.Data
{
    public class DailyWeightContext : DbContext
    {
        public DailyWeightContext()
            : base("DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<DailyWeightContext, DailyWeightMigrationsConfiguration>()
                );
        }
        public DbSet<DailyWeight> DailyWeights { get; set; }
    }
}