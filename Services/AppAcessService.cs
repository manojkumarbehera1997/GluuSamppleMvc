using AppAccessAPI.Repository;
using Dapper;
using SampleMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Data.CommandType;
using System.Threading.Tasks;

namespace SampleMvcApp.Services
{
    public class AppAcessService: ConnectionString
    {
        public List<Users> GetAllApp()
        {
            DynamicParameters parameters = new DynamicParameters();
            List<Users> apps = (SqlMapper.Query<Users>(connectionstring, "DispUsers", parameters, commandType: StoredProcedure)).ToList();
            return apps.ToList();
        }       

    }
}
