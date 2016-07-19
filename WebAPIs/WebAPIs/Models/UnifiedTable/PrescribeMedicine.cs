using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class PrescribeMedicine
    {
        public string medicine_id { get; set; }
        public string unit { get; set; }
        public int num { get; set; }
        public string name { get; set; }


        public PrescribeMedicine(string medicine_id,string unit,int num,string name)
        {
            this.medicine_id = medicine_id;
            this.unit = unit;
            this.num = num;
            this.name = name;
        }
    }
}