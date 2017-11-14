using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataExchange.EntityModel;
using SharedResources.Contracts;

namespace DataExchange.Repositories
{
    public class GenderRepository : Repository<AppDbContext, Gender>
    {
        public GenderRepository(AppDbContext context) : base(context)
        {
        }

        public bool Update(Gender gender)
        {
            bool result = false;

            var wf = Read(w => w.ID == gender.ID);

            wf.Name = gender.Name;

            GetContext.SaveChanges();

            wf = Read(w => w.ID == gender.ID);

            if (wf.Name == gender.Name)
            {
                result = true;
            }

            return result;
        }
    }
}