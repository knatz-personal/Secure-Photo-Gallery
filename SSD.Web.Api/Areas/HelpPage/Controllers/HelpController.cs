using System;
using System.Web.Http;
using System.Web.Mvc;
using SecurityUtil;
using SSD.Web.Api.Areas.HelpPage.ModelDescriptions;
using SSD.Web.Api.Areas.HelpPage.Models;

namespace SSD.Web.Api.Areas.HelpPage.Controllers
{
    /// <summary>
    ///     The controller that will handle requests for the help page. 
    /// </summary>
    public class HelpController : Controller
    {
        public HttpConfiguration Configuration { get; private set; }

        public HelpController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public ActionResult Api(string apiId)
        {
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }

        public ActionResult Index()
        {
            string token = Session.GetData<string>("access_token");
            if (!string.IsNullOrEmpty(token))
            {
                ViewBag.Token = token;
            }
            else
            {
                ViewBag.Token = null;
            }
            ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }

        private const string ErrorViewName = "Error";
    }
}