using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class PatientInfo
    {
        public string patient_id { get; set; }
        public string credit_num { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public char sex { get; set; }
        public DateTime birth { get; set; }

        public PatientInfo(string id, string num, string pw, string name, string sex, DateTime birth)
        {
            patient_id = id;
            credit_num = num;
            password = pw;
            this.name = name;
            this.sex = sex[0];
            this.birth = birth;
        }
    }
}