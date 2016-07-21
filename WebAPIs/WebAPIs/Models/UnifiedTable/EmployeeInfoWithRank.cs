using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class EmployeeInfoWithRank
    {
        public string pic_url { get; set; }
        public string employee_id { get; set; }
        public string name { get; set; }
        public string post { get; set; }
        public string skill { get; set; }
        public int rank { get; set; }
    }
}