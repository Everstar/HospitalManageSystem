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
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
            OracleDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    AllEmployee.Add(new EmployeeInfo(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                        reader[3].ToString(), reader[4].ToString()));
                }
                return AllEmployee;
            }
            catch (Exception e)
            {

            }
            return null;
        }
        /* 人事调配 但是我没看懂API 所以大家先忽略···
        static bool bool SetEmployee(string, string, string, double)
        {

        }
        */
        
            //获取投诉率高于percent的医生
        public static ArrayList GetComplaintedDoctor(double percent)
        {
            ArrayList ComplaintedDoctor = new ArrayList();
            string sqlStr = String.Format(
               @"with ComplaintedDoctor(em_id,em_percent) as
                 (select employee_id ,avg(rank)
                  from evaluation
                  group by employee_id)
                  select employee_id,dept_name,clinic_name,post,name,sex
                  from ComplaintedDoctor natural join employee natural join identity
                  where em_percent<={0}",percent);
            OracleCommand cmd = new OracleCommand(sqlStr, DatabaseHelper.Connection);
            OracleDataReader reader = cmd.ExecuteReader();

            int i = 0;

            try
            {
                while (reader.Read())
                {
                    if (i == 0)
                    {
                        ComplaintedDoctor.Add(1);
                        ComplaintedDoctor.Add(new EmployeeInfo(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                            reader[3].ToString(), reader[4].ToString(), reader[5].ToString()));
                    }
                    else
                    {
                        ComplaintedDoctor.Add(new EmployeeInfo(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(),
                            reader[3].ToString(), reader[4].ToString(), reader[5].ToString()));
                    }
                    i++;
                }
                if (i == 0)
                {
                    ComplaintedDoctor.Add(0);
                }
                return ComplaintedDoctor;
            }
            catch (Exception e)
            {

            }
            return null;
        } 
        
        //duty类的id什么时候设置
        public static bool SetDuty(Duty item)
        {
            return false;
        }
    }
}