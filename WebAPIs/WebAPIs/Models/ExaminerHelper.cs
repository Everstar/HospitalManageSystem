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
            string sqlStr =
               @"select exam_id,type,exam_time,pay,pay_time
                from examination
                where employee_id=:doc_id and exam_id not in (select exam_id from
                XRay union select exam_id from Blood union select exam_id from GASTROSCOPE)";
            cmd.CommandText = sqlStr;
            cmd.Parameters.Add("doc_id", OracleDbType.Varchar2, 5).Value = doc_id;

            OracleDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    //var time1 = new DateTime();
                    //var time2 = new DateTime();

                    //if (!reader[2].ToString().Equals(""))
                    //{
                    //    time1 = Convert.ToDateTime(reader[2]);
                    //}
                    //if (!reader[4].ToString().Equals(""))
                    //{
                    //    time1 = Convert.ToDateTime(reader[4]);
                    //}
                    AllExamination.Add(new ExaminationInfo(reader[0].ToString(), reader[1].ToString(),
                        Formater.ToDateTime(reader, 2), Convert.ToDouble(reader[3]), Formater.ToDateTime(reader, 4)));
                }
                return AllExamination;
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }


        public static bool MakeXRayExamination(XrayInfo partXrayInfo)//插入XRay的检查结果
        {
            XrayInfo xray = partXrayInfo;
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            try
            {
                string sqlStr = String.Format(
                  @"insert into XRay
                values('{0}','{1}','{2}','{3}')",
                  xray.exam_id, xray.checkpoint, xray.from_picture, xray.picture);
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

        public static bool MakeGastroscopeExamination(GastroscopeInfo item)
        {
            //  GastroscopeInfo gastroscope = new GastroscopeInfo(from_picture, diagnoses, picture);

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            try
            {

                string sqlStr =
                  @"insert into Gastroscope(exam_id,from_picture,diagnoses,picture)
                values(:exam_id,:frompicture,:diagnoses,:picture)";

                cmd.CommandText = sqlStr;
                cmd.Parameters.Add("exam_id", item.exam_id);
                cmd.Parameters.Add("frompicture", OracleDbType.Varchar2, 2000).Value = item.from_picture;
                cmd.Parameters.Add("diagnoses", OracleDbType.Varchar2, 2000).Value = item.diagnoses;
                cmd.Parameters.Add("picture", OracleDbType.Varchar2, 200).Value = item.picture;
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
                throw e;
                return false;
            }
            return false;
        }

        public static bool MakeBloodExamination(Blood item)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            try
            {
                string sqlStr = string.Format(
                    @"insert into blood
                    values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}'
                            ,'{15}','{16}','{17}','{18}','{19}','{20}','{21}', '{22}')", item.exam_id, item.wbc, item.neut_percent, item.lymph_percent,
                    item.eo_percent, item.baso_percent, item.neut_num, item.lymph_num, item.mono_num, item.eo_num, item.baso_num,
                    item.rbc, item.hgb, item.hct, item.mcv, item.mch, item.mchc, item.rdw,
                    item.plt, item.mpv, item.pdw, item.pct);
                cmd.CommandText = sqlStr;
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
                return false;
            }
            return false;

        }


        public static ArrayList GetPatientByExamId(string examineID)
        {

            ArrayList patientInfo = new ArrayList();

            string sqlStr = String.Format(
               @"with treatmentid(patient_id) as(
                      select treatment.patient_id
                      from treatment natural join examine
                      where examine.exam_id = '{0}')
                 select identity.name, identity.sex 
                 from patient natural join identity, treatmentid
                 where patient.patient_id=treatmentid.patient_id",examineID);

  

            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);


            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    patientInfo.Add(reader[0].ToString());
                    patientInfo.Add(reader[1].ToString());
                }
                return patientInfo;
            }
            catch (Exception e)
            {
                return null;
            }
            return patientInfo;
        }
    }
}