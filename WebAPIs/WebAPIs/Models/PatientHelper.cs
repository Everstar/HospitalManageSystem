using System;
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
            ArrayList employees = new ArrayList();
            string sqlStr = String.Format(
               @"select dept_name, clinic_name, post, name, sex
                from employee natural join identity natural join clinic
                where clinic_name='{0}'",
                clinic_name);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            OracleDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    employees.Add(new EmployeeInfo(
                        reader[0].ToString(),
                        reader[1].ToString(),
                        reader[2].ToString(),
                        reader[3].ToString(),
                        reader[4].ToString()));
                }
            }
            catch (Exception e)
            {

            }
            return employees;
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


        public static int RegisterTreat(Treatment treat)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            try
            {
                string sqlStr = String.Format(@"insert into treatment 
                    values(:clinic_name, to_date('{0}', 'yyyy-mm-dd hh24:mi:ss'), to_date('{1}', 'yyyy-mm-dd hh24:mi:ss'), :patient_id, :employee_id, :take)",
                    treat.start_time.ToString("yyyy-mm-dd hh24:mi:ss"),
                    treat.end_time.ToString("yyyy-mm-dd hh24:mi:ss"));
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add("clinic_name", OracleDbType.Varchar2, 20).Value = treat.clinic;
                cmd.Parameters.Add("patient_id", OracleDbType.Varchar2, 20).Value = treat.patient_id;
                cmd.Parameters.Add("take", OracleDbType.Int32).Value = 0;
                cmd.Parameters.Add("employee_id", OracleDbType.Varchar2, 20).Value = treat.doc_id;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
                return -1;
            }

            //select the max id;
            return 0;
        }

        public static bool Comment(Evaluation item)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            try
            {
                string sqlStr = String.Format(
                  @"insert into evaluation
                values('{0}','{1}','{2}', {3}, '{4}')
               ", item.treat_id, item.patient_id, item.doc_id, item.rank, item.content);
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

        //根据treatment_id，返回doctorID&name
        public static ArrayList GetDoctorIdName(string treatment_id)
        {
            string sqlStr = String.Format(
                @"select employee_id, name
                 from treatment natural join (employee natural join identity)
                 where treat_id ={0}", treatment_id);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);

            ArrayList doctorIdName = new ArrayList();
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    doctorIdName.Add(reader[0].ToString());
                    doctorIdName.Add(reader[1].ToString());
                    return doctorIdName;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

        public static ArrayList GetTreatmentInfo(string patient_id, int month, int year)
        {
            DateTime minTime = new DateTime(year, month, 1);
            DateTime maxTime = new DateTime(year, month == 12 ? 1 : month + 1, 1);
            string sqlStr = String.Format(@"select * from treatment where patient_id='{0}' 
            and end_time >= to_date('{1}', 'yyyy-mm-dd hh24:mi:ss') and end_time<to_date('{2}', 'yyyy-mm-dd hh24:mi:ss')",
            patient_id,
            minTime.ToString("yyyy-mm-dd hh24:mi:ss"),
            maxTime.ToString("yyyy-mm-dd hh24:mi:ss"));

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
                        pay = (Double)reader[7],
                        pay_time = (DateTime)reader[8]
                    });
                }
                return treatInfo;
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public static ArrayList GetAllConsumption(string patient_id, string treat_id)
        {
            ArrayList consumptionInfo = new ArrayList();
            try
            {
                #region
                ArrayList treatInfo = new ArrayList();
                string sqlStr = String.Format(
                  @"select *
                 from treatment
                 where treat_id ={0}", treat_id);
                OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
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
                        pay = (Double)reader[7],
                        pay_time = (DateTime)reader[8]
                    });
                }
                #endregion

                #region
                ArrayList examInfo = new ArrayList();
                sqlStr = String.Format(
                  @"select exam_id, type, exam_time, employee_id, pay, pay_time
                 from examination natural join examine
                 where treat_id ={0}", treat_id);

                cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
                OracleDataReader reader_1 = cmd.ExecuteReader();
                while (reader_1.Read())
                {
                    examInfo.Add(new Examination{
                        exam_id = reader_1[0].ToString(),
                        type = reader_1[1].ToString(),
                        exam_time = (DateTime)reader_1[2],
                        employee_id =reader_1[3].ToString(),
                        pay = (double)reader_1[4],
                        pay_time = (DateTime)reader_1[5]
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
                    presInfo.Add(new Prescription{
                    pres_id = reader_2[0].ToString(),
                    treat_id = reader_2[1].ToString(),
                    parm_id =reader_2[2].ToString(),
                    make_time =(DateTime)reader_2[3],
                    done_time = (DateTime)reader_2[4],
                    pay =(double) reader_2[5],
                    pay_time = (DateTime)reader_2[6]});
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
                        start_time = (DateTime)reader_3[3],
                        end_time = (DateTime)reader_3[4],
                        pay = (double)reader_3[5],
                        pay_time = (DateTime)reader_3[6]
                    });
                }
                #endregion

                #region 

                ArrayList hosInfo = new ArrayList();
                sqlStr = String.Format(
                    @"select *
                    from surgery
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
                        bed_num  = reader_4[3].ToString(),
                        in_time = (DateTime)reader_4[4],
                        out_time = (DateTime)reader_4[5],
                        pay = (double)reader_4[6],
                        pay_time = (DateTime)reader_4[7]
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

            }
            return null;
        }
    }

}