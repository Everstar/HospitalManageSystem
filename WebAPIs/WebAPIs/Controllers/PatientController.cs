using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPIs.Models;
using WebAPIs.Models.DataModels;
using WebAPIs.Models.UnifiedTable;
using WebAPIs.Providers;

namespace WebAPIs.Controllers
{
    // 权限设置
    //[Authorize(Roles = "Patient")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class PatientController : BaseController
    {

        //done
        //获取所有科室名称
        public HttpResponseMessage GetAllClinic()
        {
            // 只需要获取名字就可以了
            // Clinic表获取所有名称

            ArrayList list = PatientHelper.GetAllClinic();

            HttpResponseMessage response = new HttpResponseMessage();

            if (list == null)
            {
                response.Content = new StringContent("查询失败");
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else if (list.Count == 0)
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

        //done
        //获取某科室的医生
        [Route("api/Patient/GetEmployee/{clinicName}")]
        public HttpResponseMessage GetEmployee(string clinicName)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            // 返回所有当前科室下所有医生的所有信息
            // employee表找到clinic符合的医生
            // 返回医生的所有信息
            ArrayList list = PatientHelper.GetEmployeeOfClinic(clinicName);

            if (list == null)
            {
                response.Content = new StringContent("查询失败,请检查科室名是否正确或服务器内部错误");
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else if (list.Count == 0)
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

        //done
        //获取某医生值班信息
        [Route("api/Patient/GetEmployeeDutyTime/{employeeId}")]
        public HttpResponseMessage GetEmployeeDutyTime(string employeeId)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            // 返回医生值班的时间
            // employee表找到duty_id
            // duty表找到所有数据

            Duty employeeDuty = PatientHelper.GetEmployeeDutyTime(employeeId);
            //duty不存在
            if (employeeDuty == null)
            {
                response.Content = new StringContent("医生Id错误，或医生没有值班信息");
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                response.Content = new StringContent(JsonObjectConverter.ObjectToJson(employeeDuty));
                response.StatusCode = HttpStatusCode.OK;
            }

            return response;
        }


        //update needed
        //挂号
        [HttpPost]
        [Route("api/Patient/Register/{employeeId}")]
        public HttpResponseMessage Register([FromBody]string time)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            string url = Request.RequestUri.AbsolutePath;
            string pattern = @"\d+";
            string employeeId = Regex.Match(url, pattern, RegexOptions.IgnoreCase).Value;

            string patientId = HttpContext.Current.User.Identity.Name;

            if (employeeId == null || employeeId.Equals(""))
            {
                // 这url不合法
                response.Content = new StringContent("Url不合法！");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            else
            {
                // Url合法 
                EmployeeInfo employeeInfo = UserHelper.GetEmployeeInfo(employeeId);

                //判断医生是否有空

                if (employeeInfo == null)
                {
                    response.Content = new StringContent("医生不存在");
                    response.StatusCode = HttpStatusCode.BadRequest;
                }

                // 创建挂号记录
                Treatment treatment = new Treatment();
                //设置预约挂号时间段

                treatment.patient_id = patientId;

                DateTime treatTime = Convert.ToDateTime(time);
                DateTime treatEndTime = treatTime .AddHours(1);

                treatment.start_time = treatTime;
                treatment.end_time = treatEndTime;

                //添加医生Id

                treatment.doc_id = employeeId;
                
                // 根据employeeId找到医生的科室
               
                treatment.clinic = employeeInfo.clinic;
                // 设置挂号金额
                Random ran = new Random();
                treatment.pay = 100 * ran.NextDouble();

                // treatment 表插入一条记录
                string treatment_id = PatientHelper.RegisterTreat(treatment);
                if (treatment == null)
                {
                    response.Content = new StringContent("挂号失败");
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
                else
                {
                    response.Content = new StringContent("挂号成功:" + "\n" + employeeId + time + "\n" +
                        "挂号单id:" + treatment_id);
                    response.StatusCode = HttpStatusCode.OK;
                }
                // 得到这条记录的主码

                // takes表插入患者id treatment id 医生id设为空, 等接诊成功时再填充doc_id
       
            }
            //response.Content = new StringContent(employeeId + " " + time);
            
            return response;
        }

        /// <summary>
        ///  //update needed
        /// </summary>
        /// <returns></returns>
       
        //获取患者自己的医疗记录
        [HttpGet]
        [Route("api/Patient/GetTreatmentID")]
        public HttpResponseMessage GetTreatmentID()
        {
            bool isLogin = GenerateUserInfoByCookie();

            string patient_id = HttpContext.Current.User.Identity.Name;

            // treatment表调取数据
            ArrayList list = PatientHelper.GetAllConsumption(patient_id);
            ArrayList returnList = new ArrayList();

            HttpResponseMessage response = new HttpResponseMessage();
            if (list == null)
            {
                response.Content = new StringContent("查询失败");
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else if (list.Count == 0)
            {
                response.Content = new StringContent("查询列表空");
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {

                for(int i = 0; i < list.Count; i++)
                {
                    Treatment treatment = (Treatment)list[i];

                    TreatPayInfo treatPayInfo = new TreatPayInfo();

                    treatPayInfo.docName = PatientHelper.GetDoctorNameById(treatment.doc_id);

                    treatPayInfo.pay = treatment.pay;
                    treatPayInfo.clinicName = treatment.clinic;
                    treatPayInfo.treatId = treatment.treat_id;
                    treatPayInfo.treatTime = treatment.start_time.ToString();
                    if (treatment.pay_time.Year == 1)
                    {
                        treatPayInfo.isPay = false;
                    }
                    else
                    {
                        treatPayInfo.isPay = true;
                    }

                    returnList.Add(treatPayInfo);
                }

                response.Content = new StringContent(JsonObjectConverter.ObjectToJson(returnList));
                response.StatusCode = HttpStatusCode.OK;
            }
            return response;
        }

        [HttpGet]
        [Route("api/Patient/GetAllCost/{treatmentId")]
        public HttpResponseMessage GetAllCost(string treatmentId)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            ArrayList list = PatientHelper.GetAllConsumption(treatmentId);
            ArrayList returnList = new ArrayList();
            if (list == null)
            {
                response.Content = new StringContent("查询失败");
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else if (list.Count == 0)
            {
                response.Content = new StringContent("查询列表空");
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                for(int i = 1; i < list.Count; i++)
                {
                    ArrayList costList = (ArrayList)list[i];
                    ArrayList payList = new ArrayList();
                    for(int j = 0; j < costList.Count; j++)
                    {
                        Examination examination = (Examination)payList[i];
                        DetailCostInfo detailCostInfo = new DetailCostInfo();
                        detailCostInfo.cost = examination.pay;
                        detailCostInfo.costId = examination.exam_id;
                        detailCostInfo.docName = PatientHelper.GetDoctorNameById(examination.employee_id);
                        detailCostInfo.startTime = examination.exam_time.ToString();
                        if (examination.pay_time.Year == 1)
                        {
                            detailCostInfo.isPay = false;
                        }
                        else
                        {
                            detailCostInfo.isPay = true;
                        }
                    }
                    returnList.Add(payList);
                }
                response.Content = new StringContent(JsonObjectConverter.ObjectToJson(returnList));
                response.StatusCode = HttpStatusCode.OK;
            }


            return response;
        }

        //done
        [HttpPost]
        [Route("api/Patient/Comment")]
        //评价医生
        public HttpResponseMessage Comment(dynamic obj)
        {
            
            string patient_id = HttpContext.Current.User.Identity.Name;

            HttpResponseMessage response = new HttpResponseMessage();

            // 向evaluation插入相关评价
            // 返回评价后的界面
            
            //反序列化的过程

            Evaluation evaluation = new Evaluation();
            try
            {
                evaluation = JsonConvert.DeserializeAnonymousType(JsonObjectConverter.ObjectToJson(obj), evaluation);
            }
            catch(Exception e)
            {
                response.Content = new StringContent("参数传递出错，反序列化失败！");
                response.StatusCode = HttpStatusCode.Forbidden;
                return response;
            }
            evaluation.patient_id = patient_id;

            //数据库更新相关评论
            if (!PatientHelper.Comment(evaluation))
            {
                response.Content = new StringContent("由于某些原因，评价失败~");
                response.StatusCode = HttpStatusCode.Forbidden;
            }
            else
            {
                response.Content = new StringContent("评价成功~");
                response.StatusCode = HttpStatusCode.OK;
            }
            
            
            return response;
        }

        /// <summary>
        /// has been replaced by GetAllCost
        /// </summary>
        /// <param name="treatment_id"></param>
        /// <returns></returns>
        /* 
        //update needed
        [HttpPost]
        [Route("api/Patient/GetAllConsumption")]
        //获取某次医疗的所有消费记录
        public HttpResponseMessage GetAllConsumption(dynamic obj)
        {
            string treatment_id = obj.treatment_id.Value;
            string patient_id = HttpContext.Current.User.Identity.Name;
            // prescription surgery hospitalization 
            // examine表中用treatment_id取得exam_id, 之后在examination表取得相关数据
            // 根据治疗ID返回所有相关的治疗记录
            ArrayList list = new ArrayList();
            list.Add(new Hospitalization());
            //list.Add(new Examination());
            list.Add(new Prescribe());
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StringContent(JsonObjectConverter.ObjectToJson(list));
            return response;
        }*/

        [HttpGet]
        [Route("api/Patient/GetDoctorIdName/{treatment_id}")]
        //done
        //根据医疗流水号找到相关医生的id和name,返回一个arraylist{doc_id,doc_name}
        public HttpResponseMessage GetDoctorIdName(string treatment_id)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            ArrayList doctorIdName = PatientHelper.GetDoctorIdName(treatment_id);

            if (doctorIdName == null)
            {
                response.Content = new StringContent("查询医生未找到");
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                response.Content = new StringContent(JsonObjectConverter.ObjectToJson(doctorIdName));
                response.StatusCode = HttpStatusCode.OK;
            }

            return response;
        }


    }
}
