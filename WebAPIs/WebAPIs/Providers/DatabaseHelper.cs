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
        private static DatabaseHelper instance = null;

        public OracleConnection conn = null;

        private DatabaseHelper()
        {
            if (conn == null)
            {
                conn = new OracleConnection(WebConfigurationManager.AppSettings["connectStr"]);
                conn.Open();
            }
        }

        public static DatabaseHelper GetInstance()
        {
            if (null == instance)
            {
                instance = new DatabaseHelper();
            }
            return instance;
        }
    }
}