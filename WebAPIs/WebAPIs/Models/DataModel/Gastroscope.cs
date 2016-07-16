using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.DataModels
{
    public class Gastroscope
    {
        public string exam_id { get; set; }
        public string from_picture { get; set; }
        public string diagnoses { get; set; }
        public string picture { get; set; }
    }
}