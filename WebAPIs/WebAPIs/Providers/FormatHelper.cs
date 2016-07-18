using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Providers
{
    public class FormatHelper
    {
        public static string GetYMD()
        {
            string ymd = "";
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day  =DateTime.Now.Day;
            if (year%100 < 10)
                ymd += '0';
            ymd += year.ToString();
            if (month < 10)
                ymd += '0';
            ymd += month.ToString();
            if (day < 10)
                ymd += '0';
            ymd += day.ToString();
            return ymd;
        }
        public static string GetYear()
        {
            return GetYMD().Substring(0, 2);
        }

        public static string GetIDNum(int cnt)
        {
            if (cnt > 100) return cnt.ToString();
            else if (cnt > 10) return "0" + cnt.ToString();
            else return "00" + cnt.ToString();
        }
    }
}