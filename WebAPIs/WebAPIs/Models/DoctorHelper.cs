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
        public static ArrayList GetAllTreatment(string employee_id)
        {
            string sqlStr = String.Format(@"select *
                from treatment
                where employee_id='{0}'", employee_id);
            ArrayList treatInfo = new ArrayList();
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    treatInfo.Add(new Treatment
                    {
                        treat_id = reader[0].ToString(),
                        clinic = reader[1].ToString(),
                        start_time = (DateTime)reader[2],
                        end_time = (DateTime)reader[3],
                        patient_id = reader[4].ToString(),
                        doc_id = reader[5].ToString(),
                        take = (int)reader[6],
                        pay = (double)reader[7],
                        pay_time = (DateTime)reader[8]
                    });
                }
                return treatInfo;
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }

        public static string GetNameById(string id)
        {
            string sqlStr = String.Format(
                @"select name
                from employee natural join identity
                where employee_id='{0}'", id);
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

            }
            return null;

        }

        public static bool TakeTreat(string employee_id, string treat_id)
        {
            string sqlStr = String.Format(
            @"update treatment
            set take=1
            where treat_id='{0}'", treat_id);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            try
            {
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
            }
            return false;
        }

        public static ArrayList GetAllMedicine()
        {
            ArrayList medicine = new ArrayList();
            string sqlStr =
                @"select * 
                from medicine";
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    medicine.Add(new Medicine
                    {
                        medicine_id = reader[0].ToString(),
                        name = reader[1].ToString(),
                        unit = reader[2].ToString(),
                        stock = (int)reader[3]
                    });
                }
                return medicine;
            }
            catch (Exception e)
            {

            }
            return null;
        }
        // Exam id 木有写完....
        // 写完了
        public static bool WriteExamination(string treat_id, string employee_id, int type)
        {
            if (type < 0 || type > 2) return false;
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            OracleTransaction transaction = cmd.Connection.BeginTransaction();
            cmd.Transaction = transaction;
            string sqlStr;
            try
            {

                sqlStr = @"insert into examination(exam_id, type, exam_time, employee_id)
                values(null, :type, systimestamp, :employee_id)";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add("type", OracleDbType.Int32).Value = type;
                cmd.Parameters.Add("employee_id", OracleDbType.Varchar2, 5).Value = employee_id;
                cmd.ExecuteNonQuery();

                //select related exam_id
                sqlStr = @"select EXAM_INCREMENT.CURRVAL from dual";
                cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
                cmd.CommandText = sqlStr;
                var reader = cmd.ExecuteReader();
                string exam_id = "";
                if (reader.Read())
                    exam_id = reader[0].ToString();

                switch (type)
                {
                    case 2:
                        sqlStr = String.Format(
                        @"insert into XRay 
                        values('{0}', :checkpoint, :from_picture, :picture)", exam_id);
                        cmd = new OracleCommand();
                        // 没加text啊。。
                        cmd.CommandText = sqlStr;
                        cmd.Connection = DatabaseHelper.GetInstance().conn;
                        cmd.Transaction = transaction;
                        cmd.Parameters.Add("checkpoint", OracleDbType.Varchar2, 100).Value = ConstHelper.checkpoint;
                        cmd.Parameters.Add("from_picture", OracleDbType.Varchar2, 2000).Value = ConstHelper.from_picture_XRay;
                        cmd.Parameters.Add("picture", OracleDbType.Varchar2, 200).Value = "";
                        cmd.ExecuteNonQuery();
                        break;
                    case 1:
                        sqlStr = String.Format(
                        @"insert into gastroscope 
                        values('{0}', :from_picture, :diagnoses, :picture)", exam_id);
                        cmd = new OracleCommand();
                        // 没加text啊。。
                        cmd.CommandText = sqlStr;
                        cmd.Connection = DatabaseHelper.GetInstance().conn;
                        cmd.Transaction = transaction;
                        cmd.Parameters.Add("from_picture", OracleDbType.Varchar2, 2000).Value = ConstHelper.from_picture_Gas;
                        cmd.Parameters.Add("diagnoses", OracleDbType.Varchar2, 100).Value = ConstHelper.diagnoses;
                        cmd.Parameters.Add("picture", OracleDbType.Varchar2, 200).Value = "";
                        cmd.ExecuteNonQuery();
                        break;
                    case 0:
                        sqlStr = String.Format(
                        @"insert into blood
                        values('{0}', {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10},
                        {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22})", 
                        exam_id,
                        ConstHelper.wbc, 
                        ConstHelper.neut_percent, 
                        ConstHelper.lymph_percent,
                        ConstHelper.mono_percent, 
                        ConstHelper.eo_percent,
                        ConstHelper.baso_percent,
                        ConstHelper.neut_num, 
                        ConstHelper.lymph_num, 
                        ConstHelper.mono_num, 
                        ConstHelper.eo_num,
                        ConstHelper.baso_num, 
                        ConstHelper.rbc,
                        ConstHelper.hgb,
                        ConstHelper.hct,
                        ConstHelper.mcv,
                        ConstHelper.mch,
                        ConstHelper.mchc, 
                        ConstHelper.rdw,
                        ConstHelper.plt, 
                        ConstHelper.mpv,
                        ConstHelper.pct, 
                        ConstHelper.pdw
                        );
                        cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                        break;
                }
                cmd.Transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
            }
            return false;
        }

        public static bool WritePrescription(string treat_id, string employee_id, ArrayList medicine_id, ArrayList num)
        {
            string sqlStr = @"insert into prescription(pres_id, treat_id, employee_id, make_time, done_time)
                values(null, :treat_id, :employee_id, systimestamp, null)";
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            OracleTransaction transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            cmd.Transaction = transaction;
            cmd.Parameters.Add("treat_id", OracleDbType.Varchar2, 20).Value = treat_id;
            cmd.Parameters.Add("employee_id", OracleDbType.Varchar2, 5).Value = employee_id;
            try
            {
                cmd.ExecuteNonQuery();

                //select pres_id;
                sqlStr = @"select PRESCRIPTION_INCREMENT.CURRVAL from dual";
                cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
                cmd.CommandText = sqlStr;
                OracleDataReader reader = cmd.ExecuteReader();
                string pres_id = "";
                if (reader.Read())
                    pres_id = reader[0].ToString();

                sqlStr = @"insert into prescribe(pres_id, medicine_id, num)
                        values(:pres_id, :medicine_id, :num)";
                for (int i = 0; i < medicine_id.Count; ++i)
                {
                    cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
                    cmd.Transaction = transaction;
                    cmd.Parameters.Add("pres_id", OracleDbType.Varchar2, 20).Value = pres_id;
                    cmd.Parameters.Add("medicine_id", OracleDbType.Varchar2, 20).Value = medicine_id[i];
                    cmd.Parameters.Add("num", OracleDbType.Int32).Value = num[i];
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                // 增加一个throw语句 以用于处理异常
                throw e;
            }
            return false;
        }

        public static bool WriteSurgery(string treat_id, string surgery_name)
        {
            string sqlStr = @"insert into surgery(surg_id, treat_id, surgery_name, start_time, end_time)
                values(null, :treat_id, :surgery_name, systimestamp, null)";
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            cmd.Parameters.Add("treat_id", OracleDbType.Varchar2, 20).Value = treat_id;
            cmd.Parameters.Add("surgery_name", OracleDbType.Varchar2, 100).Value = surgery_name;
            try
            {
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
            }
            return false;
        }
        // 不需要传入nurse_id, bed_num
        // 改成自动分配了
        public static bool WriteHospitalization(string treat_id)
        {
            string sqlStr = @"insert into hospitalization(hos_id, treat_id, employee_id, bed_num, in_time, out_time)
            values(null, :treat_id, :employee_id, :bed_num, systimestamp, null)";
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            cmd.Parameters.Add("treat_id", OracleDbType.Varchar2, 20).Value = treat_id;
            cmd.Parameters.Add("employee_id", OracleDbType.Varchar2, 5).Value = GetFreeNurseId();
            cmd.Parameters.Add("bed_num", OracleDbType.Char, 3).Value = GetFreeBedNum();
            try
            {
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
                throw e;
            }
            return false;
        }

        public static string GetFreeNurseId()
        {
            string sqlStr = "select employee_id from employee where post='护士'";
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

            }
            return null;
        }

        public static string GetFreeBedNum()
        {
            string sqlStr = "select bed_num from bed where used=0";
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string bed_num = reader[0].ToString();
                    sqlStr = String.Format("update bed set used=1 where bed_num='{0}'", bed_num);
                    cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
                    cmd.ExecuteNonQuery();
                    return bed_num;
                }
            }
            catch (Exception e)
            {

            }
            return null;
        }

        //private static bool SetBedUsed(string bed_num)
        //{
        //    string sqlStr = String.Format("update bed set used=1 where bed_num='{0}'", bed_num);
        //    OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
        //    cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
        //    try
        //    {
        //        cmd.ExecuteNonQuery();
        //        cmd.Transaction.Commit();
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        cmd.Transaction.Rollback();
        //    }
        //    return false;
        //}
    }
}