using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using WebAPIs.Models;
using WebAPIs.Providers;
using WebAPIs.Models.DataModels;
using System.Web.Http.Cors;
using WebAPIs.Models.UnifiedTable;
using Newtonsoft.Json;


namespace WebAPIs.Controllers
{
    public class AccountController : BaseController
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="user">用户账号、密码构成的Json</param>
        /// <returns></returns>
        /// Test Passed
        [HttpPost]
        public HttpResponseMessage SignIn(dynamic user)
        {
            var accountModel = new AccountModel();
            string userAccount = "";
            string userPasswd = "";
            try
            {
                userAccount = user.account.Value;
                userPasswd = user.passwd.Value;
            }
            catch(Exception e)
            {

            }

            HttpResponseMessage response = new HttpResponseMessage();
            if (accountModel.ValidateUserLogin(userAccount, userPasswd))
            {
                //创建用户ticket信息
                accountModel.CreateLoginUserTicket(userAccount, userPasswd);
                
                string fail = AccountModel.GetUserAuthorities(userAccount);
                // 用户类别获取
                if (null == fail)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Content = new StringContent("用户权限不正确！无法完成权限映射！");
                }
                else if (fail.Equals("fail"))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
                else
                {
                    response.Content = new StringContent(fail);
                }
                return response;
            }
            else
            {
                response.Content = new StringContent("用户名/密码不正确！");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
        }
        /// <summary>
        /// 获取用户的信息
        /// </summary>
        /// <returns></returns>
        /// Test Passed
        //[Authorize]
        [HttpGet]
        [Route("api/Account/GetUserInfo")]
        public HttpResponseMessage GetUserInfo()
        {
            string userAccount = HttpContext.Current.User.Identity.Name;
            HttpResponseMessage response = new HttpResponseMessage();
            // 如果用户是病人
            if (userAccount.Length == 9)
            {
                // TODO:从数据库取病人的信息
                PatientInfo userInfo = UserHelper.GetPatientInfo(userAccount);
                // userInfo不存在
                if (userInfo == null)
                {
                    response.Content = new StringContent("当前用户不存在！");
                    response.StatusCode = HttpStatusCode.Forbidden;
                }
                else
                {
                    response.Content = new StringContent(JsonObjectConverter.ObjectToJson(userInfo));
                    response.StatusCode = HttpStatusCode.OK;
                }
                return response;
            }
            else if (userAccount.Length == 5)
            {
                // TODO:从数据库获取雇员的信息
                EmployeeInfo userInfo = UserHelper.GetEmployeeInfo(userAccount);
                if (null == userInfo)
                {
                    response.Content = new StringContent("当前用户不存在！");
                    response.StatusCode = HttpStatusCode.Forbidden;
                }
                else
                {
                    response.Content = new StringContent(JsonObjectConverter.ObjectToJson(userInfo));
                    response.StatusCode = HttpStatusCode.OK;
                }
                return response;
            }
            else
            {
                response.Content = new StringContent("用户长度不对哦~");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
        }
        [HttpPost]
        public HttpResponseMessage SignUp(dynamic user)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            SignUpUser signUpUser = new SignUpUser();
            try
            {
                signUpUser = JsonConvert.DeserializeAnonymousType(JsonObjectConverter.ObjectToJson(user), signUpUser);
            }
            catch (Exception e)
            {
                response.Content = new StringContent("post数据格式错误\nReceives:\n" + JsonObjectConverter.ObjectToJson(user));
                return response;
            }
            // 判断用户的id是否存在
            // 数据库中插入用户信息
            string result = UserHelper.SignUp(signUpUser);
            if (result.StartsWith("Already"))
            {
                response.Content = new StringContent(result);
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            else if (result.StartsWith("Insert"))
            {
                response.Content = new StringContent("数据错误:"+result);
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            // 注册成功 分发cookie
            SignIn(user);

            PatientInfo info = UserHelper.GetPatientInfoByCredNum(signUpUser.credit_num);
            response.Content = new StringContent(info.patient_id);
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
        /// <summary>
        /// 登出，通过Cookie判断用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage SignOut()
        {
            var accountModel = new AccountModel();
            accountModel.Logout();
            return Request.CreateResponse(HttpStatusCode.Moved);
        }
    }
}
