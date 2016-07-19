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
        public DateTime exam_time { get; set; }
        public string employee_id { get; set; }
        public double pay { get; set; }
        public DateTime pay_time { get; set; }

        public Examination(string exam_id, string type, DateTime exam_time, string doc_id, double pay, DateTime pay_time)
        {
            this.exam_id = exam_id;
            this.type = type;
            this.exam_time = exam_time;
            this.employee_id = doc_id;
            this.pay = pay;
            this.pay_time = pay_time;
        }
    }
}