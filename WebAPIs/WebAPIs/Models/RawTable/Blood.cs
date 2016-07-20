using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.DataModels
{
    public class Blood
    {
        public string exam_id { get; set; }
        public double wbc { get; set; }
        public double neut_percent { get; set; }
        public double lymph_percent { get; set; }
        public double mono_percent { get; set; }
        public double eo_percent { get; set; }
        public double baso_percent { get; set; }
        public double neut_num { get; set; }
        public double lymph_num { get; set; }
        public double mono_num { get; set; }
        public double eo_num { get; set; }
        public double baso_num { get; set; }
        public double rbc { get; set; }
        public double hgb { get; set; }
        public double hct { get; set; }
        public double mcv { get; set; }
        public double mch { get; set; }
        public double mchc { get; set; }
        public double rdw { get; set; }
        public double plt { get; set; }
        public double mpv { get; set; }
        public double pct { get; set; }
        public double pdw { get; set; }
    }
}