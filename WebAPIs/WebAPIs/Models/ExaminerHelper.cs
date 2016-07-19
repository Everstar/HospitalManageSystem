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
        public static ArrayList GetAllExamination(string doc_id)//获得该检验医生的所有检查记录
        {
            ArrayList AllExamination = new ArrayList();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.Connection;
            cmd.Transaction = DatabaseHelper.Connection.BeginTransaction();
            string sqlStr = 
               @"select exam_id,type,exam_time,pay,pay_time
                from examination
                where employee_id=:doc_id";
            cmd.CommandText = sqlStr;
            cmd.Parameters.Add("doc_id", OracleDbType.Varchar2, 5).Value = doc_id;

            OracleDataReader reader = cmd.ExecuteReader();
            DateTimeFormatInfo frm = new DateTimeFormatInfo();
            frm.ShortDatePattern = "yyyy-mm-dd HH24:mi:ss";
            try
            {
                while (reader.Read())
                {
                    AllExamination.Add(new ExaminationInfo(reader[0].ToString(), reader[1].ToString(), 
                         Convert.ToDateTime(reader[2].ToString(),frm),  Convert.ToDouble(reader[3]),Convert.ToDateTime(reader[4].ToString(),frm)));
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



        // 0:验血 1：胃镜 2：XRay
        public static ArrayList GetPatientByExamId(string examineID,char type)
        {
            ArrayList patientInfo = new ArrayList();

            if (type == '0')
            {
                string sqlStr = String.Format(
                    @"with bloodexaminfo(treatmentid)as(
                           select treat_id
                           from blood natural join examine
                           where exam_id={0});
                      with pattreat(pat_id)as{
                           select patient_id
                           from consulation natural join treatment, bloodexaminfo
                           where treat_id = bloodexaminfo.treatmentid);
                      select name, sex
                      from identity natural patient,pattreat
                      where patient.patient_id=pattreat.pat_id          
                     ", examineID);
                OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
                OracleDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.Read())
                    {
                        patientInfo.Add(reader[0].ToString());
                        patientInfo.Add(reader[1].ToString());
                        return patientInfo;
                        
                    }
                }
                catch(Exception e)
                {
                    patientInfo.Add("查询失败");
                    return patientInfo; 
                }
            }
            else if (type == '1')
            {
                string sqlStr = String.Format(
                       @"with gastexaminfo(treatmentid)as(
                           select treat_id
                           from gastroscope natural join examine
                           where exam_id={0});
                      with pattreat(pat_id)as{
                           select patient_id
                           from consulation natural join treatment, gastexaminfo
                           where treat_id = gastexaminfo.treatmentid);
                      select name, sex
                      from identity natural patient,pattreat
                      where patient.patient_id=pattreat.pat_id          
                     ", examineID);
                OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
                OracleDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.Read())
                    {
                        patientInfo.Add(reader[0].ToString());
                        patientInfo.Add(reader[1].ToString());
                        return patientInfo;

                    }
                }
                catch (Exception e)
                {
                    patientInfo.Add("查询失败");
                    return patientInfo;
                }

            }
            else if(type=='2')
            {
                string sqlStr = String.Format(
                    @"with xrayexaminfo(treatmentid)as(
                           select treat_id
                           from XRay natural join examine
                           where exam_id={0});
                      with pattreat(pat_id)as{
                           select patient_id
                           from consulation natural join treatment, xrayexaminfo
                           where treat_id = xrayexaminfo.treatmentid);
                      select name, sex
                      from identity natural patient,pattreat
                      where patient.patient_id=pattreat.pat_id          
                     ", examineID);
                OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
                OracleDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.Read())
                    {
                        patientInfo.Add(reader[0].ToString());
                        patientInfo.Add(reader[1].ToString());
                        return patientInfo;

                    }
                }
                catch (Exception e)
                {
                    patientInfo.Add("查询失败");
                    return patientInfo;
                }
            }
            else
            {
                patientInfo.Add("Examine Type Error!");
            }

            return patientInfo;


            
        } 
    }
}