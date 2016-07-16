using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPIs.Controllers
{
    public class ExaminerController : BaseController
    {
        public string accessRoles = "Examiner";
        /// <summary>
        /// 获取检查单
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Examiner/GetExamination/{docId}")]
        public HttpResponseMessage GetExamination(string docId)
        {
            // get ALL Examination FROM Specific doc_id
            // examination表查找doc_id匹配的数据
            // 序列化返回
            HttpResponseMessage response = new HttpResponseMessage();
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
            string exam_id = obj.exam_id.Value;
            string checkpoint = obj.checkpoint.Value;
            string from_picture = obj.from_picture.Value;
            var picture = obj.picture;

            // XRay表插入
            HttpResponseMessage response = new HttpResponseMessage();
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

            // 胃镜表插入
            HttpResponseMessage response = new HttpResponseMessage();
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
