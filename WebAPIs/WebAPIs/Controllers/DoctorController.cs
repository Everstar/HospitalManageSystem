using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPIs.Controllers
{
    public class DoctorController : BaseController
    {
        /// <summary>
        /// 获取挂号到该医生的挂号单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Doctor/GetAllTreatment")]
        public HttpResponseMessage GetAllTreatment()
        {
            // 验证权限和登陆
            // 没登录就要转跳到相关页面

            // 在数据库treatment takes表中找当前登陆医生
            // 的与之相关的挂号单treatment_id
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }
        /// <summary>
        /// 接诊
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Doctor/Takes")]
        public HttpResponseMessage Takes(dynamic obj)
        {
            // 验证权限和登陆
            // 没登录就要转跳到相关页面

            // 根据相关接诊转到接诊成功的结果界面
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }
        [HttpPost]
        [Route("api/Doctor/WriteExamination")]
        public HttpResponseMessage WriteExamination(dynamic obj)
        {

        }
    }
}
