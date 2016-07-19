using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models
{
    public class PartXRayInfo
    {
        public string checkpoint { get; set; }
        //从图片得出的结论
        public string from_picture { get; set; } 
        //图片路径      
        public string picture { get; set; }
    }
}