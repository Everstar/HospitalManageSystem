using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace WebAPIs.Providers
{
    public class DatabaseHelper
    {
        private static OracleConnection conn = null;

        public static OracleConnection Connection
        {
            get
            {
                if (conn == null)
                {
                    try
                    {
                        conn = new OracleConnection(WebConfigurationManager.AppSettings["connectStr"]);
                        conn.Open();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Connect Oracle Error!");
                    }
                    return conn;
                }
                return conn;
            }
        }
    }
}