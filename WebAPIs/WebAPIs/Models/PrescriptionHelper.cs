using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

using WebAPIs.Models.DataModels;
using WebAPIs.Models.UnifiedTable;
using WebAPIs.Providers;
using Oracle.ManagedDataAccess.Client;

using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using WebAPIs.Models.DataModels;
using WebAPIs.Models.UnifiedTable;
using WebAPIs.Providers;


namespace WebAPIs.Models
{

    
    public class PrescriptionHelper
    {

        
        //根据药剂师id获取所有有关的处方id和相关病人姓名，性别
        public static ArrayList GetAllPrescription(string pharmacistId)
        {
            ArrayList list = new ArrayList();

            string sqlStr = String.Format(
                @" with prewithdoc(treat_id) as(
                        select treat_id
                        from prescription, employee
                        where employee.employee_id = '{0}' and prescription.done_time = null);
                   with patientinfo(name, sex, treat_id) as(
                        select name, sex, patient_id
                        from identity natural join patient natural join consulation natural join treatment);
                   select *
                   from patientinfo natural join prewithdoc",pharmacistId);

            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
           

            int i = 0;

            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (i == 0)
                    {
                        list.Add(1);
                        list.Add(new AllPrescription(reader[0].ToString(), reader[1].ToString(), reader[2].ToString()));
                        i++;
                    }
                    else
                    {
                        list.Add(new AllPrescription(reader[0].ToString(), reader[1].ToString(), reader[2].ToString()));
                    }
                    
                }
                if (i == 0)
                {
                    list.Add(0);
                }
                return list;
            }
            catch(Exception e)
            {
                return null;
            }
            return null;
        }

        public static  ArrayList Prescribe(string pres_id)
        {
            ArrayList list = new ArrayList();

            string sqlStr = String.Format(
                @"select name, num, unit
                  from prescription natural join prescribe natural join medicine
                  where pres_id ='{0}'",pres_id);

            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
           

            int i = 0;

            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (i == 0)
                    {
                        list.Add(1);
                        list.Add(new MedicInfo(reader[0].ToString(), reader[1].ToString(), reader[2].ToString()));
                        i++;
                    }
                    else
                    {
                        list.Add(new MedicInfo(reader[0].ToString(), reader[1].ToString(), reader[2].ToString()));
                    }
                }

                if (i == 0)
                {
                    list.Add(0);
                }

                return list;

            }
            catch(Exception e)
            {
                return null;
            }
            return null;
        }

        public static bool UpdateDoneTime(string pres_id)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            string sqlStr = 
                @" update prescription
                   set done_time = to_date(:Pnowtime,'yyyy-mm-dd hh24:mi:ss')
                   where pres_id =:Pres_id";
            cmd.CommandText = sqlStr;
            cmd.Parameters.Add("Ponwtime", DateTime.Now.ToString("yyyy-mm-dd hh24:mi:ss"));
            cmd.Parameters.Add("Pres_id", pres_id);
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

    }


}