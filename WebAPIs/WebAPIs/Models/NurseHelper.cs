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

    /// <summary>
    /// 
    /// is finished
    /// </summary>
    public class NurseHelper//获得该护士照顾的所有病人
    {
        public static ArrayList GetHospitalizationInfo(string nurse_id)
        {
            ArrayList hospitalization = new ArrayList();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            string sqlStr =
                @"select hos_id,treat_id,employee_id,bed_num,pay,rank,in_time,out_time,pay_time
                from hospitalization natural join bed 
                where employee_id=:Pnurse_id and out_time is null";
            cmd.CommandText = sqlStr;
            cmd.Parameters.Add("Pnurse_id", nurse_id);
            OracleDataReader reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    hospitalization.Add(new
                        HospitalInfo(
                        reader[0].ToString(),
                        reader[1].ToString(),
                        reader[2].ToString(),
                        reader[3].ToString(),
                        double.Parse(reader[4].ToString()),
                        int.Parse(reader[5].ToString()),
                        (DateTime)reader[6]
                         ));
                }
                return hospitalization;
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }


        public static bool OutHospital(string hospitalId)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            string sqlStr =
                @" update hospitalization
                   set out_time = systimestamp
                   where hos_id =:Hos_id";
            cmd.CommandText = sqlStr;

            cmd.Parameters.Add("Hos_id", hospitalId);
            try
            {
                if (1 != cmd.ExecuteNonQuery())
                {
                    throw new Exception("更改失败！");
                }
                cmd.Transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
                throw e;
            }
            return false;

        }
    }
}

