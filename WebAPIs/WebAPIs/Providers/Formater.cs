using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace WebAPIs.Providers
{
    public class Formater
    {
        public static string ToString(DateTime dt)
        {
            return dt.Year + "/" + dt.Month + "/" + dt.Day + " " +
                dt.Hour + ":" + dt.Minute + ":" + dt.Second;
        }

        public static DateTime ToDateTime(OracleDataReader reader, int index)
        {
            DateTime dt = new DateTime();
            if (!reader[index].ToString().Equals(""))
            {
                dt = Convert.ToDateTime(reader[index]);
            }
            return dt;
        }
    }
}