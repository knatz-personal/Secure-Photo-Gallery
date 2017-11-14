namespace DataExchange.EntityModel
{
    public partial class AppDbContext
    {
        public AppDbContext(string connectionString) : base("name=" + connectionString)
        {
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
    }
}