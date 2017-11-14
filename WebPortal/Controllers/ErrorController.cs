using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPortal.Models;

namespace WebPortal.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        // GET: Error
        [Route("Error/{code}")]
        public ActionResult ErrorRedirect(string code)
        {
            return View("ErrorRedirect", new ErrorModel() { Code = code });
        }
    }
}