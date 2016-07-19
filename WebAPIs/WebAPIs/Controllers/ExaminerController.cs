﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIs.Providers;
using WebAPIs.Models.DataModels;
using System.Web.Http.Cors;
using WebAPIs.Models;
using System.Collections;
using Newtonsoft.Json;

namespace WebAPIs.Controllers
{
    //[Authorize(Roles = "Examiner")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]

    public class ExaminerController : BaseController
    {
        /// <summary>
        /// 获取检查单
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Examiner/GetExamination/{docId}")]
        public HttpResponseMessage GetExamination(string docId)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            // get ALL Examination FROM Specific doc_id
            // examination表查找doc_id匹配的数据
            // 序列化返回
            ArrayList list=ExaminerHelper.GetAllExamination(docId);
            if (list.Count == 0)
            {
                response.Content = new StringContent("未找到相关检测记录");
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                response.Content = new StringContent(JsonObjectConverter.ObjectToJson(list));
                response.StatusCode = HttpStatusCode.OK;
            }

            
            //response.Content = new StringContent(JsonObjectConverter.ObjectToJson(new Examination()));
            return response;
        }
        /// <summary>
        /// X光检查
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Examiner/MakeXRayExamination")]
        public HttpResponseMessage MakeXRayExamination(dynamic obj)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            string exam_id = obj.exam_id.Value;
            string checkpoint = obj.checkpoint.Value;
            string from_picture = obj.from_picture.Value;
            var picture = obj.picture;
            //一个对象反序列化的过程
            PartXRayInfo xrayInfo = new PartXRayInfo();

            xrayInfo = JsonConvert.DeserializeAnonymousType(JsonObjectConverter.ObjectToJson(obj), xrayInfo);

            // XRay表插入

            if (!ExaminerHelper.MakeXRayExamination(xrayInfo))
            {
                response.Content = new StringContent("由于某种原因检测插入不成功");
                response.StatusCode = HttpStatusCode.Forbidden;
            }
            else
            {
                response.Content = new StringContent("检测结果插入表中");
                response.StatusCode = HttpStatusCode.OK;
            }

            
            return response;
        }
        /// <summary>
        /// 胃镜检查
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Examiner/MakeGastroscopeExamination")]
        public HttpResponseMessage MakeGastroscopeExamination(dynamic obj)
        {
            string exam_id = obj.exam_id.Value;
            string diagnoses = obj.diagnoses.Value;
            string from_picture = obj.from_picture.Value;
            var picture = obj.picture;

            HttpResponseMessage response = new HttpResponseMessage();

            if(!ExaminerHelper.MakeGastroscopeExamination(from_picture, diagnoses, picture))
            {
                response.Content = new StringContent("由于某种原因插入检查结果不成功");
                response.StatusCode = HttpStatusCode.Forbidden;
            }
            else
            {
                response.Content = new StringContent("检测结果插入成功");
                response.StatusCode = HttpStatusCode.OK;
            }


            // 胃镜表插入
            
            return response;
        }
        /// <summary>
        /// 验血检查
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Examiner/MakeBloodExamination")]
        public HttpResponseMessage MakeBloodExamination(dynamic obj)
        {
            string exam_id = obj.exam_id.Value;
            string wbc = obj.wbc.Value;
            string neut_percent = obj.neut_percent.Value;
            string lymph_percent = obj.lymph_percent.Value;
            string mono_percent = obj.mono_percent.Value;
            string eo_percent = obj.eo_percent.Value;
            string baso_percent = obj.baso_percent.Value;
            string neut_num = obj.neut_num.Value;
            string lymph_num = obj.lymph_num.Value;
            string mono_num = obj.mono_num.Value;
            string eo_num = obj.eo_num.Value;
            string baso_num = obj.baso_num.Value;
            string rbc = obj.rbc.Value;
            string hgb = obj.hgb.Value;
            string hct = obj.hct.Value;
            string mcv = obj.mcv.Value;
            string mch = obj.mch.Value;
            string mchc = obj.mchc.Value;
            string rdw_cv = obj.rdw_cv.Value;
            string rdw_sd = obj.rdw_sd.Value;
            string plt = obj.plt.Value;
            string mpv = obj.mpv.Value;
            string pdw = obj.pdw.Value;
            string pct = obj.pct.Value;

           


            // Blood表插入
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }
    }
}
