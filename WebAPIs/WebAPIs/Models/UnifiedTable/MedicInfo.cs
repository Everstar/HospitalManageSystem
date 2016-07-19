using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class MedicInfo
    {
        public string name { get; set; }      
        public string num { get; set; }
        public string unit { get; set; }
        public MedicInfo(string name,string num,string unit)
        {
            this.name = name;
            this.num = num;
            this.unit = unit;
        }
    }
}