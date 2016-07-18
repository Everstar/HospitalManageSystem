using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class BloodInfo
    {
        const string head = "1234567890";
        static int num = 1;
        public string exam_id;
        public double wbc;
        public double neut_percent;
        public double lymph_percent;
        public double mono_percent;
        public double eo_percent;
        public double baso_percent;
        public double neut_num;
        public double lymph_num;
        public double mono_num;
        public double eo_num;
        public double baso_num;
        public double rbc;
        public double hgb;
        public double hct;
        public double mcv;
        public double mch;
        public double mchc;
        public double rdw_cv;
        public double rdw_sd;
        public double plt;
        public double mpv;
        public double pdw;
        public double pct;

        //后面还有好多项，之后确定无误再写~~~
        public BloodInfo(double wbc)
        {
            this.exam_id = head + num.ToString();
            num++;
            this.wbc = wbc;
        }
    }
}