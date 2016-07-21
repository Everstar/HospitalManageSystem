using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Models.UnifiedTable
{
    public class DutyByClinic
    {
        public string employee_id { get; set; }
        public string name { get; set; }
        public string room_num { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }

        public DutyByClinic(string employee_id,string name,string room_num, 
            string mon, string tue, string wed, string thu, string fri, string sat, string sun)
        {
            this.employee_id = employee_id;
            this.name = name;
            this.room_num = room_num;
            this.Monday = mon;
            this.Tuesday = tue;
            this.Wednesday = wed;
            this.Thursday = thu;
            this.Friday = fri;
            this.Saturday = sat;
            this.Sunday = sun;
        }
    }
}