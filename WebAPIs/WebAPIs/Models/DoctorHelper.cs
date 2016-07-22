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
        /// <summary>
        /// 没测过
        /// </summary>
        /// <param name="clinic"></param>
        /// <returns></returns>
        public static ArrayList AllRoomOfClinic(string clinic)
        {
            ArrayList rooms = new ArrayList();
            string sqlStr = @"select room_num 
                    from clinic natural join own
                    where clinic_name=:clinic";
            OracleCommand cmd = DatabaseHelper.GetInstance().conn.CreateCommand();
            cmd.CommandText = sqlStr;
            cmd.Parameters.Add("clinic", OracleDbType.Varchar2, 20).Value = clinic;
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rooms.Add(reader[0].ToString());
                }
                return rooms;
            }
            catch (Exception e)
            {

            }
            return null;
        }

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
                        start_time = Formater.ToDateTime(reader, 2),
                        end_time = Formater.ToDateTime(reader, 3),
                        patient_id = reader[4].ToString(),
                        doc_id = reader[5].ToString(),
                        take = int.Parse(reader[6].ToString()),
                        pay = double.Parse(reader[7].ToString()),
                        pay_time = Formater.ToDateTime(reader, 8)
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
            string sqlStr = "select get_name(:id, 1) from dual";
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            cmd.Parameters.Add("id", id);
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
                var num = cmd.ExecuteNonQuery();
                if (num != 1)
                {
                    cmd.Transaction.Rollback();
                    throw new Exception("需要更改的id不存在，更改的Num:" + num.ToString());
                }
                cmd.Transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                throw e;
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
                        // $exception	{"Specified cast is not valid."}	System.InvalidCastException
                        // 这怎么可以强制类型转换。。。直接Parse更好
                        stock = int.Parse(reader[3].ToString())
                    });
                }
                return medicine;
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }
        // Exam id 木有写完....
        // 写完了
        // Test Passed
        public static bool WriteExamination(string treat_id, string employee_id, int type)
        {
            if (type < 0 || type > 2) return false;
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = cmd.Connection.BeginTransaction();
            string sqlStr;
            try
            {
                sqlStr = @"insert into examination(exam_id, type, exam_time, employee_id, pay)
                values(null, :type, systimestamp, :employee_id, 15)";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add("type", OracleDbType.Int32).Value = type;
                cmd.Parameters.Add("employee_id", OracleDbType.Varchar2, 5).Value = GetFreeExaminerId();
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();

                //select related exam_id
                sqlStr = @"select EXAM_INCREMENT.CURRVAL from dual";
                cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
                cmd.CommandText = sqlStr;
                var reader = cmd.ExecuteReader();
                string exam_id = "";
                if (reader.Read())
                    exam_id = reader[0].ToString();

                sqlStr = String.Format("insert into examine values('{0}', '{1}')", exam_id, treat_id);
                cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
                cmd.Transaction = cmd.Connection.BeginTransaction();
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
                cmd.Transaction.Commit();

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
                    cmd.Transaction = cmd.Connection.BeginTransaction();
                    cmd.Parameters.Add("pres_id", OracleDbType.Varchar2, 20).Value = pres_id;
                    cmd.Parameters.Add("medicine_id", OracleDbType.Varchar2, 20).Value = medicine_id[i];
                    cmd.Parameters.Add("num", OracleDbType.Int32).Value = num[i];
                    cmd.ExecuteNonQuery();
                    cmd.Transaction.Commit();

                    cmd = DatabaseHelper.GetInstance().conn.CreateCommand();
                    cmd.Transaction = cmd.Connection.BeginTransaction();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "use_medicine";
                    cmd.Parameters.Add("id", OracleDbType.Varchar2).Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters["id"].Value = medicine_id[i];
                    cmd.Parameters.Add("num", OracleDbType.Int32).Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters["num"].Value = num[i];
                    cmd.ExecuteNonQuery();
                    cmd.Transaction.Commit();

                }
                return true;
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
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
                throw e;
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
            ArrayList list = new ArrayList();
            string sqlStr = "select employee_id from employee where post='Nurse'";
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader[0].ToString());
                }
                Random rand = new Random();
                return list[rand.Next(0, list.Count - 1)].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;
        }
        public static string GetFreeExaminerId()
        {
            ArrayList list = new ArrayList();
            string sqlStr = "select employee_id from employee where post='Examiner'";
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader[0].ToString());
                }
                Random rand = new Random();
                return list[rand.Next(0, list.Count - 1)].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;
        }

        public static string GetFreeBedNum()
        {
            
            string sqlStr = "select get_free_bed() from dual";
            sqlStr = "select bed_num from bed where used = 0";
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string bed_num = reader[0].ToString();
                    if (bed_num.Equals("")) return null;
                    cmd = DatabaseHelper.GetInstance().conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "set_bed_used";
                    cmd.Parameters.Add("num", OracleDbType.Varchar2).Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters["num"].Value = bed_num;
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