using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using SampleMvcApp.ViewModels;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using RestSharp;
using Newtonsoft.Json;

namespace SampleMvcApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewData["CurrentCulture"] = System.Threading.Thread.CurrentThread.CurrentCulture;
            ViewData["CurrentUICulture"] = System.Threading.Thread.CurrentThread.CurrentUICulture;

            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }

       
    }
}
