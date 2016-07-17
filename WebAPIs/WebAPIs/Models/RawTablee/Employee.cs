using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.DataModels
{
    public class Employee
    {
        public string employee_id { get; set; }
        public string credit_num { get; set; }
        public string password { get; set; }
        public string department { get; set; }
        public string clinic { get; set; }
        public string post { get; set; }
        public double salary { get; set; }
        public string duty_id { get; set; }

    }
}