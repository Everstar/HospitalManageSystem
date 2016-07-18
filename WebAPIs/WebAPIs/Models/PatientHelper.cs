using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Oracle.DataAccess.Client;
using WebAPIs.Models.DataModels;
using WebAPIs.Models.UnifiedTable;
using WebAPIs.Providers;

namespace WebAPIs.Models
{
    public class PatientHelper
    {

        public static ArrayList GetAllClinic()
        {
            ArrayList clinics = new ArrayList();
            string sqlStr = String.Format(
               @"select clinic_name
                from clinic
               ");
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
            OracleDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    clinics.Add(reader[0].ToString());
                }
            }
            catch (Exception e)
            {

            }
            return clinics;
        }

        //only contain department, clinic, post, name, sex info

        public static ArrayList GetEmployeeOfClinic(string clinic_name)
        {
            ArrayList employees = new ArrayList();
            string sqlStr = String.Format(
               @"select dept_name, clinic_name, post, name, sex
                from employee natural join identity natural join clinic;
                where clinic_name='{0}'",
                clinic_name);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
            OracleDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    employees.Add(new EmployeeInfo(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                        reader[3].ToString(), reader[4].ToString()));
                }
            }
            catch (Exception e)
            {

            }
            return employees;
        }


        public static Duty GetEmployeeDutyTime(string id)
        {
            string sqlStr = String.Format(
              @"select room_num, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
                from employee natural join duty
               ");
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
            OracleDataReader reader = cmd.ExecuteReader();
            try
            {
                if (reader.Read())
                {
                    return new Duty(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                        reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(),
                        reader[7].ToString());
                }
            }
            catch (Exception e)
            {

            }
            return null;
        }
        
        public static bool  RegisterTreat(Treatment treat)

        {

            return true;
        }

        public static bool Commit(Evaluation item)
        {

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.Connection;
            cmd.Transaction = DatabaseHelper.Connection.BeginTransaction();
            try
            {
                string sqlStr = String.Format(
                  @"insert into evaluation
                values('{0}','{1}','{2}', {3}, '{4}')
               ", item.treat_id, item.patient_id, item.doc_id, item.rank, item.content);
                cmd.CommandText = sqlStr;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
                return false;
            }
            return true;
        }
    }
}