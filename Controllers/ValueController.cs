using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SampleMvcApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("catch")]
        public string Get()
        {
            return "User is now authorized";
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("try")]
        public string Set()
        {
            return "Admin is now authorized";
          
        }
       
    }
}
