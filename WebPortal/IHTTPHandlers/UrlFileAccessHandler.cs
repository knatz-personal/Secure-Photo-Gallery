using System.Net.Http;
using System.Web;
using WebPortal.Helpers;

namespace WebPortal.IHTTPHandlers
{
    public class UrlFileAccessHandler : IHttpHandler
    {
        /// <summary>
        ///     Gets a value indicating whether another request can use the
        ///     <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        /// <returns>
        ///     true if the <see cref="T:System.Web.IHttpHandler" /> instance is
        ///     reusable; otherwise, false.
        /// </returns>
        public bool IsReusable => true;

        public UrlFileAccessHandler()
        {
            _client = HttpClientHelpers.GetApiClient(Properties.Settings.Default.SSDAPI);
        }

        /// <summary>
        ///     Enables processing of HTTP Web requests by a custom HttpHandler
        ///     that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">
        ///     An <see cref="T:System.Web.HttpContext" /> object that provides
        ///     references to the intrinsic server objects (for example, Request,
        ///     Response, Session, and Server) used to service HTTP requests.
        /// </param>
        public void ProcessRequest(HttpContext context)
        {
            string urlRequested = HttpUtility.UrlDecode(context.Request.Url.AbsolutePath);

            if (!context.User.Identity.IsAuthenticated)
            {
                context.Response.Redirect("/Error/403");
            }

            if (context.User.IsInRole("Administrator"))
            {
                TransmitRequestedFile(context, urlRequested);
            }

            if (context.User.IsInRole("Customer"))
            {
                string filename = urlRequested.Substring(9);
                HttpResponseMessage responseMessage =
                    _client.GetAsync($"api/CanAccessImage?filename={filename}&userId={context.User.Identity.Name}")
                        .Result;

                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    bool isValidRequest;
                    bool.TryParse(responseData, out isValidRequest);

                    if (isValidRequest)
                    {
                        TransmitRequestedFile(context, urlRequested);
                    }
                }
                else
                {
                    context.Response.Redirect("/Error/403");
                }
            }
        }

        private HttpClient _client;

        private void TransmitRequestedFile(HttpContext context, string urlRequested)
        {
            context.Response.TransmitFile(urlRequested);
        }
    }
}