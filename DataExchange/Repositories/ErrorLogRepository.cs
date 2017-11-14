using DataExchange.EntityModel;
using SharedResources.Contracts;

namespace DataExchange.Repositories
{
    public class ErrorLogRepository : Repository<AppDbContext, ErrorLog>
    {
        public ErrorLogRepository(AppDbContext context) : base(context)
        {
        }
    }
}