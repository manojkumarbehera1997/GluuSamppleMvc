using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleMvcApp.ViewModels
{
    public class User
    {
    }
    public class Meta
    {
        public string resourceType { get; set; }
        public DateTime lastModified { get; set; }
        public string location { get; set; }
        public DateTime? created { get; set; }
    }



    public class Names
    {
        public string familyName { get; set; }
        public string givenName { get; set; }
        public string middleName { get; set; }
        public string formatted { get; set; }
    }



    public class Emails
    {
        public string value { get; set; }
        public bool primary { get; set; }
        public string type { get; set; }
    }



    public class Group
    {
        public string value { get; set; }
        public string display { get; set; }
        public string type { get; set; }

        public string Ref { get; set; }
    }



    public class Resource
    {
        public List<string> schemas { get; set; }
        public string id { get; set; }
        public Meta meta { get; set; }
        public string userName { get; set; }
        public Names name { get; set; }
        public string displayName { get; set; }
        public string nickName { get; set; }
        public string timezone { get; set; }
        public bool active { get; set; }
        public List<Emails> emails { get; set; }
        public List<Group> groups { get; set; }
    }



    public class UserList
    {
        public List<string> schemas { get; set; }
        public int totalResults { get; set; }
        public int startIndex { get; set; }
        public int itemsPerPage { get; set; }
        public List<Resource> Resources { get; set; }
    }





}
