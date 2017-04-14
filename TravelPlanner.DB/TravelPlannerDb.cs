using System;
using System.Data.Entity;

namespace TravelPlanner.DB
{
    public partial class TravelPlannerDb : DbContext, IDisposable
    {
        public TravelPlannerDb(string connectionString)
            : base(connectionString)
        {
            base.Configuration.AutoDetectChangesEnabled = false;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Trip> Trips { get; set; }
    }
}
