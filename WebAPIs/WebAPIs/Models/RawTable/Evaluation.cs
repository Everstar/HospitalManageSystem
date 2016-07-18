using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.DataModels
{
    public class Evaluation
    {
        public string treat_id { get; set; }
        public string patient_id { get; set; }
        public string doc_id { get; set; }
        public int rank { get; set; }
        public string content { get; set;}
    }
}