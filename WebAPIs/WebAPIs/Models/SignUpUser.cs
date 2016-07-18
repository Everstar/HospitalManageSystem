using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models
{
    public class SignUpUser
    {
        public string name { get; set; }
        public string sex { get; set; }
        public string credit_num { get; set; }
        public DateTime birth { get; set; }
        public string passwd { get; set; }
    }
}