using System.Web;
using System.Web.Mvc;

namespace WebPortal.Filters
{
    public class SessionExpireRedirectAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            if (!ctx.User.Identity.IsAuthenticated)
            {
                filterContext.HttpContext.Response.Redirect("~/Account/Login", true);
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}