using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace WebAPIs.Providers
{
    public class DatabaseHelper
    {

        static DatabaseHelper()
        {
            try
            {
                conn = new OracleConnection(@"Data Source=(DESCRIPTION =
                (ADDRESS_LIST =
                (ADDRESS = (PROTOCOL = TCP)(HOST = 221.239.197.176)(PORT = 2333))
                )
                (CONNECT_DATA =
                (SERVICE_NAME = UnivHosDB)
                )
                );User Id=lvjinhua;Password=123456");
                conn.Open();
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("Connect Oracle Error!");
            }

        }

        private static OracleConnection conn;

        public static OracleConnection Connection
        {
            get
            {
                return conn;
            }
        }
    }
}