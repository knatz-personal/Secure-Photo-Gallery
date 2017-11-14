using DataExchange.EntityModel;
using SharedResources.Contracts;

namespace DataExchange.Repositories
{
    public class ImageRepository : Repository<AppDbContext, Image>
    {
        public ImageRepository(AppDbContext context) : base(context)
        {
        }

        public bool Update(Image image)
        {
            throw new System.NotImplementedException();
        }
    }
}