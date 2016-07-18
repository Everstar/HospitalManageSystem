using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.DataModels
{
    public class Treatment
    {
        public string treat_id { get; set; }
        public string clinic { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public string doc_id { get; set; }
        public double pay { get; set; }
        public DateTime pay_time { get; set; }

    }
}