using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.DataModels
{
    public class Examination
    {
        public string exam_id { get; set; }
        public string type { get; set; }
        public string exam_time { get; set; }
        public string doc_id { get; set; }
        public double pay { get; set; }
        public DateTime pay_time { get; set; }
    }
}