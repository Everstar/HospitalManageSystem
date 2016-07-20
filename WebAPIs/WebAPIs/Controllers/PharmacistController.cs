using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using WebAPIs.Providers;
using WebAPIs.Models.DataModels;
using System.Web.Http.Cors;
using WebAPIs.Models;
using System.Web;

namespace WebAPIs.Controllers
{
    //[Authorize(Roles = "Pharmacist")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class PharmacistController : BaseController
    {
        /// <summary>
        /// 查询配药单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllPrescription()
        {
            string employeeId = HttpContext.Current.User.Identity.Name;
            // 返回所有处方employeeId药剂师对应的处方表
            // 处方ID对应的所有药品的名称 量
            // 开方时间等
            // prescription prescribe表联合
            ArrayList list = PrescriptionHelper.GetAllPrescription(employeeId);

            HttpResponseMessage response = new HttpResponseMessage();

            if (list == null)
            {
                response.Content = new StringContent("查找失败");
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else if(list.Count==0)
            {
                response.Content = new StringContent("查询列表空");
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.Content = new StringContent(JsonObjectConverter.ObjectToJson(list));
                response.StatusCode = HttpStatusCode.OK;
            }
            return response;
        }
        /// <summary>
        /// 配药
        /// </summary>
        /// <param name="pres_id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Pharmacist/Prescribe/{pres_id}")]
        public HttpResponseMessage Prescribe(string pres_id)
        {
            // 某个药配好了
            // prescription表中标记这个药物单子配好的时间
            // prescription表加一条配处方的时间

            HttpResponseMessage response = new HttpResponseMessage();

            ArrayList list = PrescriptionHelper.Prescribe(pres_id);

            if (list == null)
            {
                response.Content = new StringContent("查询失败");
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else if(list.Count==0)
            {
                response.Content = new StringContent("查询列表空");
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.Content = new StringContent(JsonObjectConverter.ObjectToJson(list));
                response.StatusCode = HttpStatusCode.OK;
            }
            return response;
        }



        [HttpGet]
        [Route("api/Pharmacist/FinishPrescription/{pres_id}")]
        public HttpResponseMessage FinishPrescription(string pres_id)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            if (PrescriptionHelper.UpdateDoneTime(pres_id))
            {
                response.Content = new StringContent("完成配药");
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.Content = new StringContent("数据库更新失败");
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            //为了commit
            return response;
        }
    }
}
