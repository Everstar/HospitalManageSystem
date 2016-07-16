using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.DataModels
{
    public class Duty
    {
        public string duty_id { get; set; }
        public string room_num { get; set; }
        public int max_limit { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
    }
}