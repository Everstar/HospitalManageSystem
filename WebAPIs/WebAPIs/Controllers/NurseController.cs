using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIs.Models.DataModels;
using WebAPIs.Providers;

namespace WebAPIs.Controllers
{
    //[Authorize(Roles = "Nurse")]
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
            ArrayList list = new ArrayList();
            list.Add(new Hospitalization());
            list.Add(new Hospitalization());
            list.Add(new Hospitalization());
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StringContent(JsonObjectConverter.ObjectToJson(list));
            return response;
        }
    }
}
