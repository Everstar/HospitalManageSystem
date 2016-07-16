using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WebAPIs.Controllers
{
    public class DoctorController : BaseController
    {
        public string accessRoles = "Doctor";
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
        /// <summary>
        /// 开检查单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Doctor/WriteExamination")]
        public HttpResponseMessage WriteExamination(dynamic obj)
        {
            string treatment_id = obj.treatment_id.Value;
            string checkType = obj.checkType.Value;
            string doc_id = HttpContext.Current.User.Identity.Name;
            // 开了检查单子 没交费 没做检查
            // examination表设置exam_Id, type, doc_id
            // 其他留空
            // exam表插入数据
            // 把exam_id和treatment_id关联起来
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }
        /// <summary>
        /// 获取ALL medicine的名字
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Doctor/GetAllMedicine")]
        public HttpResponseMessage GetAllMedicine()
        {
            // 从数据库获取所有的药品的名称
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }
        /// <summary>
        /// 获取药品ID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Doctor/GetMedicine/{name}")]
        public HttpResponseMessage GetMedicine(string name)
        {
            // 从数据库拿到相关药品的id
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }
        /// <summary>
        /// 开处方
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Doctor/WritePrescription")]
        public HttpResponseMessage WritePrescription(dynamic obj)
        {
            string treatment_id = obj.treatment_id.Value;
            string medicine_id = obj.medicine_id.Value;
            string number = obj.number;
            string doc_id = HttpContext.Current.User.Identity.Name;

            // prescription表创建一条记录
            // 设置pres_id treat_id doc_id从药剂师中随机挑选 time
            // 其他空值
            // prescribeb表创建一条记录
            // 设置pres_id medicine_id num
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }
        /// <summary>
        /// 开手术单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Doctor/WriteSurgey")]
        public HttpResponseMessage WriteSurgey(dynamic obj)
        {
            string treatment_id = obj.treatment_id.Value;
            string surgey_name = obj.surgey_name.Value;
            // surgery表插入数据
            // 设置surg_id treat_id surgey_name start_time end_time pay
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }
        /// <summary>
        /// 开住院单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Doctor/WriteHospitalization")]
        public HttpResponseMessage WriteHospitalization(dynamic obj)
        {
            string treatment_id = obj.treatment_id.Value;
            // 一个住院病人只有一个护士 一个护士可以有多个住院病人
            // hospitalization表增加
            // 设置hos_id treat_id nurse_id随机生成，在employee表查找 bed_num需要唯一 in_time系统获取
            // out_time空 pay随机生成
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }
    }
}
