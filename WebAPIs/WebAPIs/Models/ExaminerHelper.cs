using System;
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
        // Test Passed
        public static ArrayList GetAllExamination(string doc_id)//获得该检验医生的所有检查记录
        {
            ArrayList AllExamination = new ArrayList();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            string sqlStr = 
               @"select exam_id,type,exam_time,pay,pay_time
                from examination
                where employee_id=:doc_id";
            cmd.CommandText = sqlStr;
            cmd.Parameters.Add("doc_id", OracleDbType.Varchar2, 5).Value = doc_id;

            OracleDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    AllExamination.Add(new ExaminationInfo(reader[0].ToString(), reader[1].ToString(), 
                         (DateTime)reader[2],  Convert.ToDouble(reader[3]),(DateTime)reader[4]));
                }
                return AllExamination;
            }
            catch (Exception e)
            {

            }
            return null;
        }


        public static bool MakeXRayExamination(PartXRayInfo partXrayInfo)//插入XRay的检查结果
        {
            XrayInfo xray = new XrayInfo(partXrayInfo.checkpoint,partXrayInfo.from_picture, partXrayInfo.picture);
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
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
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
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

        public static bool MakeBloodExamination(Blood item)
        {
            //TODO
            return false;

        }

        // 0:验血 1：胃镜 2：XRay
        //public static ArrayList GetPatientByExamId(string examineID)
        //{
        //    ArrayList patientInfo = new ArrayList();

        //    string sqlStr = String.Format(
        //       @"with treatmentid(treat_id) as(
        //              select treatment.treat_id
        //              from treatment natural join examine natural join examination
        //              where examine");

        //    OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);

        //    return patientInfo;


            
        //} 
    }
}