using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleMvcApp.ViewModels
{
    public class ConsulConfiguration
    {
        public OIDC OIDC { get; set; }
        public Session Session { get; set; }
    }
    public class OIDC
    {
        public string Domain { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CustomPath { get; set; }
        public string CallbackPath { get; set; }
        public string RedirectUri { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public string Protocol { get; set; }
    }
    public class Session
    {
        public string TimeOut { get; set; }
    }


}
