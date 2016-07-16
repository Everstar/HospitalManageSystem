using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using WebAPIs.Models;
using WebAPIs.Providers;

namespace WebAPIs.Controllers
{
    public class PatientController : BaseController
    {
        public string accessRoles = "Patient";

        public HttpResponseMessage GetAllClinic()
        {
            GenerateUserInfoByCookie();
            HttpResponseMessage response = new HttpResponseMessage();
            // 如果用户权限正确
            if (HttpContext.Current.User.IsInRole(accessRoles))
            {
                // 数据库找数据
                return response;
            }
            else
            {
                response.Content = new StringContent("你没有权限，拒绝访问");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
        }
        [Route("api/Patient/GetEmployee/{clinicName}")]
        public HttpResponseMessage GetEmployee(string clinicName)
        {
            GenerateUserInfoByCookie();
            HttpResponseMessage response = new HttpResponseMessage();
            // 如果用户权限正确
            if (HttpContext.Current.User.IsInRole(accessRoles))
            {
                // 返回所有当前科室下所有医生的所有信息
                return response;
            }
            else
            {
                response.Content = new StringContent("你没有权限，拒绝访问");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
        }
        [Route("api/Patient/GetEmployeeDutyTime/{employeeId}")]
        public HttpResponseMessage GetEmployeeDutyTime(string employeeId)
        {
            GenerateUserInfoByCookie();
            // TODO:权限判断
            HttpResponseMessage response = new HttpResponseMessage();
            // 返回医生值班的时间
            response.Content = new StringContent(JsonObjectConverter.ObjectToJson(employeeId));
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
                return response;
            }
            else
            {
                // Url合法
                // 创建挂号记录
            }
            GenerateUserInfoByCookie();
            response.Content = new StringContent(employeeId);
            return response;
        }
        [HttpPost]
        [Route("api/Patient/GetTreatmentID")]
        public HttpResponseMessage GetTreatmentID(dynamic obj)
        {
            GenerateUserInfoByCookie();
            var patientName = HttpContext.Current.User.Identity.Name;
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }
    }
}
