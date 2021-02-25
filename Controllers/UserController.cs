using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using SampleMvcApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleMvcApp.Controllers
{
    public class UserController : Controller
    {

        GetToken gettoken = new GetToken();
        string url = "https://accounts.centroxy.com/identity/restv1/scim/v2/Users/";
        public ActionResult List()
        {
            string token = gettoken.GetAccessToken();
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "04d7c49a-6341-5636-ed32-33005edd3afc");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);

            var obj = JsonConvert.DeserializeObject<UserList>(response.Content);
            List<UserList> objlst = new List<UserList>();
            objlst.Add(obj);
            return View(objlst);
        }
        public ActionResult AssignApp(string username)
        {
            TempData["username"] = username;
            return RedirectToAction("AssignApp", "Account");
        }
    }
}
