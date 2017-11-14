using System;
using System.Collections.Generic;
using System.Linq;
using DataExchange.EntityModel;
using SharedResources.Contracts;

namespace DataExchange.Repositories
{
    public class MenuRepository : Repository<AppDbContext, Menu>
    {
        public MenuRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Menu> GetMenuByRole(string roleId)
        {
            var r = GetContext.AspNetRoles.SingleOrDefault(rm => rm.Name == roleId || rm.Name == roleId);

            if (r != null)
            {
                var result = from mm in r.Menus
                             where mm.ParentID == null || mm.ParentID == 0
                             select mm;

                return result;
            }
            throw new ArgumentException("Role name not found");
        }

        public IEnumerable<Menu> GetSubMenusByRole(string roleId)
        {
            var role = GetContext.AspNetRoles.SingleOrDefault(rm => rm.Id == roleId || rm.Name == roleId);

            if (role == null) throw new NullReferenceException("GetSubMenusByRole: role is null");

            var result = role.Menus.Where(pid => pid?.ParentID != null || pid.ParentID > 0);

            result = result.OrderBy(m => m.SortOrder);

            return result;
        }
    }
}