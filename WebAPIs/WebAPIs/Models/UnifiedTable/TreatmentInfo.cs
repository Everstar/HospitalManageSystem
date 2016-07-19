using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class TreatmentInfo
    {
        public int treat_id { get; set; }
        public int patient_id { get; set; }
        public string patient_name { get; set; }

        public TreatmentInfo(int treat_id,int patient_id,string patient_name)
        {
            this.treat_id = treat_id;
            this.patient_id = patient_id;
            this.patient_name = patient_name;
        }
    }
}