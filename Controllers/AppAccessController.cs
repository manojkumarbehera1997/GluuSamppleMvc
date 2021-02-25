using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RestSharp;
using SampleMvcApp.Models;
using SampleMvcApp.Services;
using SampleMvcApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SampleMvcApp.Controllers
{
    public class AppAccessController : Controller
    {
        private readonly AppAcessService _appAcessContext;
        public AppAccessController(AppAcessService context)
        {
            _appAcessContext = context;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Assign(int id)
        {
           
            return View();
        }
        [HttpPost]
        public IActionResult Assign(SelectedAppDetailsViewModel selectedAppDetailsViewModel)
        {
            int id = 0;
            string mergedString = String.Join(",", selectedAppDetailsViewModel.appid);    
            var client = new RestClient("https://localhost:44313/api/app/PostAssignedApps"+id+ mergedString);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return View("MyApps");
        }

        [HttpGet]
        public IActionResult GetAllApp()
        {

            return View(_appAcessContext.GetAllApp());
        }  
        public IActionResult AssignedApp(SelectedAppDetailsViewModel apps)
        {
            apps.appnames = NewApps();
            return View(apps);

        }

        [HttpGet]
        public IActionResult AssignedApp(int id)
        {
            SelectedAppDetailsViewModel apps = new SelectedAppDetailsViewModel();
            apps.appnames = NewApps();

            SelAppdDetails selAppdDetails = new SelAppdDetails();
            selAppdDetails.apps = PopulateApps(id);

            foreach(SelectListItem app in apps.appnames)
            {
                foreach(var item in selAppdDetails.apps)
                {
                    if (app.Value == item.Value)
                    {
                        app.Selected = true;
                    }
                }
            }
           


            return View(apps);
        }
        private static List<SelectListItem> NewApps()
            {
            var client = new RestClient("https://localhost:44313/api/app/getapps");
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
        private static List<SelectListItem> PopulateApps(int id)
        {
            var client = new RestClient("https://localhost:44313/api/app/getuser_assignedapps/" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            string apps = Convert.ToString(JsonConvert.DeserializeObject(response.Content));

            string[] appList = apps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in appList)
            {
               items.Add(new SelectListItem
               {
                   //Text = item.ToString(),
                    Value = item.ToString()
               });
            }
            return items;
        }
        // [Authorize]
        public IActionResult MyApps(int id)
        {
            

            var client = new RestClient("https://localhost:44313/api/AppAccess/Getuserapps_byid/" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            var model = JsonConvert.DeserializeObject<Apps[]>(response.Content);
            ViewData["Apps"] = model;
            return View();

        }
        private static List<GetAppDetails> Allapps()
        {
            var client = new RestClient("https://localhost:44351/api/app/getapps");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            var apps = JsonConvert.DeserializeObject<GetAppDetails[]>(response.Content);         
            return apps.ToList();
        }


    }
}
