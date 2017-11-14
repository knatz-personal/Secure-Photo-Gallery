using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SecurityUtil;
using SecurityUtil.Keys;

namespace WebPortal.Helpers
{
    public static class UrlHelpers
    {
        public static MvcHtmlString EncryptedActionLink(this HtmlHelper htmlHelper, string linkText, string Action,
            string ControllerName, string Area, object routeValues, object htmlAttributes)
        {
            string queryString = string.Empty;
            string htmlAttributesString = string.Empty;
            SymmetricParameters symmparam = HttpContext.Current.Session["SymmetricParameters"] as SymmetricParameters;
            bool IsRoute = false;
            if (routeValues != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(routeValues);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    //My Changes
                    if (!d.Keys.Contains("IsRoute"))
                    {
                        if (i > 0)
                        {
                            queryString += "?";
                        }
                        queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                    }
                    else
                    {
                        //My Changes
                        if (!d.Keys.ElementAt(i).Contains("IsRoute"))
                        {
                            queryString += d.Values.ElementAt(i);
                            IsRoute = true;
                        }
                    }
                }
            }

            if (htmlAttributes != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    htmlAttributesString += " " + d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                }
            }

            StringBuilder ancor = new StringBuilder();
            ancor.Append("<a ");
            if (htmlAttributesString != string.Empty)
            {
                ancor.Append(htmlAttributesString);
            }
            ancor.Append(" href='");
            if (Area != string.Empty)
            {
                ancor.Append("/" + Area);
            }

            if (ControllerName != string.Empty)
            {
                ancor.Append("/" + ControllerName);
            }

            if (Action != "Index")
            {
                ancor.Append("/" + Action);
            }

            if (queryString != string.Empty)
            {
                if (IsRoute == false)
                {
                    ancor.Append("?q=" + EncryptionUtil.Encrypt(symmparam, queryString));
                }
                else
                {
                    ancor.Append("/" + EncryptionUtil.Encrypt(symmparam, queryString));
                }
            }
            ancor.Append("'");
            ancor.Append(">");
            ancor.Append(linkText);
            ancor.Append("</a>");
            return new MvcHtmlString(ancor.ToString());
        }
    }
}