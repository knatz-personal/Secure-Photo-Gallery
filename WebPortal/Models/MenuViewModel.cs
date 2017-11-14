using System.Collections.Generic;

namespace WebPortal.Models
{
    public class MenuViewModel
    {
        public IEnumerable<MenuModel> RootMenu { get; set; }
        public IEnumerable<MenuModel> SubMenu { get; set; }
    }
}