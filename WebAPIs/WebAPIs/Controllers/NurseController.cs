using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPIs.Controllers
{
    public class NurseController : BaseController
    {
        public string accessRoles = "Nurse";

        /// <summary>
        /// 获取住院信息
        /// </summary>
        /// <param name="nurseId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Nurse/GetHospitalization/{nurseId}")]
        public HttpResponseMessage GetHospitalization(string nurseId)
        {
            // Retuen ALL Hospitalization natural join Bed
            // hospitalization表查找nurse_id相关的记录 找到床 返回
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }
    }
}
