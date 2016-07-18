using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class ExaminationInfo
    {
        public string exam_id { get; set; }
        public string type { get; set; }
        public string exam_time { get; set; }
        public string doc_id { get; set; }
        public double pay { get; set; }
        public DateTime pay_time { get; set; }

        public ExaminationInfo(string exam_id,string type,string exam_time,string doc_id,double pay,DateTime pay_time)
        {
            this.exam_id = exam_id;
            this.type = type;
            this.exam_time = exam_time;
            this.doc_id = doc_id;
            this.pay = pay;
            this.pay_time = pay_time;
        }
    }
}