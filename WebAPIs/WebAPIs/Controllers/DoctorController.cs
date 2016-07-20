using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPIs.Providers;
using WebAPIs.Models;
using WebAPIs.Models.DataModels;
using System.Web.Http.Cors;
using WebAPIs.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAPIs.Models.UnifiedTable;

namespace WebAPIs.Controllers
{
    //[Authorize(Roles = "Doctor")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class DoctorController : BaseController
    {
        /// <summary>
        /// 获取挂号到该医生的挂号单
        /// </summary>
        /// <example>
        /// <code>        
        ///status(找到记录为 1/无记录为 0),
        ///{treatment_id, patient_name, clinic, employee_name, start_time(预约时间),take
        ///    }
        ///{treatment_id, patient_name, clinic, employee_name, start_time(预约时间),take
        ///}
        ///<---若干条记录--->
        ///]
        /// </code>
        /// </example>
        /// <returns>所有接诊挂号单(需要cookie获取employee_id)</returns>
        /// Pending to test
        [HttpGet]
        public HttpResponseMessage GetAllTreatment()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            string doc_id = HttpContext.Current.User.Identity.Name;
            if (doc_id.Equals(""))
                doc_id = "16687";
            // 在数据库treatment表找到所有与当前医生doc_id相关的所有挂号记录
            // treatment natural join takes using(treat_id)
            // tekes.doc_id若为空 取出这条数据组成数组
            // 设置takes表treat_id对应记录的doc_id为当前doc_id
            try
            {
                ArrayList list = DoctorHelper.GetAllTreatment(doc_id);
                JArray resArray = new JArray();
                if (list.Count == 0)
                {
                    resArray.Add(0);
                }
                else
                {
                    resArray.Add("1");
                    string jsonRes = JsonObjectConverter.ObjectToJson(list);
                    var strObject = JsonConvert.DeserializeObject(jsonRes) as JArray;

                    for (int i = 0; i < strObject.Count; i++)
                    {
                        JObject tmpObj = JObject.Parse(strObject[i].ToString());
                        JObject obj = new JObject();
                        string patient_id = tmpObj.GetValue("patient_id").ToString();
                        string employee_id = tmpObj.GetValue("doc_id").ToString();
                        PatientInfo patientInfo = UserHelper.GetPatientInfo(patient_id);
                        EmployeeInfo employeeInfo = UserHelper.GetEmployeeInfo(employee_id);
                        string treatment_id = tmpObj.GetValue("treat_id").ToString();
                        string patient_name = patientInfo.name.ToString();
                        string clinic = tmpObj.GetValue("clinic").ToString();
                        string employee_name = employeeInfo.name.ToString();
                        string start_time = tmpObj.GetValue("start_time").ToString();
                        string take = tmpObj.GetValue("take").ToString();
                        obj.Add("treatment_id", treatment_id);
                        obj.Add("patient_name", patient_name);
                        obj.Add("clinic", clinic);
                        obj.Add("employee_name", employee_name);
                        obj.Add("start_time", start_time);
                        obj.Add("take", take);
                        resArray.Add(obj);
                    }
                }
                response.Content = new StringContent(JsonObjectConverter.ObjectToJson(resArray));

            }
            catch (Exception e)
            {
                response.Content = new StringContent(e.Message);
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }
        /// <summary>
        /// 接诊
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// Test Passed
        [HttpGet]
        [Route("api/Doctor/TakesRegistration/{treatment_id}")]
        public HttpResponseMessage TakesRegistration(string treatment_id)
        {
            string doc_id = HttpContext.Current.User.Identity.Name;
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                DoctorHelper.TakeTreat(doc_id, treatment_id);
            }
            catch (Exception e)
            {
                response.Content = new StringContent(e.Message);
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            response.Content = new StringContent("接诊成功！医生:" + doc_id + " 病人：" + treatment_id);
            // tekes表填充treatment_id对应的doc_id
            return response;
        }
        /// <summary>
        /// 开检查单
        /// exam_type ∈ {“blood” : 0, “gastroscope” : 1, “xray” : 2}
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// Test Passed.
        [HttpGet]
        [Route("api/Doctor/WriteExamination/{treatment_id}/{exam_type}")]
        public HttpResponseMessage WriteExamination(string treatment_id, string exam_type)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            string doc_id = HttpContext.Current.User.Identity.Name;
            //doc_id = "16687";
            switch (int.Parse(exam_type))
            {
                case 0:
                case 1:
                case 2:
                    if (!DoctorHelper.WriteExamination(treatment_id, doc_id, int.Parse(exam_type)))
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Content = new StringContent("insert table failed");
                    }
                    break;
                default:
                    response.Content = new StringContent("exam_type invalid");
                    response.StatusCode = HttpStatusCode.BadRequest;
                    break;
            }
            // 开了检查单子 没交费 没做检查
            // examination表设置exam_Id, type, doc_id
            // 其他留空
            // exam表插入数据
            // 把exam_id和treatment_id关联起来
            return response;
        }
        /// <summary>
        /// 获取ALL medicine的所有信息
        /// </summary>
        /// <returns></returns>
        /// Test Passed
        [HttpGet]
        [Route("api/Doctor/GetMedicineList")]
        public HttpResponseMessage GetAllMedicine()
        {
            // 从medicine表获取所有的药品的数据
            ArrayList list = DoctorHelper.GetAllMedicine();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StringContent(JsonObjectConverter.ObjectToJson(list));
            return response;
        }

        /// <summary>
        /// 开处方
        /// </summary>
        /// <example>
        /// Post的Json格式：
        /// <code>
        /// ["treat_id",{ id: '11', number: '2' }, { id: '13', number: '2' }, { id: '114', number: '5' }, { id: '611', number: '2' }];
        /// </code>
        /// </example>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// Test passed
        [HttpPost]
        [Route("api/Doctor/WritePrescription")]
        public HttpResponseMessage WritePrescription(dynamic obj)
        {
            ArrayList medicine_id = new ArrayList();
            ArrayList num = new ArrayList();
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                string jsonRes = JsonObjectConverter.ObjectToJson(obj);
                var strObject = JsonConvert.DeserializeObject(jsonRes) as JArray;

                var treatment_id = strObject[0].ToString();
                // 遍历strObject得到每一个药物
                for (int i = 1; i < strObject.Count; i++)
                {
                    JObject tmpObj = JObject.Parse(strObject[i].ToString());
                    var id = tmpObj["id"];
                    var number = tmpObj["number"];
                    medicine_id.Add(id);
                    num.Add(number);
                }
                string doc_id = "16656";
                // TODO:
                // doc_id随机分配一个而不是从Cookie来
                ArrayList empList = UserHelper.GetAllEmployee();
                ArrayList pharmacist = new ArrayList();
                foreach (EmployeeInfo item in empList)
                {
                    // 获取所有药剂师
                    if (item.post.Equals("Pharmacist"))
                    {
                        pharmacist.Add(item);
                    }
                }
                // 随机选择一个药剂师
                Random rand = new Random();
                if (pharmacist.Count > 0)
                {
                    int index = rand.Next(0, pharmacist.Count - 1);
                    doc_id = ((EmployeeInfo)pharmacist[index]).employee_id;
                }

                // prescription表创建一条记录
                // 设置pres_id treat_id doc_id从药剂师中随机挑选 time
                // 其他空值
                // prescribeb表创建一条记录
                // 设置pres_id medicine_id num
                DoctorHelper.WritePrescription(treatment_id, doc_id, medicine_id, num);
            }
            catch (Exception e)
            {
                response.Content = new StringContent(e.Message);
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            response.Content = new StringContent("开方成功！");
            return response;
        }
        /// <summary>
        /// 开手术单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// Test Passed
        [HttpGet]
        [Route("api/Doctor/WriteSurgey/{treatment_id}/{surgey_name}")]
        public HttpResponseMessage WriteSurgey(string treatment_id, string surgey_name)
        {
            // surgery表插入数据
            // 设置surg_id treat_id surgey_name start_time end_time pay
            HttpResponseMessage response = new HttpResponseMessage();
            if (!DoctorHelper.WriteSurgery(treatment_id, surgey_name))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("添加失败！");
            }
            return response;
        }
        /// <summary>
        /// 开住院单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// Test Passed
        [HttpGet]
        [Route("api/Doctor/WriteHospitalization/{treatment_id}")]
        public HttpResponseMessage WriteHospitalization(string treatment_id)
        {
            // 一个住院病人只有一个护士 一个护士可以有多个住院病人
            // hospitalization表增加
            // 设置hos_id treat_id nurse_id随机生成，在employee表查找 bed_num需要唯一 in_time系统获取
            // out_time空 pay随机生成
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                DoctorHelper.WriteHospitalization(treatment_id);
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent(e.Message);
            }
            return response;
        }
    }
}
