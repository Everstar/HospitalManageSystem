using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.DataModels
{
    public class Surgery
    {
        public string surg_id { get; set; }
        public string treat_id { get; set; }
        public string surgery_name { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public double pay { get; set; }
        public DateTime pay_time { get; set;}
    }
}