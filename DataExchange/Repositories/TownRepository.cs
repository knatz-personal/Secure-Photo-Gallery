using DataExchange.EntityModel;
using SharedResources.Contracts;

namespace DataExchange.Repositories
{
    public class TownRepository : Repository<AppDbContext, Town>
    {
        public TownRepository(AppDbContext context) : base(context)
        {
        }

        public bool Update(Town town)
        {
            bool result = false;

            var wf = Read(w => w.ID == town.ID);

            wf.Name = town.Name;

            GetContext.SaveChanges();

            wf = Read(w => w.ID == town.ID);

            if (wf.Name == town.Name)
            {
                result = true;
            }

            return result;
        }
    }
}