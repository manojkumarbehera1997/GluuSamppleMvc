using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMvcApp.ViewModels
{
    public static class ConsulExtensions
    {
        static ConsulConfiguration consul_configdata;
        
        public static ConsulConfiguration FetchConsul(this IServiceCollection services, string host,string oidcKey)
        {
            ReadConsul(host, oidcKey);
            return consul_configdata;
        }
        public static ConsulConfiguration FetchConsul(string host, string oidcKey)
        {
            ReadConsul(host, oidcKey);
            return consul_configdata;
        }
        private static void ReadConsul(string host, string key)
        {
            using (var client = new ConsulClient(clientConfig => new Uri(host)))
            {
                var getPair = client.KV.Get(key);
                Log.Debug("Consul getPair status:{STATUS}", getPair.Result.StatusCode);
                if (getPair.Result.Response != null)
                {
                    var settings = Encoding.UTF8.GetString(getPair.Result.Response.Value, 0, getPair.Result.Response.Value.Length);
                    consul_configdata = JsonConvert.DeserializeObject<ConsulConfiguration>(settings);

                }
            }
        }
    }
}
