using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPIs.Models.RawTable;
using WebAPIs.Providers;
using Oracle.DataAccess.Client;
using WebAPIs.Models.UnifiedTable;
using System.Globalization;

namespace WebAPIs.Models
{
    public class UserHelper
    {
        private int _cnt = 0;

        //SignUp as Patient
        public bool SignUp(SignUpUser item)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.Connection;
            cmd.Transaction = DatabaseHelper.Connection.BeginTransaction();
            try
            {
                string sqlStr = String.Format("insert into identity values ('{0}', '{1}', '{2}', to_date('{3}', 'dd/mm/yyyy')",
                    item.credit_num, item.name, item.sex, item.birth.ToShortDateString());
                cmd.CommandText = sqlStr;
                cmd.ExecuteNonQuery();

                sqlStr = String.Format("insert into patient values ('{0}', '{1}', '{2}')",
                    FormatHelper.GetYMD() + FormatHelper.GetIDNum(_cnt++), item.credit_num, item.passwd);
                cmd.CommandText = sqlStr;
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

        public string GetPwOfPatient(string id)
        {
            string sqlStr = String.Format("select password from patient where id='{0}'",
                id);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
            OracleDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader[2].ToString();
            }
            return null;
        }

        public string GetPwOfEmployee(string id)
        {
            string sqlStr = String.Format("select password from employee where id='{0}'",
                id);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
            OracleDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader[2].ToString();
            }
            return null;
        }

        //entire patient info
        public PatientInfo GetPatientInfo(string id)
        {
            string sqlStr = String.Format(
                @"select *
                from patient natural join identity
                where patient_id='{0}'",
                id);
            DateTimeFormatInfo frm = new DateTimeFormatInfo();
            frm.ShortDatePattern = "yyyy-mm-dd";
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
            OracleDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new PatientInfo(reader[0].ToString(),reader[1].ToString(), reader[2].ToString(),
                    reader[3].ToString(),reader[4].ToString(), Convert.ToDateTime(reader[5].ToString(), frm));
            }
            return null;
        }

        //entire employee info
        public EmployeeInfo GetEmployeeInfo(string id)
        {
            string sqlStr = String.Format(
               @"select *
                from employee natural join identity
                where patient_id='{0}'",
               id);
            DateTimeFormatInfo frm = new DateTimeFormatInfo();
            frm.ShortDatePattern = "yyyy-mm-dd";
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
            OracleDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new EmployeeInfo(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                    reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), Convert.ToDouble(reader[6].ToString()),
                    reader[7].ToString(), reader[8].ToString(),  Convert.ToDateTime(reader[9].ToString(), frm));
            }
            return null;
        }
    }
}