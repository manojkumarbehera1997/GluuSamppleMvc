using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SampleMvcApp.ViewModels;

namespace SampleMvcApp.ViewModels
{
    public class GetToken
    {
       
        public string GetAccessToken()
        {

            var client = new RestClient("https://accounts.centroxy.com/oxauth/restv1/token");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic MDc5ODY0MDYtMmIxNS00ZjhhLWFmOWItMjk5MmVlZjcyNTIxOndmYXNvdFdHdnJKTUZhckxpSVdwck00WER2OFo0MGRKc0NDZGJNbnE=");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Cookie", "org.gluu.i18n.Locale=en");
            request.AddParameter("grant_type", "client_credentials");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            AccessToken accesstoken   = JsonConvert.DeserializeObject<AccessToken>(response.Content);
            //return View();
            string token = accesstoken.access_token;
            return token;
        }
    }
}
