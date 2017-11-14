using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using SecurityUtil;
using SSD.Web.Api.Models;

namespace SSD.Web.Api.Controllers
{
    [AllowAnonymous]
    public class ConsumerController : Controller
    {
        public ConsumerController()
        {
            _serializer = new JavaScriptSerializer();
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var url = Properties.Settings.Default.APIURL + "/Token";

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", model.UserName),
                    new KeyValuePair<string, string>("password", model.Password)
                });

                var responseMessage = _client.PostAsync(url, content);

                if (responseMessage == null)
                {
                    throw new NullReferenceException("ConsumerController>Login: responseMessage is null");
                }

                var responseData = responseMessage.Result;

                if (responseData != null)
                {
                    if (responseData.IsSuccessStatusCode)
                    {
                        var result = responseData.Content.ReadAsStringAsync().Result;
                        Dictionary<string, string> tokenDictionary =
                           JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                        string token = tokenDictionary["access_token"];

                        Session.Add("access_token", token);
                        Session.Add("userName", tokenDictionary["userName"]);
                        Session.Add("IsAuthenticated", true);

                        return RedirectToAction("Index", "Help");
                    }

                    ModelState.AddModelError("", responseData.ReasonPhrase);
                    ModelState.AddModelError("", "Invalid Credentials.");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LogOut()
        {
            var url = Properties.Settings.Default.APIURL + "/api/Account/Logout";
            var responseMessage = await _client.PostAsync(url, null);

            if (responseMessage == null)
            {
                throw new NullReferenceException("ConsumerController>LogOut: responseMessage is null");
            }

            if (responseMessage.IsSuccessStatusCode)
            {
                Session.Remove("access_token");
                Session.Remove("userName");
                Session.Remove("IsAuthenticated");

                return RedirectToAction("Login", "Consumer");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Consumer
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var url = Properties.Settings.Default.APIURL + "/api" + Url.Action("Register", "Account");

                var postbody = _serializer.Serialize(model);
                var responseMessage = _client.PostAsync(url, new StringContent(postbody, Encoding.UTF8, "application/json"));

                if (responseMessage == null)
                {
                    throw new NullReferenceException("ConsumerController>Register: responseMessage is null");
                }

                var responseData = responseMessage.Result;

                if (responseData != null)
                {
                    if (responseData.IsSuccessStatusCode)
                    {
                        string authbody = $"token?username={model.Email}&password={model.Password}&granttype=token";
                        var token = _client.GetAsync(authbody);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        var anonymousErrorObject = new { message = "", ModelState = new Dictionary<string, string[]>() };
                        var httpErrorObject = responseData.Content.ReadAsStringAsync().Result;
                        var deserializedErrorObject = JsonConvert.DeserializeAnonymousType(httpErrorObject,
                            anonymousErrorObject);
                        if (deserializedErrorObject.ModelState == null)
                        {
                            ModelState.AddModelError(responseData.StatusCode.ToString(),
                                "An unexpected error has occurred");
                        }
                        else
                        {
                            foreach (var err in deserializedErrorObject.ModelState.Values)
                            {
                                foreach (var m in err)
                                {
                                    ModelState.AddModelError(responseData.StatusCode.ToString(), m);
                                }
                            }
                        }
                    }
                }
            }
            return View();
        }

        private HttpClient _client;
        private JavaScriptSerializer _serializer;
    }
}