using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models
{
    [Serializable]
    public class LoginUser
    {
        public string account { get; set; }
        public string passwd { get; set; }
    }
}