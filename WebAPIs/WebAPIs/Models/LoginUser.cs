using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using WebAPIs.Providers;

namespace WebAPIs.Models
{
    [Serializable]
    public class LoginUser
    {
        public string account { get; set; }
        public string passwd { get; set; }
        public override string ToString()
        {
            return JsonObjectConverter.ObjectToJson(this);
        }
    }
}