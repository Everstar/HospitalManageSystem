using System;
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
using System.Threading.Tasks;
using System.IO;
using WebAPIs.Models.UnifiedTable;

namespace WebAPIs.Controllers
{
    //[Authorize(Roles = "Examiner")]
    public class ExaminerController : BaseController
    {
        /// <summary>
        /// Test Passed
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
            ArrayList list = ExaminerHelper.GetAllExamination(docId);
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
        /// Test Passed
        /// X光检查
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //done
        [HttpPost]
        [Route("api/Examiner/MakeXRayExamination")]
        public async Task<HttpResponseMessage> MakeXRayExamination()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            // 存储文件到数据库
            //一个对象反序列化的过程
            XrayInfo xrayInfo = (XrayInfo)await ReadAndSaveFile();
            string jsonStr = "";
            try
            {
                jsonStr = JsonObjectConverter.ObjectToJson(xrayInfo);
                response.Content = new StringContent(jsonStr);
            }
            catch (Exception e)
            {
                response.Content = new StringContent("post数据格式错误\nReceives:\n" + JsonObjectConverter.ObjectToJson(xrayInfo));
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
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

        private async Task<Object> ReadAndSaveFile()
        {
            string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads");

            var provider = new MultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);

            FileInfo finfo = new FileInfo(provider.FileData.First().LocalFileName);

            string guid = Guid.NewGuid().ToString();
            string path = Path.Combine(root, guid + "_" + provider.FileData.First().Headers.ContentDisposition.FileName.Replace("\"", ""));
            File.Move(finfo.FullName, path);
            var formData = provider.FormData;
            int type = 10;
            try
            {
                type = int.Parse(formData.GetValues("type").FirstOrDefault());
            }
            catch (Exception e)
            {
                throw e;
            }

            // 新建一个对象
            Object info = new object();
            string exam_id;
            string checkpoint;
            string from_picture;
            string picture;
            string diagnoses;

            switch (type)
            {
                case 0:
                    break;
                case 1:
                    exam_id = formData.GetValues("exam_id").FirstOrDefault();
                    diagnoses = formData.GetValues("diagnoses").FirstOrDefault();
                    from_picture = formData.GetValues("from_picture").FirstOrDefault();
                    info = new GastroscopeInfo(from_picture, diagnoses, path);
                    break;
                case 2:
                    exam_id = formData.GetValues("exam_id").FirstOrDefault();
                    checkpoint = formData.GetValues("checkpoint").FirstOrDefault();
                    from_picture = formData.GetValues("from_picture").FirstOrDefault();
                    info = new XrayInfo(exam_id, checkpoint, from_picture, path);
                    break;
                default:
                    return null;
            }

            return info;
        }
        /// <summary>
        /// 胃镜检查
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Examiner/MakeGastroscopeExamination")]
        public async Task<HttpResponseMessage> MakeGastroscopeExamination()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            GastroscopeInfo gastroInfo;
            try
            {
                gastroInfo = (GastroscopeInfo)await ReadAndSaveFile();
            }
            catch (Exception e)
            {
                response.Content = new StringContent(e.Message);
                return response;
            }
            try
            {
                if (!ExaminerHelper.MakeGastroscopeExamination(gastroInfo))
                {
                    response.Content = new StringContent("由于某种原因插入检查结果不成功");
                    response.StatusCode = HttpStatusCode.Forbidden;
                }
                else
                {
                    response.Content = new StringContent("检测结果插入成功" +JsonObjectConverter.ObjectToJson(gastroInfo));
                    response.StatusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception e)
            {
                response.Content = new StringContent(e.Message);
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }
        /// <summary>
        /// 验血检查
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //update needed
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

            HttpResponseMessage response = new HttpResponseMessage();

            Blood blood = new Blood();
            try
            {
                blood = JsonConvert.DeserializeAnonymousType(JsonObjectConverter.ObjectToJson(obj), blood);
                ExaminerHelper.MakeBloodExamination(blood, "fuck");
            }
            catch(Exception e)
            {
                response.Content = new StringContent(e.Message);
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            // Blood表插入
            return response;
        }



        //dnoe
        //return {name,sex}
        [HttpGet]
        [Route("api/Examiner/GetPatientById/{examInfo}")]
        public HttpResponseMessage GetPatientNameById(string examInfo)
        {

            ArrayList list = ExaminerHelper.GetPatientByExamId(examInfo);

            HttpResponseMessage response = new HttpResponseMessage();

            if (list == null)
            {
                response.Content = new StringContent("查询失败");
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else if (list.Count == 0)
            {
                response.Content = new StringContent("查询失败");
                response.StatusCode = HttpStatusCode.BadRequest;
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
