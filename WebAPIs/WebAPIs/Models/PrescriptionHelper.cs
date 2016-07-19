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
    public class PrescriptionHelper
    {
        public static ArrayList GetAllPrescription(string employee_id)//获得该药剂师的所有配药记录
        {
            ArrayList allPrescription = new ArrayList();
            string sqlStr = 
                @"select *
                from prescription
                where employee_id=:em_id";
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.Connection;
            cmd.Transaction = DatabaseHelper.Connection.BeginTransaction();
            cmd.CommandText = sqlStr;
            cmd.Parameters.Add("em_id",  employee_id);
            OracleDataReader reader = cmd.ExecuteReader();
            DateTimeFormatInfo frm = new DateTimeFormatInfo();
            frm.ShortDatePattern = "yyyy-mm-dd HH24:mi:ss";
            try
            {
                while(reader.Read())
                {
                    allPrescription.Add(new PrescriptionInfo(reader[0].ToString(),
                        reader[1].ToString(), Convert.ToDateTime(reader[2], frm),
                        Convert.ToDateTime(reader[3], frm), Convert.ToDouble(reader[4]),
                        Convert.ToDateTime(reader[5], frm)));
                }
                return allPrescription;
            }
            catch(Exception ex)
            {
                
            }
            return null;
        }


        public static ArrayList Prescribe(string pres_id)
        {
            ArrayList prescribeMedicine = new ArrayList();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.Connection;
            cmd.Transaction = DatabaseHelper.Connection.BeginTransaction();
            string sqlStr =
                @"select medicine_id,unit,num,name
                from prescribe natural join medicine
                where prescribe.pres_id=:Ppres_id";
            cmd.CommandText = sqlStr;
            cmd.Parameters.Add("Ppres_id", pres_id);
            OracleDataReader reader = cmd.ExecuteReader();
            try
            {
                while(reader.Read())
                {
                    prescribeMedicine.Add(new PrescribeMedicine(reader[0].ToString(), reader[1].ToString(), Convert.ToInt32(reader[2]), reader[3].ToString()));
                }
                return prescribeMedicine;
            }
            catch(Exception ex)
            {

            }
            return null
;        }
    }
}