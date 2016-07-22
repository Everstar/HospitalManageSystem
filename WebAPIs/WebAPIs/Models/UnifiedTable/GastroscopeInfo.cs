using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class GastroscopeInfo
    {
        static int num = 1;
        public string exam_id { get; set; }
        public string from_picture { get; set; }
        public string diagnoses { get; set; }
        public string picture { get; set; }

        public GastroscopeInfo(string exam, string from_picture,string diagnoses,string picture)
        {
            this.exam_id = exam;
            this.from_picture = from_picture;
            this.diagnoses = diagnoses;
            this.picture = picture;
        }
    }
}