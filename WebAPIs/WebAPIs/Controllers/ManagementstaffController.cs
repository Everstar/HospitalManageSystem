using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPIs.Models;
using WebAPIs.Models.DataModels;
using WebAPIs.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAPIs.Models.UnifiedTable;

namespace WebAPIs.Controllers
{
    [Authorize(Roles = "Managementstaff")]
    public class ManagementstaffController : ApiController
    {
        /// <summary>
        /// 查询人事信息
        /// </summary>
        /// <permission cref="Managementstaff"></permission>
        /// <returns></returns>
        /// Test Passed
        /// 
        [HttpGet]
        [Route("api/ManagementStaff/GetAllEmployee")]
        public HttpResponseMessage GetAllEmployee()
        {
            // 数据库中找到所有employee的信息
            // 如果工资为-1
            // 则不显示
            // 序列化成Json
            ArrayList list = ManagementHelper.GetAllEmployee();

            HttpResponseMessage response = new HttpResponseMessage();
            if (list.Count == 0)
            {
                response.Content = new StringContent("查找失败");
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                response.Content = new StringContent(JsonObjectConverter.ObjectToJson(list));

                response.StatusCode = HttpStatusCode.OK;
            }

            return response;
        }
        /// <summary>
        /// 人事调配
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/ManagementStaff/SetEmployee")]
        public HttpResponseMessage SetEmployee(dynamic obj)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            var employee = JsonConvert.DeserializeObject(JsonObjectConverter.ObjectToJson(obj)) as JObject;
            try
            {
                string department = employee.GetValue("dept_name").ToString();
                string clinic = employee.GetValue("clinic_name").ToString();
                string post = employee.GetValue("post").ToString();
                string salary = obj.salary.Value;
                string id = employee.GetValue("id").ToString();
                ManagementHelper.SetEmployee(id, department, clinic, post, double.Parse(salary));
            }
            catch (Exception e)
            {
                response.Content = new StringContent(e.Message);
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            // 增加人员 删除人员 
            return response;
        }
        /// <summary>
        /// 查询投诉率大于n%的医生
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="percent">百分比000-100</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ManagementStaff/GetComplaintedDoctor/{year}/{month}/{percent}")]
        public HttpResponseMessage GetComplaintedDoctor(int year, int month, string percent)
        {
            // 被投诉是指某个医生的Rank小于等于两颗星星
            // 在evaluation表找到每一个doc_id下的数据总数 找到doc_id对应的rank小于等于2的数量
            // 如果这个比例大于percent
            // 记录下这个医生的doc_id
            // 返回一个doc_id的列表
            HttpResponseMessage response = new HttpResponseMessage();
            JArray strArray = new JArray();
            ArrayList list = null;
            try
            {
                list = ManagementHelper.GetComplaintedDoctor(year, month, double.Parse(percent));
            }
            catch (Exception e)
            {
                response.Content = new StringContent(e.Message);
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            if (list == null)
            {
                response.Content = new StringContent("查询过程出现异常");
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                if (list.Count == 0)
                {
                    strArray.Add("0");
                    response.Content = new StringContent(JsonObjectConverter.ObjectToJson(strArray));
                }
                else
                {
                    strArray.Add("1");
                    for (int i = 1; i < list.Count; i++)
                    {
                        EmployeeInfo emp = (EmployeeInfo)list[i];
                        JObject obj = new JObject();
                        obj.Add("department", emp.department);
                        obj.Add("clinic", emp.clinic);
                        obj.Add("id", emp.employee_id);
                        obj.Add("name", emp.name);
                        obj.Add("compliant_rate", emp.compliant_rate.ToString());
                        strArray.Add(obj);
                    }
                }
                response.Content = new StringContent(JsonObjectConverter.ObjectToJson(strArray));
                response.StatusCode = HttpStatusCode.OK;
            }
            return response;
        }
        /// <summary>
        /// 设置员工工作的时间
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ManagementStaff/SetDutyTime")]
        public HttpResponseMessage SetDutyTime(dynamic obj)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            var employee = JsonConvert.DeserializeObject(JsonObjectConverter.ObjectToJson(obj)) as JObject;
            string emp_id = employee.GetValue("employee_id").ToString();
            string room_num = employee.GetValue("clinic_num").ToString();
            string Monday = employee.GetValue("Monday").ToString();
            string Tuesday = employee.GetValue("Tuesday").ToString();
            string Wednesday = employee.GetValue("Wednesday").ToString();
            string Thursday = employee.GetValue("Thursday").ToString();
            string Friday = employee.GetValue("Friday").ToString();
            string Saturday = employee.GetValue("Saturday").ToString();
            string Sunday = employee.GetValue("Sunday").ToString();
            Duty item = new Duty(room_num, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday);
            // 插入数据
            try
            {
                ManagementHelper.SetDuty(item, emp_id);
            }
            catch (Exception e)
            {
                response.Content = new StringContent(e.Message);
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            // duty表生成id
            // 设置room_num
            // max_limit设置成随机数
            // 日期随便设置了 上下午。。。
            return response;
        }
        /// <summary>
        /// 增加雇员
        /// </summary>
        /// <example>
        /// 调用格式
        /// <code>
        /// {credit_num: '2222', password: '344123', dept_name: '门诊部', clinic_name： '妇产科', post: 'Nurse', salary: '100000000'}
        /// </code>
        /// </example>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ManagementStaff/AddEmployee")]
        public HttpResponseMessage AddEmployee(dynamic obj)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            Employee emp = new Employee();
            Identity identity = new Identity();


            try
            {
                var employee = JsonConvert.DeserializeObject(JsonObjectConverter.ObjectToJson(obj)) as JObject;
                emp.clinic = employee.GetValue("clinic_name").ToString();
                emp.credit_num = employee.GetValue("credit_num").ToString();
                emp.password = employee.GetValue("password").ToString();
                emp.post = employee.GetValue("post").ToString();
                emp.salary = double.Parse(employee.GetValue("salary").ToString());
                emp.department = employee.GetValue("dept_name").ToString();

                identity.credit_num = emp.credit_num;
                identity.name = employee.GetValue("name").ToString();
                identity.sex = char.Parse(employee.GetValue("sex").ToString());
                identity.birth = Convert.ToDateTime(employee.GetValue("birth").ToString());
            }
            catch (Exception e)
            {
                response.Content = new StringContent("数据格式错误:"+e.Message);
                return response;
            }

            try
            {
                ManagementHelper.AddEmployee(identity, emp);
            }
            catch (Exception e)
            {
                response.Content = new StringContent(e.Message);
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            response.Content = new StringContent("添加成功！" + JsonObjectConverter.ObjectToJson(emp));
            // 数据库中插入
            return response;
        }
        /// <summary>
        /// 删除雇员信息 通过设置工资为-1标注
        /// </summary>
        /// <param name="employee_id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ManagementStaff/DeleteEmployee/{employee_id}")]
        public HttpResponseMessage DeleteEmployee(string employee_id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                ManagementHelper.DeleteEmployee(employee_id);
            }
            catch (Exception e)
            {
                response.Content = new StringContent(e.Message);
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            response.Content = new StringContent("更新成功！");
            return response;
        }
    }
}
