using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class TreatPayInfo
    {
        public string treatTime { set; get; }
        public string treatId { set; get; }
        public string clinicName { set; get; }
        public double pay { set; get; }
        public bool isPay { set; get; }
        public string docName { set; get; }
    }
}