using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.DataModels
{
    public class XRay
    {
        public string exam_id { get; set; }
        public string checkpoint { get; set; }
        public string from_picture { get; set; }
        public string picture { get; set; }

    }
}