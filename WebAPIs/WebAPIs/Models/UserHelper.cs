using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlTypes;
using WebAPIs.Models.DataModels;
using WebAPIs.Providers;
using WebAPIs.Models.UnifiedTable;
using System.Globalization;
using Oracle.ManagedDataAccess.Client;

namespace WebAPIs.Models
{
    public class UserHelper
    {
        private static int _cnt = 0;

        //SignUp as Patient
        public static bool SignUp(SignUpUser item)
        {
            //check if the credit_num is used
            string sqlStr =
                @"select * from identity
                  where credit_num = :credit_num";

            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
            cmd.Transaction = DatabaseHelper.Connection.BeginTransaction();
            cmd.Parameters.Add("credit_num", item.credit_num);
            OracleDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return false;
            }
            var strBirth = item.birth.ToString().Split(' ')[0];
            //sign up patient
            try
            {
                sqlStr = "insert into identity values (:credit_num, :name, :sex, to_date('"
                    + strBirth + "', 'yyyy/mm/dd'))";
                cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);

                cmd.CommandText = sqlStr;
                cmd.Parameters.Add("credit_num", item.credit_num);
                cmd.Parameters.Add("name", item.name);
                cmd.Parameters.Add("sex", item.sex);
                cmd.ExecuteNonQuery();

                sqlStr = "insert into patient values (null, :credit_num, :password)";
                cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
                cmd.Parameters.Add("credit_num", item.credit_num);
                cmd.Parameters.Add("password", item.passwd);
                cmd.ExecuteNonQuery();

                cmd.Transaction.Commit();
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
                return false;
            }
            return true;
        }



        public static string GetPwOfPatient(string id)
        {
            string sqlStr = String.Format("select password from patient where patient_id='{0}'",
                id);

            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);

            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var b = reader[0].ToString();
                    return b;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

        public static string GetPwOfEmployee(string id)
        {
            string sqlStr = String.Format("select password from employee where employee_id='{0}'",
                id);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);

            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return reader[2].ToString();
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

        //entire patient info
        public static PatientInfo GetPatientInfo(string id)
        {
            string sqlStr = String.Format(
                @"select patient_id, credit_num, password, name, sex, birth
                from patient natural join identity
                where patient_id='{0}'",
                id);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);

            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new PatientInfo(
                        reader[0].ToString(), 
                        reader[1].ToString(), 
                        reader[2].ToString(),
                        reader[3].ToString(), 
                        reader[4].ToString(), 
                        Convert.ToDateTime(reader[5].ToString()));
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

        //entire employee info
        public static EmployeeInfo GetEmployeeInfo(string id)
        {
            string sqlStr = String.Format(
               @"select employee_id, credit_num, password, dept_name, clinic_name, post, salary, name, sex, birth
                from employee natural join identity
                where employee_id='{0}'",
               id);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new EmployeeInfo(
                        reader[0].ToString(),
                        reader[1].ToString(),
                        reader[2].ToString(),
                        reader[3].ToString(),
                        reader[4].ToString(),
                        reader[5].ToString(),
                        Convert.ToDouble(reader[6].ToString()),
                        reader[7].ToString(),
                        reader[8].ToString(),
                        Convert.ToDateTime(reader[9].ToString()));
                }
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public static PatientInfo GetPatientInfoByCredNum(string num)
        {
            string sqlStr = String.Format(
                @"select patient_id, credit_num, password, name, sex, birth
                from patient natural join identity
                where credit_num='{0}'",
                num);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);

            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new PatientInfo(
                        reader[0].ToString(),
                        reader[1].ToString(),
                        reader[2].ToString(),
                        reader[3].ToString(),
                        reader[4].ToString(),
                        Convert.ToDateTime(reader[5].ToString()));
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
    }
}