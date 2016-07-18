using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using WebAPIs.Models.DataModels;
using WebAPIs.Models.UnifiedTable;
using WebAPIs.Providers;
namespace WebAPIs.Models
{
    public class NurseHelper//获得该护士照顾的所有病人
    {
        public static ArrayList GetHospitalizationInfo(string nurse_id)
        {
            ArrayList hospitalization = new ArrayList();
            string sqlStr = String.Format(
               @"select hos_id,treat_id,nurse_id,bed_num,pay,rank,in_time,out_time,pay_time
                from hospitalization natural join bed;
                where nurse_id='{0}'",
                nurse_id);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
            OracleDataReader reader = cmd.ExecuteReader();
            DateTimeFormatInfo frm = new DateTimeFormatInfo();
            frm.ShortDatePattern = "yyyy-mm-dd HH24:mi:ss";
            try
            {
                while (reader.Read())
                {
                    hospitalization.Add(new HospitalInfo(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                        reader[3].ToString(),Convert.ToDouble(reader[4]),Convert.ToInt32(reader[5]),
                         Convert.ToDateTime(reader[5].ToString(), frm), 
                         Convert.ToDateTime(reader[6].ToString(), frm), 
                         Convert.ToDateTime(reader[7].ToString(), frm)));
                }
                return hospitalization;
            }
            catch (Exception e)
            {

            }
            return null;
        }
    }
}