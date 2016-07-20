using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class AllPrescription
    {
        public string pres_id { get; set; }
        public string patientname { get; set;}
        public string sex { get; set; }
       


        public AllPrescription(string pres_id,string sex,string patient)
        {
            this.pres_id = pres_id;
            this.patientname = patient;
            this.sex = sex;
           
        }

    }
}