using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPIs.Models;
using WebAPIs.Models.DataModels;
using WebAPIs.Providers;

namespace WebAPIs.Controllers
{
    // 权限设置
    [Authorize(Roles = "Patient")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class PatientController : BaseController
    {
        public HttpResponseMessage GetAllClinic()
        {
            // 只需要获取名字就可以了
            // Clinic表获取所有名称
            HttpResponseMessage response = new HttpResponseMessage();
            ArrayList list = new ArrayList();
            list.Add(new Clinic());
            list.Add(new Clinic());
            list.Add(new Clinic());
            list.Add(new Clinic());
            list.Add(new Clinic());
            list.Add(new Clinic());
            response.Content = new StringContent(JsonObjectConverter.ObjectToJson(list));
            return response;
        }
        [Route("api/Patient/GetEmployee/{clinicName}")]
        public HttpResponseMessage GetEmployee(string clinicName)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            // 返回所有当前科室下所有医生的所有信息
            // employee表找到clinic符合的医生
            // 返回医生的所有信息
            ArrayList list = new ArrayList();
            list.Add(new Employee());
            list.Add(new Employee());
            response.Content = new StringContent(JsonObjectConverter.ObjectToJson(list));

            return response;
        }
        [Route("api/Patient/GetEmployeeDutyTime/{employeeId}")]
        public HttpResponseMessage GetEmployeeDutyTime(string employeeId)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            // 返回医生值班的时间
            // employee表找到duty_id
            // duty表找到所有数据

            //response.Content = new StringContent(JsonObjectConverter.ObjectToJson(new Duty()));
            return response;
        }
        [HttpPost]
        [Route("api/Patient/Register/{employeeId}")]
        public HttpResponseMessage Register([FromBody]string time)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            string url = Request.RequestUri.AbsolutePath;
            string pattern = @"\d+";
            string employeeId = Regex.Match(url, pattern, RegexOptions.IgnoreCase).Value;

            if (employeeId == null || employeeId.Equals(""))
            {
                // 这url不合法
                response.Content = new StringContent("Url不合法！");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            else
            {
                // Url合法
                // 创建挂号记录
                // 根据employeeId找到医生的科室
                // 设置挂号金额
                // 填充支付时间
                // treatment 表插入一条记录
                // 得到这条记录的主码
                // takes表插入患者id treatment id 医生id设为空, 等接诊成功时再填充doc_id
            }
            response.Content = new StringContent(employeeId + " " + time);
            return response;
        }
        [HttpPost]
        [Route("api/Patient/GetTreatmentID")]
        public HttpResponseMessage GetTreatmentID(dynamic obj)
        {
            bool isLogin = GenerateUserInfoByCookie();

            string patient_id = HttpContext.Current.User.Identity.Name;
            string month = obj.month.Value;
            string year = obj.year.Value;
            // treatment表调取数据
            ArrayList list = new ArrayList();
            list.Add(new Treatment());
            list.Add(new Treatment());
            list.Add(new Treatment());
            list.Add(new Treatment());

            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StringContent(JsonObjectConverter.ObjectToJson(list));
            return response;
        }
        [HttpPost]
        [Route("api/Patient/Comment")]
        public HttpResponseMessage Comment(dynamic obj)
        {
            string employee_id = obj.employee_id.Value;
            string treatment_id = obj.treatment_id.Value;
            string rank = obj.rank.Value;
            string text = obj.text.Value;
            string patient_id = HttpContext.Current.User.Identity.Name;

            // 向evaluation插入相关评价
            // 返回评价后的界面
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StringContent("评价成功~");
            return response;
        }
        [HttpPost]
        [Route("api/Patient/GetAllConsumption")]
        public HttpResponseMessage GetAllConsumption(dynamic obj)
        {
            string treatment_id = obj.treatment_id.Value;
            string patient_id = HttpContext.Current.User.Identity.Name;
            // prescription surgery hospitalization 
            // examine表中用treatment_id取得exam_id, 之后在examination表取得相关数据
            // 根据治疗ID返回所有相关的治疗记录
            ArrayList list = new ArrayList();
            list.Add(new Hospitalization());
            list.Add(new Examination());
            list.Add(new Prescribe());
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StringContent(JsonObjectConverter.ObjectToJson(list));
            return response;
        }

    }
}
