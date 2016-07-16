using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.DataModels
{
    public class Prescribe
    {
        public string pres_id { get; set; }
        public string medicine_id { get; set; }
        public int num { get; set; }
        public string unit { get; set; }

    }
}