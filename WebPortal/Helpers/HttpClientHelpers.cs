using System;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.SessionState;

namespace WebPortal.Helpers
{
    public static class HttpClientHelpers
    {
        public static HttpClient GetApiClient(string baseApiUrl)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(baseApiUrl)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                //Token Authentication
                string token = HttpContext.Current.Application["access_token"].ToString();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            catch (Exception)
            {
            }
            return client;
        }
    }
}