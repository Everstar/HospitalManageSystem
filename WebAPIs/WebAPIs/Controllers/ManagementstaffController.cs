using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPIs.Controllers
{
    [Authorize(Roles = "Managementstaff")]
    public class ManagementstaffController : BaseController
    {
        /// <summary>
        /// 查询人事信息
        /// </summary>
        /// <permission cref="Managementstaff"></permission>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ManagementStaff/GetAllEmployee")]
        public HttpResponseMessage GetAllEmployee()
        {
            // 数据库中找到所有employee的信息
            // 序列化成Json
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }
        /// <summary>
        /// 人事调配
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ManagementStaff/SetEmployee")]
        public HttpResponseMessage SetEmployee(dynamic obj)
        {
            string department = obj.department.Value;
            string clinic = obj.clinic.Value;
            string post = obj.post.Value;
            string salary = obj.salary.Value;
            HttpResponseMessage response = new HttpResponseMessage();
            // 增加人员 删除人员 
            return response;
        }
        /// <summary>
        /// 查询投诉率大于n%的医生
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ManagementStaff/GetComplaintedDoctor/{percent}")]
        public HttpResponseMessage GetComplaintedDoctor(string percent)
        {
            // 被投诉是指某个医生的Rank小于等于两颗星星
            // 在evaluation表找到每一个doc_id下的数据总数 找到doc_id对应的rank小于等于2的数量
            // 如果这个比例大于percent
            // 记录下这个医生的doc_id
            // 返回一个doc_id的列表
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }
        [HttpPost]
        [Route("api/ManagementStaff/SetDutyTime")]
        public HttpResponseMessage SetDutyTime(dynamic obj)
        {
            string room_num = obj.room_num.Value;
            string Monday = obj.Monday.Value;
            string Tuesday = obj.Tuesday.Value;
            string wednesday = obj.wednesday.Value;
            string Thursday = obj.Thursday.Value;
            string Friday = obj.Friday.Value;
            // duty表生成id
            // 设置room_num
            // max_limit设置成随机数
            // 日期随便设置了 上下午。。。
            HttpResponseMessage response = new HttpResponseMessage();
            return response;
        }
    }
}
