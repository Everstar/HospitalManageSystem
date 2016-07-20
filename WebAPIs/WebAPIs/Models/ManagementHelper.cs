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
    public class ManagementHelper
    {
        //string dept_name, string clinic_name, string post, string name, string sex
        static public ArrayList GetAllEmployee()
        {
            ArrayList AllEmployee = new ArrayList();
            string sqlStr = String.Format(
               @"select dept_name,clinic_name,post,name,sex
                from employee;");
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AllEmployee.Add(new EmployeeInfo(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                        reader[3].ToString(), reader[4].ToString()));
                }
                return AllEmployee;
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

        public static bool SetEmployee(string employee_id, string department, string clinic, string post, double salary)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            try
            {
                string sqlStr =
                @"update employee
                 set department=:Pdep,clinic=:Pcl,post=:Ppos,salary=:Psal
                 where employee_id=:Pemp";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add("Pdep", department);
                cmd.Parameters.Add("Pcl", clinic);
                cmd.Parameters.Add("Ppos", post);
                cmd.Parameters.Add("Psal", salary);
                cmd.Parameters.Add("Pemp", employee_id);

            }

            //异常这里我还要研究一下
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }



        //获取投诉率高于percent的医生


        public static ArrayList GetComplaintedDoctor(double percent)//获取投诉率高于percent的医生
        {
            ArrayList ComplaintedDoctor = new ArrayList();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            string sqlStr = String.Format(
               @"with ComplaintedDoctor(em_id,em_percent) as
                 (select employee_id ,avg(rank)
                  from evaluation
                  group by employee_id)
                  select employee_id,dept_name,clinic_name,post,name,sex
                  from ComplaintedDoctor natural join employee
                  where em_percent>='{0}'", percent);
            cmd.CommandText = sqlStr;


            int i = 0;

            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (i == 0)
                    {
                        ComplaintedDoctor.Add(1);
                        ComplaintedDoctor.Add(new EmployeeInfo(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                            reader[3].ToString(), reader[4].ToString(), reader[5].ToString()));
                        i++;
                    }
                    else
                    {
                        ComplaintedDoctor.Add(new EmployeeInfo(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                            reader[3].ToString(), reader[4].ToString(), reader[5].ToString()));
                    }

                }
                if (i == 0)
                {
                    ComplaintedDoctor.Add(0);
                }
                return ComplaintedDoctor;
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }


        //duty类的id什么时候设置

        public static bool SetDuty(Duty item)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            string sqlStr =
                @"insert into duty
                values (null, :Proom_num, :Pmon, Ptue, Pwed, Pthu, Pfri, Psat, Psun)";
            cmd.Parameters.Add("Pduty_id", item.duty_id);
            cmd.CommandText = sqlStr;
            cmd.Parameters.Add("Proom_num", item.room_num);
            cmd.Parameters.Add("Pmon", item.Monday);
            cmd.Parameters.Add("Ptue", item.Tuesday);
            cmd.Parameters.Add("Pwed", item.Wednesday);
            cmd.Parameters.Add("Pthu", item.Thursday);
            cmd.Parameters.Add("Pfri", item.Friday);
            cmd.Parameters.Add("Psat", item.Saturday);
            cmd.Parameters.Add("Psun", item.Sunday);
            if (cmd.ExecuteNonQuery() == 0)
            {
                cmd.Transaction.Rollback();
                return false;
            }
            else
                return true;
        }
    }
}