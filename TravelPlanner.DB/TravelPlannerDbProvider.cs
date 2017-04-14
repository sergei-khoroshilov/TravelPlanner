namespace TravelPlanner.DB
{
    public class TravelPlannerDbProvider
    {
        private readonly string connectionString;

        public TravelPlannerDbProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public TravelPlannerDb Get()
        {
            return new TravelPlannerDb(connectionString);
        }
    }
}
