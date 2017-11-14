using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using Newtonsoft.Json;
using SecurityUtil;
using WebPortal.Models;

namespace WebPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        #region Constructor

        public MvcApplication()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion Constructor

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                Exception exception = Server.GetLastError();
                Response.Clear();

                HttpException httpException = exception as HttpException;

                var model = new ErrorLogModel
                {
                    DateAndTime = DateTime.Now,
                    ExceptionAndTrace = exception.InnerException?.Message + " " + exception.StackTrace,
                    Message = "WebPortal: " + exception.Message,
                    Username = User.Identity.Name == string.Empty ? "Guest" : User.Identity.Name
                };

                var jsonObject = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                var result = _client.PostAsync(Properties.Settings.Default.SSDAPI + "/api/ErrorLog", content).Result;

                if (!result.IsSuccessStatusCode)
                {
                    FileLog.Log(exception);
                }

                if (httpException != null)
                {
                    var statusCode = httpException.GetHttpCode();
                    Server.ClearError();

                    Response.Redirect($"/Error/{statusCode}");
                }
            }
            catch (Exception ex)
            {
                FileLog.Log(ex);
            }
        }

        protected void Application_Start()
        {
#if DEBUG
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
#endif
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string loginUrl = Properties.Settings.Default.SSDAPI + "/Token";
            var content = new FormUrlEncodedContent(new[]
               {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", "001knatz@gmail.com"),
                    new KeyValuePair<string, string>("password", "Qwerty1234[]")
                });

            var responseData = _client.PostAsync(loginUrl, content).Result;

            if (responseData == null)
            {
                throw new NullReferenceException("Global.asax>Application_Start: responseData is null");
            }

            if (!responseData.IsSuccessStatusCode) return;

            var result = responseData.Content.ReadAsStringAsync().Result;
            Dictionary<string, string> tokenDictionary =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            string token = tokenDictionary["access_token"];

            Application["access_token"] = token;
        }

        #region Fields

        private HttpClient _client;

        #endregion Fields
    }
}