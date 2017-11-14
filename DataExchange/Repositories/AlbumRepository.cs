using DataExchange.EntityModel;
using SharedResources.Contracts;

namespace DataExchange.Repositories
{
    public class AlbumRepository : Repository<AppDbContext, Album>
    {
        public AlbumRepository(AppDbContext context) : base(context)
        {
        }

        public bool Update(Album model)
        {
            throw new System.NotImplementedException();
        }
    }
}