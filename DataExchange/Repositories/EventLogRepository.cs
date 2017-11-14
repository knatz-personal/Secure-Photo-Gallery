using DataExchange.EntityModel;
using SharedResources.Contracts;

namespace DataExchange.Repositories
{
    public class EventLogRepository : Repository<AppDbContext, EventLog>
    {
        public EventLogRepository(AppDbContext context) : base(context)
        {
        }
    }
}