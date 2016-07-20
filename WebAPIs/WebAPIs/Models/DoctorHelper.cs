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
    public class DoctorHelper
    {
        public static ArrayList GetAllTreatment(string employee_id)
        {
            string sqlStr = String.Format(@"select *
                from treatment
                where employee_id='{0}'", employee_id);
            ArrayList treatInfo = new ArrayList();
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    treatInfo.Add(new Treatment
                    {
                        treat_id = reader[0].ToString(),
                        clinic = reader[1].ToString(),
                        start_time = (DateTime)reader[2],
                        end_time = (DateTime)reader[3],
                        patient_id = reader[4].ToString(),
                        doc_id = reader[5].ToString(),
                        take = (int)reader[6],
                        pay = (double)reader[7],
                        pay_time = (DateTime)reader[8]
                    });
                }
                return treatInfo;
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public static bool TakeTreat(string employee_id, string treat_id)
        {
            string sqlStr = String.Format(
            @"update treatment
            set take=1
            where treat_id='{0}'", treat_id);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            try
            {
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
            }
            return false;
        }

        public static ArrayList GetAllMedicine()
        {
            ArrayList medicine = new ArrayList();
            string sqlStr =
                @"select * 
                from medicine";
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    medicine.Add(new Medicine
                    {
                        medicine_id = reader[0].ToString(),
                        name = reader[1].ToString(),
                        unit = reader[2].ToString(),
                        stock = (int)reader[3]
                    });
                }
                return medicine;
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public static bool  WriteExamination(string treat_id, string employee_id, int type){
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            OracleTransaction transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            cmd.Transaction = transaction;
            string sqlStr;
            try
            {
                
                sqlStr = @"insert into examination
                values(null, :type, systimestamp, :employee_id)";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add("type" ,OracleDbType.Int32).Value = type;
                cmd.Parameters.Add("employee_id", OracleDbType.Varchar2, 20).Value = employee_id;
                cmd.ExecuteNonQuery();

                //select related exam_id
                string exam_id = "";
                switch (type)
                {
                    case 2:
                        sqlStr = String.Format(
                        @"insert into XRay 
                        values('{0}', :checkpoint, :from_picture, :picture)", exam_id);
                        cmd = new OracleCommand();
                        cmd.Connection = DatabaseHelper.GetInstance().conn;
                        cmd.Transaction = transaction;
                        cmd.Parameters.Add("checkpoint", OracleDbType.Varchar2, 100).Value = "";
                        cmd.Parameters.Add("from_picture", OracleDbType.Varchar2, 2000).Value = "";
                        cmd.Parameters.Add("picture", OracleDbType.Varchar2, 200).Value = "";
                        cmd.ExecuteNonQuery();
                        break;
                    case 1:
                        sqlStr = String.Format(
                        @"insert into gastroscope 
                        values('{0}', :from_picture, :diagnoses, :picture)", exam_id);
                        cmd = new OracleCommand();
                        cmd.Connection = DatabaseHelper.GetInstance().conn;
                        cmd.Transaction = transaction;
                        cmd.Parameters.Add("from_picture", OracleDbType.Varchar2, 2000).Value = "";
                        cmd.Parameters.Add("diagnoses", OracleDbType.Varchar2, 100).Value = "";
                        cmd.Parameters.Add("picture", OracleDbType.Varchar2, 200).Value = "";
                        cmd.ExecuteNonQuery();
                        break;
                    case 0:

                        break;
                    default:
                        return false;
                }

                return true;
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
            }
            return false;
        }

        
    

    }
}