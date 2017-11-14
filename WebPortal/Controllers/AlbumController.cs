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
using WebPortal.Models;
using PagedList;
using WebPortal.Helpers;

namespace WebPortal.Controllers
{
    public class AlbumController : Controller
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

        public AlbumController()
        {
            _dbcontext = new ApplicationDbContext();
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dbcontext));
            _client = HttpClientHelpers.GetApiClient(Properties.Settings.Default.SSDAPI);
        }

        #endregion Constructor

        public PartialViewResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        public ActionResult Create(AlbumCreateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_client)
                    {
                        var date = DateTime.Now;
                        model.CreationDate = date;
                        model.UserId = User.Identity.GetUserId();

                        HttpResponseMessage responseMessage = _client.PostAsJsonAsync("api/Album", model).Result;
                        if (responseMessage != null && responseMessage.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index", "Gallery");
                        }
                        ModelState.AddModelError(string.Empty, "Failed to create new album");
                        return PartialView("_Create", model);
                    }
                }
            }
            catch
            {
                return PartialView("_Create");
            }
            return Create();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Open(string id, string q, int pg = 1, int ps = 12)
        {
            using (_client)
            {
                HttpResponseMessage responseMessage = _client.GetAsync("/api/My/Images?albumId=" + id).Result;

                if (responseMessage != null && responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                    ImageViewModel list = _serializer.Deserialize<ImageViewModel>(responseData);

                    if (list != null)
                    {
                        if (!string.IsNullOrEmpty(q))
                        {
                            list.ImagePagedList = list.Images.Where(s => s.Title.ToLower().Contains(q.ToLower())
                            || s.Description.ToLower().Contains(q.ToLower())).ToPagedList(pg, ps);
                        }
                        else
                        {
                            list.ImagePagedList = list.Images.ToPagedList(pg, ps);
                        }
                        return View("Open", list);
                    }
                }
                return RedirectToAction("ErrorRedirect", "Error", new { code = "400" });
            }
        }
    }
}