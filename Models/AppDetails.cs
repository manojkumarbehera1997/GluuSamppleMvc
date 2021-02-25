using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleMvcApp.Models
{
    public class AppDetails
    {
        public int userid { get; set; }
        public string appname { get; set; }
        public int appid { get; set; }
    }
    public class SelectedAppDetailsViewModel
    {
        public List<SelectListItem> appnames { get; set; }
        public int[] appid { get; set; }
    }
    public class SelAppdDetails
    {
        public List<SelectListItem> apps { get; set; }
        public int[] id { get; set; }
    }
    public class GetAppDetails
    {
        public int appid { get; set; }
        public string appname { get; set; }
        public string appicon { get; set; }
        public string appurl { get; set; }
    }
}
