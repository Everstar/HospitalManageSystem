using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;

namespace WebAPIs.Providers
{
    public class DatabaseHelper
    {

        static DatabaseHelper()
        {
            conn = new OracleConnection(@"Data Source=(DESCRIPTION =
            (ADDRESS_LIST =
            (ADDRESS = (PROTOCOL = TCP)(HOST = 222.69.213.9)(PORT = 2333))
            )
            (CONNECT_DATA =
            (SERVICE_NAME = UnivHosDB)
            )
            );User Id=lvjinhua;Password=123456");
            conn.Open();
        }

        private static OracleConnection conn;

        public OracleConnection Connection
        {
            get
            {
                return conn;
            }
        }
    }
}