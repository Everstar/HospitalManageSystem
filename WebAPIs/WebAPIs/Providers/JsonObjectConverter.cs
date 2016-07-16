using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WebAPIs.Providers
{
    public class JsonObjectConverter
    {
        public static string ObjectToJson(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}