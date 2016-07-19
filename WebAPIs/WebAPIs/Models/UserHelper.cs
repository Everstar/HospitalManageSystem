﻿using System;
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

            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
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
                cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);

                cmd.CommandText = sqlStr;

                cmd.Parameters.Add("credit_num", OracleDbType.Varchar2, 18).Value = item.credit_num;
                cmd.Parameters.Add("name", OracleDbType.Varchar2, 40).Value = item.name;
                cmd.Parameters.Add("sex", OracleDbType.Char, 1).Value = item.sex;
                cmd.Parameters.Add("birth", OracleDbType.Date).Value = item.birth.ToShortDateString();
                cmd.ExecuteNonQuery();//这里貌似没意义，问一下？？？

                sqlStr = "insert into patient values (:credit_num, :password)";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add("credit_num", OracleDbType.Varchar2, 18).Value = item.credit_num;
                cmd.Parameters.Add("password", OracleDbType.Varchar2, 20).Value = item.passwd;
                    

                cmd.Parameters.Add("credit_num", item.credit_num);
                cmd.Parameters.Add("name", item.name);
                cmd.Parameters.Add("sex", item.sex);
                cmd.ExecuteNonQuery();

                sqlStr = "insert into patient values (null, :credit_num, :password)";
                cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
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

            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);

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
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);

            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return reader[0].ToString();
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
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);

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
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
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
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);

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