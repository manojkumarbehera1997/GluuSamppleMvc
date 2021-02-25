using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleMvcApp.ViewModels
{


    public class AllApps
    {
       public string appname { get; set; }
        public string appid { get; set; }




    }
    
    public class SelectedAppViewModel
    {
        public AllApps App { get; set; }
        public List<int> SelectedAppId { get; set; }
        public List<SelectListItem> AppList { get; set; }
    }


}
