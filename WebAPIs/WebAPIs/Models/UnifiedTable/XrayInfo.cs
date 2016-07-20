using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class XrayInfo
    {
        public string exam_id { get; set; }
        public string checkpoint { get; set; }
        public string from_picture { get; set; }
        public string picture { get; set; }

        public XrayInfo(string exam_id, string checkpoint,string from_picture,string picture)
        {
            this.exam_id = exam_id;
            this.checkpoint = checkpoint;
            this.from_picture = from_picture;
            this.picture = picture;
        }
    }
}