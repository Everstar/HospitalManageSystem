using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.DataModels
{
    public class Medicine
    {
        public string medicine_id { get; set; }
        public string name { get; set; }
        public string unit { get; set; }
        public int stock { get; set; }

    }
}