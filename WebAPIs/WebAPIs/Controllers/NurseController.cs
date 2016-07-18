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

namespace WebAPIs.Controllers
{
    //[Authorize(Roles = "Nurse")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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

        //retrun{hos_id,treat_id,nurse_id,bed_num,pay,rank,in_time,out_time,pay_time}
        public HttpResponseMessage GetHospitalization(string nurseId)
        {

            HttpResponseMessage response = new HttpResponseMessage();
            // Retuen ALL Hospitalization natural join Bed
            // hospitalization表查找nurse_id相关的记录 找到床 返回

            ArrayList list = NurseHelper.GetHospitalizationInfo(nurseId);

            if (list.Count == 0)
            {
                response.Content = new StringContent("未找到相关信息");
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                response.Content = new StringContent(JsonObjectConverter.ObjectToJson(list));
                response.StatusCode = HttpStatusCode.OK;
            }
           
            return response;
        }
    }
}
