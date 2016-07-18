﻿using System;
using System.Collections;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using WebAPIs.Models.DataModels;
using WebAPIs.Models.UnifiedTable;
using WebAPIs.Providers;
namespace WebAPIs.Models
{
    public class ExaminerHelper
    {
        public static ArrayList GetAllExamination(string doc_id)//获得该检验医生的所有检查记录
        {
            ArrayList AllExamination = new ArrayList();
            string sqlStr = String.Format(
               @"select patient_id, name, sex, birth, bed_num
                from hospitalization natural join consultation natural join patient natural join identity;
                where nurse_id='{0}'",
                doc_id);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
            OracleDataReader reader = cmd.ExecuteReader();
            DateTimeFormatInfo frm = new DateTimeFormatInfo();
            frm.ShortDatePattern = "yyyy-mm-dd HH24:mi:ss";
            try
            {
                while (reader.Read())
                {
                    AllExamination.Add(new ExaminationInfo(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                         reader[3].ToString(), Convert.ToDouble(reader[4]),Convert.ToDateTime(reader[5].ToString(),frm)));
                }
                return AllExamination;
            }
            catch (Exception e)
            {

            }
            return null;
        }


        public static bool MakeXRayExamination(string checkpoint, string from_picture, string picture)//插入XRay的检查结果
        {
            XrayInfo xray = new XrayInfo(checkpoint, from_picture, picture);
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.Connection;
            cmd.Transaction = DatabaseHelper.Connection.BeginTransaction();
            try
            {
                string sqlStr = String.Format(
                  @"insert into XRay
                values('{0}','{1}','{2}', {3})
               ", xray.exam_id,xray.checkpoint,xray.from_picture,xray.picture);
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
        public static bool MakeGastroscopeExamination(string from_picture, string diagnoses, string picture)//插入胃镜检查结果
        {
            GastroscopeInfo gastroscope = new GastroscopeInfo(from_picture, diagnoses ,picture);
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.Connection;
            cmd.Transaction = DatabaseHelper.Connection.BeginTransaction();
            try
            {
                string sqlStr = String.Format(
                  @"insert into XRay
                values('{0}','{1}','{2}', {3})
               ",gastroscope.exam_id,gastroscope.from_picture,gastroscope.diagnoses,gastroscope.picture);
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