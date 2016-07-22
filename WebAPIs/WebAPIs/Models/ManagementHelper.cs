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
                 set dept_name=:Pdep,clinic_name=:Pcl,post=:Ppos,salary=:Psal
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
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取投诉率高于percent的医生
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="percent"></param>
        /// <returns>医生组成的ArrayList</returns>
        public static ArrayList GetComplaintedDoctor(int year, int month, double percent)//获取投诉率高于percent的医生
        {
            // 投诉率处理
            percent = 1 - percent / 100;
            percent *= 5;
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
                        select employee.dept_name,employee.clinic_name,badDoc.id,identity.name,(5-badDoc.rate)*20
                        from badDoc join employee on badDoc.id=employee.employee_id join identity on employee.credit_num=identity.credit_num
                        where rate<'{2}'",
                        beginTime, endTime, percent);
            cmd.CommandText = sqlStr;
            OracleDataReader reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    ComplaintedDoctor.Add(new EmployeeInfo(reader[0].ToString(), reader[1].ToString(),
                                    reader[2].ToString(), reader[3].ToString(),
                                    Convert.ToDouble(reader[4])));
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

        public static bool insertDuty(Duty item)
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
            {
                cmd.Transaction.Commit();
                return true;
            }
        }


        //public static bool SetDuty(Duty item)
        //{
        //    OracleCommand cmd = new OracleCommand();
        //    cmd.Connection = DatabaseHelper.GetInstance().conn;
        //    cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
        //    string sqlStr =
        //        @"insert into duty
        //        values (null, :Proom_num, :Pmon, Ptue, Pwed, Pthu, Pfri, Psat, Psun)";
        //    cmd.CommandText = sqlStr;
        //    cmd.Parameters.Add("Proom_num", item.room_num);
        //    cmd.Parameters.Add("Pmon", item.Monday);
        //    cmd.Parameters.Add("Ptue", item.Tuesday);
        //    cmd.Parameters.Add("Pwed", item.Wednesday);
        //    cmd.Parameters.Add("Pthu", item.Thursday);
        //    cmd.Parameters.Add("Pfri", item.Friday);
        //    cmd.Parameters.Add("Psat", item.Saturday);
        //    cmd.Parameters.Add("Psun", item.Sunday);
        //    if (cmd.ExecuteNonQuery() == 1)
        //    {
        //        cmd.Transaction.Commit();
        //        return true;

        //    }
        //    else
        //    {
        //        cmd.Transaction.Rollback();
        //        return false;
        //    }
        //}
        public static bool SetDuty(Duty item, string employee_id)
        {
            try
            {
                insertDuty(item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            OracleCommand geetDutycmd = new OracleCommand();
            geetDutycmd.Connection = DatabaseHelper.GetInstance().conn;
            string sqlStr1 = @"select DUTY_INCREMENT.CURRVAL from dual";
            geetDutycmd.CommandText = sqlStr1;
            OracleDataReader reader1 = geetDutycmd.ExecuteReader();
            string duty_id = "";
            try
            {
                reader1.Read();
                duty_id = reader1[0].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            string sqlStr = String.Format(@"update employee
                            set duty_id={0}
                            where employee_id=:employee", duty_id);
            cmd.CommandText = sqlStr;
            cmd.Parameters.Add("employee", OracleDbType.Varchar2, 5).Value = employee_id;
            if (cmd.ExecuteNonQuery() != 1)
            {
                cmd.Transaction.Rollback();
                return false;
            }
            else
            {
                cmd.Transaction.Commit();
                return true;
            }
            return false;
        }


        static public bool AddEmployee(Identity identity, Employee item)
        {
            try
            {
                SignUpEmployee(identity);
            }
            catch (Exception e)
            {
                throw e;
            }
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = DatabaseHelper.GetInstance().conn;
            cmd.Transaction = DatabaseHelper.GetInstance().conn.BeginTransaction();
            try
            {
                string sqlStr =
                @"Insert into EMPLOYEE (EMPLOYEE_ID,CREDIT_NUM,PASSWORD,DEPT_NAME,CLINIC_NAME,POST,SALARY,DUTY_ID,AVATAR_PATH,PROFILE,SKILL)
                    values (null,:credit_num, :password, :dept_name, :clinic_name, :post, :salary,null,null,null,null)";
                cmd.CommandText = sqlStr;
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
            }
            return false;
        }
        private string GetRandomAvatarPath()
        {
            string path = @"..\..\image\avatars\avatar (";
            Random rand = new Random();
            switch (rand.Next(1, 9))
            {
                case 1:
                    path += "1";
                    break;
                case 2:
                    path += "2";
                    break;
                    path += "3";
                case 3:
                    break;
                    path += "4";
                case 4:
                    break;
                    path += "5";
                case 5:
                    break;
                    path += "6";
                case 6:
                    break;
                    path += "7";
                case 7:
                    break;
                    path += "8";
                case 8:
                    break;
                    path += "9";
                case 9:
                    break;
                default:
                    path += "9";
                    break;
            }
            path += @").jpg";
            return path;
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

        public static string SignUpEmployee(Identity item)
        {
            //check if the credit_num is used
            string sqlStr = @"select count_credit(:credit_num) from dual";

            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
            cmd.Parameters.Add("credit_num", item.credit_num);
            OracleDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (Convert.ToInt32(reader[0]) > 0)
                    //查询语句不需要回滚
                    throw new Exception("Already exise");
            }

            var strBirth = item.birth.Year.ToString() + "/" + item.birth.Month.ToString() + "/" + item.birth.Day.ToString();
            //sign up employee
            try
            {
                sqlStr = "insert into identity values (:credit_num, :name, :sex, to_date('"
                    + strBirth + "', 'yyyy/mm/dd'))";
                cmd = new OracleCommand(sqlStr, DatabaseHelper.GetInstance().conn);
                cmd.Transaction = cmd.Connection.BeginTransaction();
                cmd.CommandText = sqlStr;

                cmd.Parameters.Add("credit_num", item.credit_num);
                cmd.Parameters.Add("name", item.name);
                cmd.Parameters.Add("sex", item.sex);
                if (1 != cmd.ExecuteNonQuery())
                {
                    throw new Exception("插入失败！");
                }

                cmd.Transaction.Commit();
            }
            catch (Exception e)
            {
                cmd.Transaction.Rollback();
                throw new Exception("Insert employee into identity table failed, message:" + e.Message + " Birth format:" + strBirth);
            }
            return "Ok";
        }
    }
}