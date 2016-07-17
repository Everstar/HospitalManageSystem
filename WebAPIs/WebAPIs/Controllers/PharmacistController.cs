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

namespace WebAPIs.Controllers
{
    [Authorize(Roles = "Pharmacist")]
    public class PharmacistController : BaseController
    {
        /// <summary>
        /// 查询配药单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Pharmacist/GetAllPrescription/{employeeId}")]
        public HttpResponseMessage GetAllPrescription()
        {
            string url = Request.RequestUri.AbsolutePath;
            string pattern = @"\d+";
            string employeeId = Regex.Match(url, pattern, RegexOptions.IgnoreCase).Value;

            // 返回所有处方employeeId药剂师对应的处方表
            // 处方ID对应的所有药品的名称 量
            // 开方时间等
            // prescription prescribe表联合
            HttpResponseMessage response = new HttpResponseMessage();
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
            ArrayList list = new ArrayList();
            list.Add(new Prescribe());
            list.Add(new Prescribe());
            list.Add(new Prescribe());
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StringContent(JsonObjectConverter.ObjectToJson(list));
            return response;
        }
    }
}
