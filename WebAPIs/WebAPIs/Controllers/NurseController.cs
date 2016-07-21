using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPIs.Models;
using WebAPIs.Models.DataModels;
using WebAPIs.Providers;
using Newtonsoft.Json;
using WebAPIs.Models.UnifiedTable;
using Newtonsoft.Json.Linq;

namespace WebAPIs.Controllers
{

    /// <summary>
    /// 
    /// is finished
    /// </summary>
    [Authorize(Roles = "Nurse")]
    public class NurseController : BaseController
    {
        /// <summary>
        /// Test Passed
        /// 获取住院信息
        /// </summary>
        /// <param name="nurseId"></param>
        /// <returns>{hos_id,treat_id,nurse_id,bed_num,pay,rank,in_time,out_time,pay_time}</returns>
        /// done
        /// Version 7.20 11:14 根据新的API文档去掉了参数的nurseid 改为由Cookie获取
        [HttpGet]
        [Route("api/Nurse/GetHospitalization")]
        public HttpResponseMessage GetHospitalization()
        {
            string nurseId = HttpContext.Current.User.Identity.Name;
            if (nurseId.Equals(""))
                nurseId = "14001";
            HttpResponseMessage response = new HttpResponseMessage();
            // Retuen ALL Hospitalization natural join Bed
            // hospitalization表查找nurse_id相关的记录 找到床 返回
            ArrayList list = new ArrayList();
            JArray resArray = new JArray();
            try
            {
                list = NurseHelper.GetHospitalizationInfo(nurseId);
                if (list.Count == 0)
                {
                    response.Content = new StringContent("未找到相关信息");
                    response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    foreach (HospitalInfo item in list)
                    {
                        JObject obj = new JObject();
                        obj.Add("hosp_id", item.hos_id);
                        string patient_id = UserHelper.GetPatientIdByTreatmentId(item.treat_id);
                        PatientInfo user_info = UserHelper.GetPatientInfo(patient_id);
                        obj.Add("patient_name", user_info.name);
                        obj.Add("bed_num", item.bed_num);
                        obj.Add("nursing_rank", item.rank);
                        obj.Add("in_hosp_time", item.in_time);
                        resArray.Add(obj);
                    }
       
                    response.Content = new StringContent(JsonObjectConverter.ObjectToJson(resArray));
                    response.StatusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception e)
            {
                response.Content = new StringContent(e.Message);
            }

           
            return response;
        }

        [HttpGet]
        [Route("api/Nurse/OutHospital/{hospitalId}")]
        public HttpResponseMessage OutHospital(string hospitalId)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            if (NurseHelper.OutHospital(hospitalId))
            {
                response.Content = new StringContent("出院成功");
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.Content = new StringContent("更新失败");
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }

    }
}
