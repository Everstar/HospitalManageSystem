using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class PrescriptionInfo
    {
        public string pres_id { get; set; }
        public string treat_id { get; set; }
        public DateTime make_time { get; set; }
        public DateTime done_time { get; set; }
        public double pay { get; set; }
        public DateTime pay_time { get; set; }

        public PrescriptionInfo(string pres_id,string treat_id ,DateTime make_time ,DateTime done_time , double pay, DateTime pay_time)
        {
            this.pres_id = pres_id;
            this.treat_id = treat_id;
            this.pay = pay;
            this.make_time = make_time;
            this.done_time = done_time;
            this.pay_time = pay_time;
        }
    }
}