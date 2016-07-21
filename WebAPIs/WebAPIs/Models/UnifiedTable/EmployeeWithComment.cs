using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class EmployeeWithComment
    {
        public string pic_url { set; get; }
        public string name { set; get; }
        public string sex { set; get; }
        public string clinic { set; get; }
        public string post { set; get; }
        public string profile { set; get; }
        public ArrayList comment { set; get; }
    }
}