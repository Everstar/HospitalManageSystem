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
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class AccountController : BaseController
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="user">用户账号、密码构成的Json</param>
        /// <returns></returns>
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
                
                //response.Headers.Add("FORCE_REDIRECT", "http://www.baidu.com");
                response.Content = new StringContent("登陆成功！" + " " + AccountModel.GetUserAuthorities(userAccount));
                return response;
            }
            else
            {
                response.Content = new StringContent("用户名/密码不正确！");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
        }
        //[Authorize]
        [HttpGet]
        [Route("api/Account/GetUserInfo")]
        public HttpResponseMessage GetUserInfo()
        {
            string userAccount = HttpContext.Current.User.Identity.Name;
            userAccount = "14001";
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StringContent(JsonObjectConverter.ObjectToJson(new Patient()));
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
                signUpUser.birth = user.birth.Value;
                signUpUser.name = user.name.Value;
                signUpUser.sex = user.sex.Value;
                signUpUser.credit_num = user.id.Value;

                signUpUser = JsonConvert.DeserializeAnonymousType(JsonObjectConverter.ObjectToJson(user), signUpUser);
            }
            catch (Exception e)
            {
                response.Content = new StringContent("post数据格式错误\nReceives:\n"+JsonObjectConverter.ObjectToJson(user));
                return response;
            }
            // 判断用户的id是否存在
            // 数据库中插入用户信息
            if (!UserHelper.SignUp(signUpUser))
            {
                response.Content = new StringContent("该账号已注册过！");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            // 注册成功 分发cookie
            SignIn(user);
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
        /// <summary>
        /// 登出，通过Cookie判断用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage SingOut()
        {
            var accountModel = new AccountModel();
            accountModel.Logout();
            return new HttpResponseMessage(HttpStatusCode.Redirect);
        }

    }
}
