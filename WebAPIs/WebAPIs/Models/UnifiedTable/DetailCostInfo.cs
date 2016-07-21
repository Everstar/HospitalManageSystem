using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class DetailCostInfo
    {
        public string costId { set; get; }
        public string docName { set; get; }
        public double cost { set; get; }
        public string startTime { set; get; }
        public bool isPay { set; get; }
    }
}