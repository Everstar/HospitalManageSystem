using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.DataModels
{
    public class Hospitalization
    {
        public string hos_id { get; set; }
        public string treat_id { get; set; }
        public string nurse_id { get; set; }
        public string bed_num { get; set; }
        public DateTime in_time { get; set; }
        public DateTime out_time { get; set; }
        public double pay { get; set; }
        public DateTime pay_time { get; set; }

    }
}