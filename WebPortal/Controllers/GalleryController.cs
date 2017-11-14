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
using PagedList;
using SharedResources;
using WebPortal.Helpers;
using WebPortal.Models;

namespace WebPortal.Controllers
{
    public class GalleryController : Controller
    {
        #region Properties

        public RoleManager<IdentityRole> GetRoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<RoleManager<IdentityRole>>();
            }
        }

        public AppUserManager UserManager => _userManager ?? Request.GetOwinContext().GetUserManager<AppUserManager>();

        #endregion Properties

        #region Field

        private readonly HttpClient _client;
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();
        private ApplicationDbContext _dbcontext;
        private RoleManager<IdentityRole> _roleManager;
        private AppUserManager _userManager;

        #endregion Field

        #region Constructor

        public GalleryController()
        {
            _dbcontext = new ApplicationDbContext();
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dbcontext));

            _client = HttpClientHelpers.GetApiClient(Properties.Settings.Default.SSDAPI);
        }

        #endregion Constructor

        [HttpGet]
        public ActionResult Index(string q, int pg = 1, int ps = 12)
        {
            using (_client)
            {
                string url = string.Format("api/Gallery/{0}", User.Identity.GetUserId());
                HttpResponseMessage responseMessage = _client.GetAsync(url).Result;

                if (responseMessage != null && responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                    var list = _serializer.Deserialize<List<GalleryModel>>(responseData);

                    if (list != null)
                    {
                        if (!string.IsNullOrEmpty(q))
                        {
                            list = list.Where(s => s.Album.Title.ToLower().Contains(q.ToLower())
                            || s.Album.Description.ToLower().Contains(q.ToLower())).ToList();
                        }

                        return View("Index", list.ToPagedList(pg, ps));
                    }
                }
                return View();
            }
        }
    }
}