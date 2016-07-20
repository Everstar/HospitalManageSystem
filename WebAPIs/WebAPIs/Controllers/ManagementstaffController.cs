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

namespace WebAPIs.Controllers
{
    //[Authorize(Roles = "Managementstaff")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]

    public class ManagementstaffController : BaseController
    {
        /// <summary>
        /// 查询人事信息
        /// </summary>
        /// <permission cref="Managementstaff"></permission>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ManagementStaff/GetAllEmployee")]
        public HttpResponseMessage GetAllEmployee()
        {
            // 数据库中找到所有employee的信息
            // 序列化成Json
            ArrayList list=ManagementHelper.GetAllEmployee();

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
        [HttpPost]
        [Route("api/ManagementStaff/SetEmployee")]
        public HttpResponseMessage SetEmployee(dynamic obj)
        {
            string department = obj.department.Value;
            string clinic = obj.clinic.Value;
            string post = obj.post.Value;
            string salary = obj.salary.Value;
            HttpResponseMessage response = new HttpResponseMessage();
            // 增加人员 删除人员 
            return response;
        }
        /// <summary>
        /// 查询投诉率大于n%的医生
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ManagementStaff/GetComplaintedDoctor/{percent}")]
        public HttpResponseMessage GetComplaintedDoctor(double percent)
        {
            // 被投诉是指某个医生的Rank小于等于两颗星星
            // 在evaluation表找到每一个doc_id下的数据总数 找到doc_id对应的rank小于等于2的数量
            // 如果这个比例大于percent
            // 记录下这个医生的doc_id
            // 返回一个doc_id的列表
            HttpResponseMessage response = new HttpResponseMessage();

            ArrayList list = ManagementHelper.GetComplaintedDoctor(percent);

            if (list == null)
            {
                response.Content = new StringContent("查询失败");
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                response.Content = new StringContent(JsonObjectConverter.ObjectToJson(list));
                response.StatusCode = HttpStatusCode.OK;
            }
            return response;
        }
        [HttpPost]
        [Route("api/ManagementStaff/SetDutyTime")]
        public HttpResponseMessage SetDutyTime(dynamic obj)
        {
            string room_num = obj.room_num.Value;
            string Monday = obj.Monday.Value;
            string Tuesday = obj.Tuesday.Value;
            string wednesday = obj.wednesday.Value;
            string Thursday = obj.Thursday.Value;
            string Friday = obj.Friday.Value;
            // duty表生成id
            // 设置room_num
            // max_limit设置成随机数
            // 日期随便设置了 上下午。。。
            HttpResponseMessage response = new HttpResponseMessage();
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
            var employee = JsonConvert.DeserializeObject(JsonObjectConverter.ObjectToJson(obj)) as JObject;
            Employee emp = new Employee();
            emp.clinic = employee.GetValue("clinic_name").ToString();
            emp.credit_num = employee.GetValue("credit_num").ToString();
            emp.password = employee.GetValue("password").ToString();
            emp.post = employee.GetValue("post").ToString();
            emp.salary = double.Parse(employee.GetValue("salary").ToString());
            // 数据库中插入
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }
    }
}
