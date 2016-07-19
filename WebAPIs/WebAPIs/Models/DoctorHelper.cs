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
        public static ArrayList GetAllTreatment(int employee_id)
        {
            sqlStr = @"select treat_id,patient_id,patient_name
                       from treatment T join takes using treat_id join patient using patient_id join identity using credi
                        where treatment.employee_id=:Pemployee_id";
            cmd.CommandText = sqlStr;
            cmd.Parameters.Add("credit_num", OracleDbType.Varchar2, 18).Value = item.credit_num;
            cmd.Parameters.Add("name", OracleDbType.Varchar2, 40).Value = item.name;
            cmd.Parameters.Add("sex", OracleDbType.Char, 1).Value = item.sex;
            cmd.Parameters.Add("birth", OracleDbType.Date).Value = item.birth.ToShortDateString();
            cmd.ExecuteNonQuery();

        }
    }
}