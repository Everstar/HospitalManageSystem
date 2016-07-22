using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPIs.Providers;
using System.Collections;
using WebAPIs.Models.DataModels;
using WebAPIs.Models.UnifiedTable;
using Oracle.ManagedDataAccess.Client;


namespace WebAPIs.Models
{
    public class PayHelper
    {
        //if type=Treat, then id=treat_id
        //if type=Exam, then id=exam_id
        //if type=Pres, then id=pres_id
        //if type=Surg, then id=surg_id
        //if type=Hos, then id=hos_id
        public static bool Pay(double pay, PayType type, string id)
        {
            string sqlStr;
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = cmd.Connection.BeginTransaction();
            try
            {
                switch (type)
                {
                    case PayType.Treat:
                        sqlStr = @"update treatment set pay=:pay , pay_time=systimestamp where treat_id=:treat_id";
                        cmd.CommandText = sqlStr;
                        cmd.Parameters.Add("pay", OracleDbType.Double).Value = pay;
                        cmd.Parameters.Add("treat_id", OracleDbType.Varchar2, 20).Value = id;
                        cmd.ExecuteNonQuery();
                        break;
                    case PayType.Exam:
                        sqlStr = @"update examination set pay=:pay , pay_time=systimestamp where exam_id=:exam_id";
                        cmd.CommandText = sqlStr;
                        cmd.Parameters.Add("pay", OracleDbType.Double).Value = pay;
                        cmd.Parameters.Add("exam_id", OracleDbType.Varchar2, 20).Value = id;
                        cmd.ExecuteNonQuery();
                        break;
                    case PayType.Pres:
                        sqlStr = @"update prescription set pay=:pay , pay_time=systimestamp where pres_id=:pres_id";
                        cmd.CommandText = sqlStr;
                        cmd.Parameters.Add("pay", OracleDbType.Double).Value = pay;
                        cmd.Parameters.Add("pres_id", OracleDbType.Varchar2, 20).Value = id;
                        cmd.ExecuteNonQuery();
                        break;
                    case PayType.Surg:
                        sqlStr = @"update surgery set pay=:pay , pay_time=systimestamp where surg_id=:surg_id";
                        cmd.CommandText = sqlStr;
                        cmd.Parameters.Add("pay", OracleDbType.Double).Value = pay;
                        cmd.Parameters.Add("surg_id", OracleDbType.Varchar2, 20).Value = id;
                        cmd.ExecuteNonQuery();
                        break;
                    case PayType.Hos:
                        sqlStr = @"update hospitalization set pay=:pay , pay_time=systimestamp where hos_id=:hos_id";
                        cmd.CommandText = sqlStr;
                        cmd.Parameters.Add("pay", OracleDbType.Double).Value = pay;
                        cmd.Parameters.Add("hos_id", OracleDbType.Varchar2, 20).Value = id;
                        cmd.ExecuteNonQuery();
                        break;
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