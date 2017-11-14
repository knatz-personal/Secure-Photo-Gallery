using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DataExchange;
using DataExchange.EntityModel;
using SSD.Web.Api.Models;

namespace SSD.Web.Api.Controllers
{
    public class MenuController : ApiController
    {
        public MenuController(DataManager dataManager)
        {
            db = dataManager;
        }

        public MenuController()
        {
            db = new DataManager(new AppDbContext());
        }

        [Route("api/Menus")]
        [ResponseType(typeof(List<MenuListBindingModel>))]
        public IHttpActionResult Get()
        {
            var list = db.Menus.ListAll();
            if (list.Any())
            {
                return Ok(list.Select(item => new MenuListBindingModel()
                {
                    ID = item.ID,
                    Name = item.Name,
                    SortOrder = item.SortOrder,
                    ParentID = item.ParentID,
                    Description = item.Description,
                    Action = item.ActionName,
                    Controller = item.ControllerName,
                    Url = item.ControllerName != null ? string.Format("/{0}/{1}", item.ControllerName, item.ActionName) : item.Url
                }));
            }
            return NotFound();
        }

        [Route("api/Menu/Root")]
        [ResponseType(typeof(List<MenuListBindingModel>))]
        public IHttpActionResult GetRootMenus(string roleId)
        {
            try
            {
                var mainMenu = db.Menus.GetMenuByRole(roleId).ToList();
                if (mainMenu.Any())
                {
                    return Ok(mainMenu.Select(item => new MenuListBindingModel()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        SortOrder = item.SortOrder,
                        ParentID = item.ParentID,
                        Description = item.Description,
                        Action = item.ActionName,
                        Controller = item.ControllerName,
                        Url = item.ControllerName != null ? string.Format("/{0}/{1}", item.ControllerName, item.ActionName) : item.Url
                    }));
                }
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NotFound();
        }

        [Route("api/Menu/Sub")]
        [ResponseType(typeof(List<MenuListBindingModel>))]
        public IHttpActionResult GetSubMenus(string roleId)
        {
            try
            {
                var subMenu = db.Menus.GetSubMenusByRole(roleId).ToList();
                if (subMenu.Any())
                {
                    return Ok(subMenu.Select(item => new MenuListBindingModel()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        SortOrder = item.SortOrder,
                        ParentID = item.ParentID,
                        Description = item.Description,
                        Action = item.ActionName,
                        Controller = item.ControllerName,
                        Url = item.ControllerName != null ? string.Format("/{0}/{1}", item.ControllerName, item.ActionName) : item.Url
                    }));
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
            return NotFound();
        }

        private DataManager db { get; }
    }
}