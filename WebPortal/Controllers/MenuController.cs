using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using WebPortal.Helpers;
using WebPortal.Models;

namespace WebPortal.Controllers
{
    public class MenuController : Controller
    {
        #region Properties

        public AppUserManager UserManager => _userManager ?? Request.GetOwinContext().GetUserManager<AppUserManager>();

        private AppRoleManager GetRoleManager => HttpContext.GetOwinContext().Get<AppRoleManager>();

        #endregion Properties

        public MenuController()
        {
            _dbcontext = new ApplicationDbContext();
            _roleManager = new RoleManager<AspNetRole>(new RoleStore<AspNetRole>(_dbcontext));

            _client = HttpClientHelpers.GetApiClient(_baseUrl);
        }

        [AllowAnonymous]
        public PartialViewResult _Menu()
        {
            //Show the menu with the highest privileges first
            string roleId = null;
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole(ADMINROLE))
                {
                    var singleOrDefault = GetRoleManager.FindByName(ADMINROLE);
                    if (singleOrDefault != null)
                    {
                        roleId = ADMINROLE;
                    }
                }
                else if (User.IsInRole(CLIENTROLE))
                {
                    var singleOrDefault = GetRoleManager.FindByName(CLIENTROLE);
                    if (singleOrDefault != null)
                    {
                        roleId = CLIENTROLE;
                    }
                }
            }
            else
            {
                var singleOrDefault = GetRoleManager.FindByName(ANONROLE);
                if (singleOrDefault == null)
                {
                    throw new NullReferenceException("Role not found!");
                }
                roleId = ANONROLE;
            }

            var model = new MenuViewModel();
            if (roleId != null)
            {
                model.RootMenu = GetMenuByRole(roleId, "/Root");
                model.SubMenu = GetMenuByRole(roleId, "/Sub");
            }
            return PartialView("_Menu", model);
        }

        private IEnumerable<MenuModel> GetMenuByRole(string roleId, string route = null)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                throw new ArgumentNullException("roleId");
            }

            HttpResponseMessage responseMessage = null;

            if (string.IsNullOrEmpty(route))
            {
                responseMessage = _client.GetAsync(_baseUrl + "?roleId=" + roleId).Result;
            }
            else
            {
                responseMessage = _client.GetAsync(_baseUrl + route + "?roleId=" + roleId).Result;
            }

            if (responseMessage == null)
            {
                throw new NullReferenceException("MenuController>GetMenuByRole: responseMessage is null");
            }

            var responseData = responseMessage.Content.ReadAsStringAsync().Result;

            if (responseData != null)
            {
                var menus = _serializer.Deserialize<List<MenuModel>>(responseData);

                if (menus == null)
                {
                    menus = new List<MenuModel>();
                }

                return menus;
            }
            return null;
        }

        #region Constants

        private const string ADMINROLE = "Administrator";

        private const string ANONROLE = "Guest";

        private const string CLIENTROLE = "Customer";

        #endregion Constants

        #region Field

        private readonly HttpClient _client;
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();
        private string _baseUrl = Properties.Settings.Default.SSDAPI + "/api/Menu";
        private ApplicationDbContext _dbcontext;
        private RoleManager<AspNetRole> _roleManager;
        private AppUserManager _userManager;

        #endregion Field
    }
}