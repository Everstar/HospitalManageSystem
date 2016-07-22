using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlTypes;
using WebAPIs.Models.DataModels;
using WebAPIs.Providers;
using WebAPIs.Models.UnifiedTable;
using System.Globalization;
using Oracle.ManagedDataAccess.Client;
using System.Collections;

namespace WebAPIs.Models
{
    public class UserHelper
    {
        //SignUp as Patient
        public static string SignUp(SignUpUser item)
        {
            //check if the credit_num is used
            string sqlStr = @"select count_credit(:credit_num) from dual";

            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            cmd.Parameters.Add("credit_num", item.credit_num);
            OracleDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (Convert.ToInt32(reader[0]) > 0)
                    //查询语句不需要回滚
                    return "Already exise";
            }
            //var strBirth = item.birth.ToString().Split(' ')[0];
            var strBirth = item.birth.Year.ToString() + "/" + item.birth.Month.ToString() + "/" + item.birth.Day.ToString();
            //sign up patient
            try
            {
                sqlStr = "insert into identity values (:credit_num, :name, :sex, to_date('"
                    + strBirth + "', 'yyyy/mm/dd'))";
                cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
                cmd.CommandText = sqlStr;
                cmd.Transaction = cmd.Connection.BeginTransaction();

                cmd.Parameters.Add("credit_num", item.credit_num);
                cmd.Parameters.Add("name", item.name);
                cmd.Parameters.Add("sex", item.sex);
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();


                sqlStr = "insert into patient values (null, :credit_num, :password, null)";
                cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
                cmd.Transaction = cmd.Connection.BeginTransaction();
                cmd.Parameters.Add("credit_num", item.credit_num);
                cmd.Parameters.Add("password", item.passwd);
                cmd.ExecuteNonQuery();


                cmd.Transaction.Commit();
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
                return "Insert failed, message:" + e.Message + " " + strBirth;
            }
            return "Ok";
        }

        
        public static string GetPwOfPatient(string id)
        {
            OracleCommand cmd = DatabaseHelper.GetInstance().conn.CreateCommand();
            cmd.CommandText = "select get_pw(:id, 0) from dual";
            cmd.Parameters.Add("id", OracleDbType.Varchar2, 9).Value = id;

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
                throw e;
            }
            return null;
        }

        public static string GetPwOfEmployee(string id)
        {
            OracleCommand cmd = DatabaseHelper.GetInstance().conn.CreateCommand();
            cmd.CommandText = "select get_pw(:id, 1) from dual";
            cmd.Parameters.Add("id", OracleDbType.Varchar2, 5).Value = id;
            
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
                //无需做任何操作
                throw e;
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
                        Formater.ToDateTime(reader, 5));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }

        //entire employee info
        public static EmployeeInfo GetEmployeeInfo(string id)
        {
            string sqlStr = String.Format(
               @"select employee_id, credit_num, password, dept_name, clinic_name, post, salary, name, sex, birth, skill, profile, avatar_path
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
                        Formater.ToDateTime(reader, 9),
                        reader[10].ToString(),
                        reader[11].ToString(),
                        reader[12].ToString()
                        );
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }
        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetAllEmployee()
        {
            string sqlStr = @"select employee_id, credit_num, password, dept_name, clinic_name, post, salary, name, sex, birth, skill, profile, avatar_path 
                            from employee natural join identity";
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                ArrayList list = new ArrayList();
                while (reader.Read())
                {
                    list.Add(new EmployeeInfo(
                        reader[0].ToString(),
                        reader[1].ToString(),
                        reader[2].ToString(),
                        reader[3].ToString(),
                        reader[4].ToString(),
                        reader[5].ToString(),
                        Convert.ToDouble(reader[6].ToString()),
                        reader[7].ToString(),
                        reader[8].ToString(),
                        Formater.ToDateTime(reader, 9),
                        reader[10].ToString(),
                        reader[11].ToString(),
                        reader[12].ToString()
                        ));
                }
                return list;
            }
            catch (Exception e)
            {
                throw e;
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
                        Formater.ToDateTime(reader, 5));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }

        public static string GetPatientIdByTreatmentId(string id)
        {
            string sqlStr = @"select * from treatment natural join patient";
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);

            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[1].ToString().Equals(id))
                        return reader[0].ToString();
                }
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }

        public static string GetTreatmentIdByExamId(string exam_id)
        {
            string sqlStr = @"select * from examine";
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);

            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[0].ToString().Equals(exam_id))
                        return reader[1].ToString();
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

        

    }
}