using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class HospitalInfo
    {
        public string hos_id { get; set; }
        public string treat_id { get; set; }
        public string employee_id { get; set; }
        public string bed_num { get; set; }
        public double pay { get; set; }
        public int rank { get; set; }
        public DateTime in_time { get; set; }
        public DateTime out_time { get; set; }
        public DateTime pay_time { get; set; }
        

        public HospitalInfo(string hos,string treat,string nurse,string bed, double pay, int rank,DateTime in_time)
        {
            this.hos_id = hos;
            this.treat_id = treat;
            this.employee_id = nurse;
            this.bed_num = bed;
            this.in_time = in_time;
            this.pay = pay;
            this.rank = rank;
            this.bed_num = bed_num;
        }
    }
}