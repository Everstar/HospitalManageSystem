using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIs.Providers
{
    public class Formater
    {
        public static string ToString(DateTime dt)
        {
            return dt.Year + "/" + dt.Month + "/" + dt.Day + " " +
                dt.Hour + ":" + dt.Minute + ":" + dt.Second;
        }
    }
}