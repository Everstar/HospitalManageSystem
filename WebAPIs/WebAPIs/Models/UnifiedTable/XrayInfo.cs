using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class XrayInfo
    {
        const string head = "1234567890";//这里最后要改！！！自动加一
        static int num = 1;
        public string exam_id { get; set; }
        public string checkpoint { get; set; }
        public string from_picture { get; set; }
        public string picture { get; set; }

        public XrayInfo(string checkpoint,string from_picture,string picture)
        {
            this.exam_id = head + num.ToString();
            num++;
            this.checkpoint = checkpoint;
            this.from_picture = from_picture;
            this.picture = picture;
        }
    }
}