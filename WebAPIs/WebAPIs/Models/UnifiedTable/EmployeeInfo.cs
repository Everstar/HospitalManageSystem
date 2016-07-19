using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class EmployeeInfo
    {
        public string employee_id { get; set; }
        public string credit_num { get; set; }
        public string password { get; set; }
        public string department { get; set; }
        public string clinic { get; set; }
        public string post { get; set; }
        public double salary { get; set; }
        public string duty_id { get; set; }
        public string name { get; set; }
        public char sex { get; set; }
        public DateTime birth { get; set; }

        public EmployeeInfo(string id, string num, string pw, string dm, string cn,
            string post, double salary, string name, string sex, DateTime birth)
        {
            employee_id = id;
            credit_num = num;
            password = pw;
            department = dm;
            clinic = cn;
            this.post = post;
            this.salary = salary;
            this.name = name;
            this.sex = sex[0];
            this.birth = birth;
        }

        public EmployeeInfo(string dept_name, string clinic_name, string post, string name, string sex)
        {
            this.department = dept_name;
            this.clinic = clinic_name;
            this.post = post;
            this.name = name;
            this.sex = sex[0];
        }
        public EmployeeInfo(string employee_id ,string dept_name, string clinic_name, string post, string name, string sex)
        {
            this.employee_id = employee_id;
            this.department = dept_name;
            this.clinic = clinic_name;
            this.post = post;
            this.name = name;
            this.sex = sex[0];
        }
    }
}