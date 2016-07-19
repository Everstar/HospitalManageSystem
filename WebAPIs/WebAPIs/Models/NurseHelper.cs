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
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            string sqlStr = 
                @"select hos_id,treat_id,employee_id,bed_num,pay,rank,in_time,out_time,pay_time
                from hospitalization natural join bed 
                where employee_id=:Pnurse_id";
            cmd.CommandText = sqlStr;
            cmd.Parameters.Add(":Pnurse_id", OracleDbType.Varchar2, 5).Value = nurse_id;            
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

