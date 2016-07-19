using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.DataModels
{
    public class Prescription
    {
        public string pres_id { get; set; }
        public string treat_id { get; set; }
        public string employee_id { get; set; }
        public DateTime make_time { get; set;}
        public DateTime done_time { get; set; }
        public double pay { get; set; }
        public DateTime pay_time { get; set; }

    }
}