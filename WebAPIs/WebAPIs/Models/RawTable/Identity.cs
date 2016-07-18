using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.DataModels
{
    public class Identity
    {
        public string credit_num { get; set; }
        public string name { get; set; }
        public char sex { get; set; }
        public DateTime birth { get; set; }

    }
}