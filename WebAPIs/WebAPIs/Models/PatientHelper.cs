﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPIs.Providers;
using System.Collections;
using WebAPIs.Models.DataModels;
using WebAPIs.Models.UnifiedTable;
using Oracle.ManagedDataAccess.Client;

namespace WebAPIs.Models
{
    public class PatientHelper
    {

        public static ArrayList GetAllClinic()
        {
            ArrayList clinics = new ArrayList();
            string sqlStr = String.Format(
               @"select clinic_name
                from clinic
               ");
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            OracleDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    clinics.Add(reader[0].ToString());
                }
            }
            catch (Exception e)
            {

            }
            return clinics;
        }

        //only contain department, clinic, post, name, sex info
        public static ArrayList GetEmployeeOfClinic(string clinic_name)
        {
            ArrayList list = new ArrayList();
            string sqlStr = String.Format(
                @"select avatar_path, employee_id, name, post, skill
                  from employee natural join identity
                  where employee.clinic_name = '{0}' and employee.salary !=-1", clinic_name);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    EmployeeInfoWithRank employeeInfoWithRank = new EmployeeInfoWithRank();
                    employeeInfoWithRank.pic_url = reader[0].ToString();
                    employeeInfoWithRank.employee_id = reader[1].ToString();
                    employeeInfoWithRank.name = reader[2].ToString();
                    employeeInfoWithRank.post = reader[3].ToString();
                    employeeInfoWithRank.skill = reader[4].ToString();
                    list.Add(employeeInfoWithRank);
                }

            }
            catch(Exception e)
            {
                throw e;
            }
            for(int i = 0; i < list.Count; i++)
            {
                EmployeeInfoWithRank employeeInfoWithRank = (EmployeeInfoWithRank)list[i];
                string subSqlStr = String.Format(
                    @"select count(*), sum(rank)
                      from evaluation
                      where evaluation.employee_id = '{0}'",employeeInfoWithRank.employee_id);

                OracleCommand subCmd = new OracleCommand(subSqlStr, DatabaseHelper.GetInstance().conn);

                try
                {
                    OracleDataReader reader = subCmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (Convert.ToInt32(reader[0]) == 0)
                        {
                            employeeInfoWithRank.rank = -1;
                        }
                        else
                        {
                            employeeInfoWithRank.rank = (int)(Convert.ToInt32(reader[1]) / Convert.ToInt32(reader[0]));
                        }
                    }
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
            return list;
        }

        public static Duty GetEmployeeDutyTime(string id)
        {
            string sqlStr = String.Format(
              @"select room_num, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
                from employee natural join duty
                where employee_id='{0}'",
                id);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            OracleDataReader reader = cmd.ExecuteReader();
            try
            {
                if (reader.Read())
                {
                    return new Duty(
                        reader[0].ToString(),
                        reader[1].ToString(),
                        reader[2].ToString(),
                        reader[3].ToString(),
                        reader[4].ToString(),
                        reader[5].ToString(),
                        reader[6].ToString(),
                        reader[7].ToString());
                }
            }
            catch (Exception e)
            {

            }
            return null;
        }

        //Test Passed
        public static string RegisterTreat(Treatment treat)
        {
            OracleCommand cmd = DatabaseHelper.GetInstance().conn.CreateCommand();
            cmd.Transaction = cmd.Connection.BeginTransaction();
            try
            {
                string sqlStr = String.Format("insert into treatment(treat_id, clinic_name, start_time, end_time, patient_id, employee_id, take)" +
                    "values(null, :clinic_name, to_timestamp('{0}', 'yyyy/mm/dd hh24:mi:ss'), to_timestamp('{1}','yyyy/mm/dd hh24:mi:ss'), :patient_id, :employee_id, :take)",
                    Formater.ToString(treat.start_time),
                    Formater.ToString(treat.end_time));
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add("clinic_name", OracleDbType.Varchar2, 20).Value = treat.clinic;
                cmd.Parameters.Add("patient_id", OracleDbType.Varchar2, 9).Value = treat.patient_id;
                cmd.Parameters.Add("employee_id", OracleDbType.Varchar2, 5).Value = treat.doc_id;
                cmd.Parameters.Add("take", OracleDbType.Int32).Value = 0;
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();

                string sql = "select TREATMENT_INCRE.currval from dual";
                cmd = DatabaseHelper.GetInstance().conn.CreateCommand();
                cmd.CommandText = sql;
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return reader[0].ToString();
                }
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
            }
            return null;
        }
        public static bool Comment(Evaluation item)
        {
            ArrayList doc_id_name = GetDoctorIdName(item.treat_id);
            item.doc_id = doc_id_name[0].ToString();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = cmd.Connection.BeginTransaction();
            try
            {
                string sqlStr =
                  @"insert into evaluation
                values(:treat_id, :patient_id, :employee_id, :rank, :content)";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add("treat_id", OracleDbType.Varchar2, 20).Value = item.treat_id;
                cmd.Parameters.Add("patient_id", OracleDbType.Varchar2, 9).Value = item.patient_id;
                cmd.Parameters.Add("employee_id", OracleDbType.Varchar2, 5).Value = item.doc_id;
                cmd.Parameters.Add("rank", OracleDbType.Int32).Value = item.rank;
                cmd.Parameters.Add("content", OracleDbType.Varchar2, 2000).Value = item.content;
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

        //根据treatment_id，返回doctorID&name
        public static ArrayList GetDoctorIdName(string treatment_id)
        {
            string sqlStr = String.Format(
                @"select employee_id, name
                 from treatment natural join (employee natural join identity)
                 where treat_id ='{0}'", treatment_id);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            ArrayList doctorIdName = new ArrayList();
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();//写在里面防止reader抛exception
                if (reader.Read())
                {
                    doctorIdName.Add(reader[0].ToString());
                    doctorIdName.Add(reader[1].ToString());
                    return doctorIdName;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }

        public static string GetNameById(string id)
        {
            string sqlStr = String.Format(@"select get_name('{0}', 0) from dual", id);
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
                throw e;
            }
            return null;

        }

        public static string GetDoctorNameById(string id)
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

        //Test Passed
        public static ArrayList GetTreatmentInfo(string patient_id)
        {
            string sqlStr = String.Format(@"select * from treatment where patient_id='{0}'",
            patient_id);

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
                        take = Convert.ToInt32(reader[6]),
                        pay = Convert.ToDouble(reader[7]),
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

        public static ArrayList GetAllConsumption(string treat_id)
        {
            ArrayList consumptionInfo = new ArrayList();
            try
            {
                #region
                ArrayList treatInfo = new ArrayList();
                string sqlStr = String.Format(
                  @"select *
                 from treatment
                 where treat_id ='{0}'", treat_id);
                OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
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
                        take = Convert.ToInt32(reader[6]),
                        pay = Convert.ToDouble(reader[7]),
                        pay_time = Formater.ToDateTime(reader, 8)
                    });
                }
                #endregion

                #region
                ArrayList examInfo = new ArrayList();
                sqlStr = String.Format(
                  @"select exam_id, type, exam_time, employee_id, pay, pay_time
                 from examination natural join examine
                 where treat_id ='{0}'", treat_id);

                cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
                OracleDataReader reader_1 = cmd.ExecuteReader();
                while (reader_1.Read())
                {
                    examInfo.Add(new Examination
                    {
                        exam_id = reader_1[0].ToString(),
                        type = reader_1[1].ToString(),
                        exam_time = Formater.ToDateTime(reader_1, 2),
                        employee_id = reader_1[3].ToString(),
                        pay = Convert.ToDouble(reader_1[4]),
                        pay_time = Formater.ToDateTime(reader_1, 5)
                    });
                }

                #endregion

                #region
                ArrayList presInfo = new ArrayList();
                sqlStr = String.Format(
                    @"select *
                    from prescription
                    where treat_id='{0}'", treat_id);
                cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
                OracleDataReader reader_2 = cmd.ExecuteReader();
                while (reader_2.Read())
                {
                    presInfo.Add(new Prescription
                    {
                        pres_id = reader_2[0].ToString(),
                        treat_id = reader_2[1].ToString(),
                        employee_id = reader_2[2].ToString(),
                        make_time = Formater.ToDateTime(reader_2, 3),
                        done_time = Formater.ToDateTime(reader_2, 4),
                        pay = Convert.ToDouble(reader_2[5]),
                        pay_time = Formater.ToDateTime(reader_2, 6)
                    });
                }
                #endregion

                #region
                ArrayList surInfo = new ArrayList();
                sqlStr = String.Format(
                    @"select *
                    from surgery
                    where treat_id='{0}'", treat_id);
                cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
                OracleDataReader reader_3 = cmd.ExecuteReader();
                while (reader_3.Read())
                {
                    surInfo.Add(new Surgery
                    {
                        surg_id = reader_3[0].ToString(),
                        treat_id = reader_3[1].ToString(),
                        surgery_name = reader_3[2].ToString(),
                        start_time = Formater.ToDateTime(reader_3, 3),
                        end_time = Formater.ToDateTime(reader_3, 4),
                        pay = Convert.ToDouble(reader_3[5]),
                        pay_time = Formater.ToDateTime(reader_3, 6)
                    });
                }
                #endregion

                #region

                ArrayList hosInfo = new ArrayList();
                sqlStr = String.Format(
                    @"select *
                    from hospitalization
                    where treat_id='{0}'", treat_id);
                cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
                OracleDataReader reader_4 = cmd.ExecuteReader();
                while (reader_4.Read())
                {
                    hosInfo.Add(new Hospitalization
                    {
                        hos_id = reader_4[0].ToString(),
                        treat_id = reader_4[1].ToString(),
                        nurse_id = reader_4[2].ToString(),
                        bed_num = reader_4[3].ToString(),
                        in_time = Formater.ToDateTime(reader_4, 4),
                        out_time = Formater.ToDateTime(reader_4, 5),
                        pay = Convert.ToDouble(reader_4[6]),
                        pay_time = Formater.ToDateTime(reader_4, 7)
                    });
                }
                #endregion

                consumptionInfo.Add(treatInfo);
                consumptionInfo.Add(examInfo);
                consumptionInfo.Add(presInfo);
                consumptionInfo.Add(surInfo);
                consumptionInfo.Add(hosInfo);

                return consumptionInfo;
            }
            catch (Exception e)
            {
                throw e;
            }
            return consumptionInfo;
        }


        public static ArrayList GetCommentByDocId(string docId)
        {
            string sqlStr = String.Format(
               @"select content, name, avatar_path
                  from evaluation, patient natural join identity
                  where employee_id='{0}' and evaluation.patient_id=patient.patient_id", docId);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            ArrayList commentList = new ArrayList();
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CommentInfo commentInfo = new CommentInfo();
                    commentInfo.comment = reader[0].ToString();
                    commentInfo.patientName = reader[1].ToString();
                    commentInfo.pic_url = reader[2].ToString();
                    commentList.Add(commentInfo);
                }
                return commentList;
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

        public static EmployeeWithComment GetDoctor(string docId)
        {
            string sqlStr = string.Format(
                @"select employee.avatar_path, identity.name, identity.sex, employee.clinic_name, employee.post, employee.profile
                  from employee natural join identity
                  where employee.employee_id = '{0}'",docId);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            EmployeeWithComment employeeWithComment = new EmployeeWithComment();
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    employeeWithComment.pic_url = reader[0].ToString();
                    employeeWithComment.name = reader[1].ToString();
                    employeeWithComment.sex = reader[2].ToString();
                    employeeWithComment.clinic = reader[3].ToString();
                    employeeWithComment.post = reader[4].ToString();
                    employeeWithComment.profile = reader[5].ToString();
                    return employeeWithComment;
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