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
            // 把工资为-1的员工都过滤
            string sqlStr = String.Format(
               @"select employee_id,name,dept_name,clinic_name,post,salary
                from employee natural join identity where salary != -1");
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.CommandText = sqlStr;
            OracleDataReader reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    AllEmployee.Add(new EmployeeInfo(reader[0].ToString(), reader[1].ToString(),
                                    reader[2].ToString(), reader[3].ToString(),
                                    reader[4].ToString(), Convert.ToDouble(reader[5])));
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
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                return true;
            }

            //异常这里我还要研究一下
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                return false;
            }
            return true;

        }


        //获取投诉率高于percent的医生


        public static ArrayList GetComplaintedDoctor(int year, int month, double percent)//获取投诉率高于percent的医生
        {
            percent = 1 - percent / 100;
            int begin_year = year;
            int begin_month = month;
            int end_year, end_month;
            if (begin_month == 12)
            {
                end_month = 1;
                end_year = begin_year + 1;
            }
            else
            {
                end_month = begin_month + 1;
                end_year = begin_year;
            }
            string beginTime, endTime;
            if (begin_month < 10)
            {
                beginTime = begin_year.ToString() + "-0" + begin_month.ToString();
            }
            else
            {
                beginTime = begin_year.ToString() + "-" + begin_month.ToString();
            }
            if (end_month < 10)
            {
                endTime = end_year.ToString() + "-0" + end_month.ToString();
            }
            else
            {
                endTime = end_year.ToString() + "-" + end_month.ToString();
            }
            ArrayList ComplaintedDoctor = new ArrayList();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            string sqlStr = String.Format(
                        @"with badDoc(id,rate) as 
                                (select evaluation.employee_id,avg(rank)
                                from evaluation join treatment on evaluation.treat_id=treatment.treat_id
                                where treatment.end_time>to_date('{0}', 'yyyy-mm') and treatment.end_time<to_date('{1}', 'yyyy-mm')
                                group by evaluation.employee_id)
                        select employee.dept_name,employee.clinic_name,badDoc.id,identity.name,badDoc.rate
                        from badDoc join employee on badDoc.id=employee.employee_id join identity on employee.credit_num=identity.credit_num
                        where rate<'{2}'",
                        beginTime, endTime,percent);
            cmd.CommandText = sqlStr;
            OracleDataReader reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    ComplaintedDoctor.Add(new EmployeeInfo(reader[0].ToString(), reader[1].ToString(),
                                    reader[2].ToString(), reader[3].ToString(),
                                    Convert.ToDouble(reader[5])));
                }
                return ComplaintedDoctor;
            }
            catch (Exception e)
            {
                throw e;
                return null;
            }
            return null;


        }


        public static bool SetDuty(Duty item)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            string sqlStr =
                @"insert into duty
                values (null, :Proom_num, :Pmon, Ptue, Pwed, Pthu, Pfri, Psat, Psun)";
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
        static public bool AddEmployee(Employee item)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            try
            {
                string sqlStr =
                @"insert into employee(exployee_id, credit_num, password, dept_name, clinic_name, post, salary,duty_id)
                  values (null, :credit_num, :password, :dept_name, :clinic_name, :post, :salary,null);
                ";
                cmd.Parameters.Add("credit_num", OracleDbType.Varchar2, 18).Value = item.credit_num;
                cmd.Parameters.Add("password", OracleDbType.Varchar2, 20).Value = item.password;
                cmd.Parameters.Add("dept_name", OracleDbType.Varchar2, 20).Value = item.department;
                cmd.Parameters.Add("clinic_name", OracleDbType.Varchar2, 20).Value = item.clinic;
                cmd.Parameters.Add("post", OracleDbType.Varchar2, 20).Value = item.post;
                cmd.Parameters.Add("salary", OracleDbType.Double).Value = item.salary;
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                throw ex;
                return false;
            }
            return false;
        }
        static public bool DeleteEmployee(string employee_id)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            try
            {
                string sqlStr =
              @"update employee
                 set salary=-1
                where employee_id=:employeeid";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add("employeeid", OracleDbType.Varchar2, 5).Value = employee_id;
                if (cmd.ExecuteNonQuery() != 1)
                {
                    throw new Exception("更新失败！");
                }
                else
                {
                    cmd.Transaction.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                throw ex;
                return false;
            }
            return false;
        }


        static public ArrayList GetDutyByClinic(string clinic)
        {
            ArrayList dutyByClinic = new ArrayList();
            string sqlStr =
               @"select employee_id,name,room_num,monday,tuesday,wednesday,thursday,friday,saturday,sunday
                from own natural join duty natural join employee natural join identity
                where clinic_name=':clinic'";
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.CommandText = sqlStr;
            cmd.Parameters.Add("clinic", OracleDbType.Varchar2, 20).Value = clinic;
            OracleDataReader reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    dutyByClinic.Add(new DutyByClinic(reader[0].ToString(), reader[1].ToString(),
                                    reader[2].ToString(), reader[3].ToString(),
                                    reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), reader[8].ToString(),
                                    reader[9].ToString()));
                }
                return dutyByClinic;
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
    }
}