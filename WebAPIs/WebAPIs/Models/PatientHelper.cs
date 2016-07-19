using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using WebAPIs.Models.DataModels;
using WebAPIs.Models.UnifiedTable;
using WebAPIs.Providers;
using Oracle.ManagedDataAccess.Client;

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
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
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
                from employee natural join identity natural join clinic
                where clinic_name='{0}'",
                clinic_name);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            OracleDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    employees.Add(new EmployeeInfo(
                        reader[0].ToString(), 
                        reader[1].ToString(), 
                        reader[2].ToString(),
                        reader[3].ToString(), 
                        reader[4].ToString()));
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
                where employee_id='{0}'",
                id);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            OracleDataReader reader = cmd.ExecuteReader();
            try
            {
                if (reader.Read())
                {
                    return new Duty(
                        reader[0].ToString(), 
                        reader[1].ToString(), 
                        reader[2].ToString(),
                        reader[3].ToString(), 
                        reader[4].ToString(), 
                        reader[5].ToString(),
                        reader[6].ToString(),
                        reader[7].ToString());
                }
            }
            catch (Exception e)
            {

            }
            return null;
        }


        public static int RegisterTreat(Treatment treat)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            try
            {
                string sqlStr = "insert into treatment values(:clinic_name, :start_time, :end_time, :employee_id)";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add("clinic_name", OracleDbType.Varchar2, 20).Value = treat.clinic;
                cmd.Parameters.Add("start_time", OracleDbType.Date).Value = treat.start_time.ToString();
                cmd.Parameters.Add("end_time", OracleDbType.Date).Value = treat.end_time.ToString();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
                return -1;
            }

            //select the max id;
            return 0;
        }

        public static bool Comment(Evaluation item)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
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

        //根据treatment_id，返回doctorID&name
        public static ArrayList GetDoctorIdName(string treatment_id)
        {
            string sqlStr = String.Format(
                @"select employee_id, name
                 from treatment natural join (employee natural join identity)
                 where treat_id ={0}", treatment_id);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            OracleDataReader reader = cmd.ExecuteReader();

            ArrayList doctorIdName = new ArrayList();
            try
            {
                if (reader.Read())
                {
                    doctorIdName.Add(reader[0].ToString());
                    doctorIdName.Add(reader[1].ToString());
                    return doctorIdName;
                }
            }
            catch(Exception e)
            {
                return null;
            }
            return null;
        }
    }
}