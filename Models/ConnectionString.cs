using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;


namespace AppAccessAPI.Repository
{
    public class ConnectionString : IDisposable
    {
        protected IDbConnection connectionstring;
        //private readonly IConfigurationRoot Configuration;
        private readonly IConfiguration Configuration;

        public ConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            var constr = $"{Configuration["ConnectionStrings:DefaultConnection"]}";

            connectionstring = new MySql.Data.MySqlClient.MySqlConnection(constr);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
