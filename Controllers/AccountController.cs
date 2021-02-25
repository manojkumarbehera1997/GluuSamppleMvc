using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SampleMvcApp.ViewModels;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization.Internal;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Collections.Generic;
using Consul;
using Serilog;
using System.Text;
using Newtonsoft.Json;
using SampleMvcApp.Controllers;
using RestSharp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SampleMvcApp.Controllers
{
    public class AccountController : Controller
    {
        private IConfiguration configuration;
        private IOptions<ConfigData> _configData;
        ConsulConfiguration getOIDCConfig = new ConsulConfiguration();

        public AccountController(IConfiguration iConfig, IOptions<ConfigData> configData)
        {
            configuration = iConfig;
            _configData = configData;
        }

        public async Task Login(string returnUrl = "/Account/MyApps", string ui_locales = "")
        {
            HttpContext.Request.Headers.Add("ui_locales", ui_locales);
            await HttpContext.ChallengeAsync("oidc", new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        public async Task SessionOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        }

        [Authorize]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("oidc", new AuthenticationProperties
            {
                // Indicate here where iCrypto should redirect the user after a logout.
                // Note that the resulting absolute Uri must be whitelisted in the 
                // **Allowed Logout URLs** settings for the client.
                RedirectUri = Url.Action("Index", "Home")
            });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [Authorize]
        public async Task BackChannelLogout()
        {
            getOIDCConfig = ConsulExtensions.FetchConsul(configuration["Consul:Host"], configuration["Consul:Key"]);
            string session_id = "";
            var claim_sessionid = User.Claims.Where(x => x.Type == "session_id").FirstOrDefault();
            if (claim_sessionid != null)
            {
                session_id = claim_sessionid.Value;
            }


            try
            {
                var request = HttpContext.Request;
                var postLogoutRedirectUri = getOIDCConfig.OIDC.Protocol + "://" + request.Host + request.PathBase + getOIDCConfig.OIDC.PostLogoutRedirectUri;

                HttpClient client = new HttpClient();

                var responseString = client.GetStringAsync($"https://" + getOIDCConfig.OIDC.Domain + "/" + getOIDCConfig.OIDC.CustomPath + "/restv1/end_session?session_id=" +
                    session_id + $"&post_logout_redirect_uri = { Uri.EscapeDataString(postLogoutRedirectUri) }");

            }
            catch (Exception ex)
            {

            }
            finally
            {
                HttpContext.Session.Clear();
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [Authorize]
        public ActionResult BCLogout()
        {
            BackChannelLogout();

            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// This is just a helper action to enable you to easily see all claims related to a user. It helps when debugging your
        /// application to see the in claims populated from the iCrypto ID Token
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Claims()
        {
            var addList = new List<string> { "tokenInfo" };
            var claims = User.Claims.GroupBy(x => x.Type).Select(x => x.First());
            var AccessInfo = claims;
            var access_info = "";
            AccessInfo = claims.Where(x => addList.Contains(x.Type));
            foreach (var claimsToken in AccessInfo)
            {
                access_info = claimsToken.Value;

            }
            OIDCTokenResponse Atoken = Newtonsoft.Json.JsonConvert.DeserializeObject<OIDCTokenResponse>(access_info);
            ViewData["Message"] = Atoken;
            return View();
        }
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
        //[Authorize]
        //public IActionResult AssignApp()
        //{
        //    return View();
        //}

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Error(string error, string error_description)
        {
            ViewBag.Error = error;
            ViewBag.ErrorDescription = error_description;
            return View();
        }
        // apps which are already assigned to user
        [Authorize]
        public IActionResult MyApps()
        {
            string uname = "";
            foreach (var claim in User.Claims)
            {
                if (claim.Type == "name")
                {
                    uname = claim.Value;
                    break;
                }
            }

            var client = new RestClient("https://localhost:44313/api/app/getappbyuser/" + uname);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            var model = JsonConvert.DeserializeObject<Apps[]>(response.Content);
            ViewData["Apps"] = model;
            return View();

        }
        public IActionResult AssignApp()
        {
            string uname = "";
            foreach (var claim in User.Claims)
            {
                if (claim.Type == "name")
                {
                    uname = claim.Value;
                    break;
                }
            }
            SelectedAppViewModel myapps = new SelectedAppViewModel();
            myapps.AppList = PopulateApps(uname);

            string username = (string)TempData["username"];
            TempData["User"] = username;
            return View(myapps);
           

        }
        [HttpPost]
        public IActionResult AssignApp(SelectedAppViewModel apps)
        {
            string uname = "";
            foreach (var claim in User.Claims)
            {
                if (claim.Type == "name")
                {
                    uname = claim.Value;
                    break;
                }
            }
            apps.AppList = PopulateApps(uname);
            if (apps.SelectedAppId != null)
            {
                var selectedItems = apps.AppList.Where(p => apps.SelectedAppId.Contains(int.Parse(p.Value))).ToList();

                ViewBag.Message = "Selected Apps:";
                foreach (var selectedItem in selectedItems)
                {
                    ViewBag.Message += "\\n" + selectedItem.Text;
                }
            }

            return View(apps);
        }
        private static List<SelectListItem> PopulateApps(string uname)
        {
            var client = new RestClient("https://localhost:44313/api/app/newapps/" + uname);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            var apps = JsonConvert.DeserializeObject<AllApps[]>(response.Content).ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in apps)
            {
                items.Add(new SelectListItem
                {
                    Text = item.appname.ToString(),
                    Value = item.appid.ToString()
                });
            }
            return items;
        }
        public JsonResult GetApps()
        {
            string name = (string)TempData["User"];
            try
            {
                var client = new RestClient("https://localhost:44313/api/app/getappbyuser/" + name);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                var model = JsonConvert.DeserializeObject<Apps[]>(response.Content);
                return Json(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IActionResult NewApp(SelectedAppViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var apps = string.Join(",", model.SelectedAppId);
                    model.App.appid = apps;


                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public IActionResult test()
        {
            return View();
        }
        public IActionResult index()
        {
            return View();
        }
        #region Helpers

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public IActionResult CustomAuth()
        {
            return View();
        }
        #endregion
    }
}
