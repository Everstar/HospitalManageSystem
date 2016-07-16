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

namespace WebAPIs.Controllers
{
    public class AccountController : BaseController
    {
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
            if (accountModel.ValidateUserLogin(userAccount, userPasswd))
            {
                //创建用户ticket信息
                accountModel.CreateLoginUserTicket(userAccount, userPasswd);
                
                var response = Request.CreateResponse(HttpStatusCode.OK);
                //response.Headers.Add("FORCE_REDIRECT", "http://www.baidu.com");
                response.Content = new StringContent(user.ToString());
                return response;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetUserInfo()
        {
            GenerateUserInfoByCookie();
            string userAccount = HttpContext.Current.User.Identity.Name;
            HttpResponseMessage response = new HttpResponseMessage();
            // 如果用户是病人
            if (userAccount.Length == 9)
            {
                // TODO:从数据库取病人的信息
                return response;
            }
            else if (userAccount.Length == 5)
            {
                // TODO:从数据库获取雇员的信息
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
        public HttpResponseMessage SignUp([FromBody]SignUpUser user)
        {
            // 判断用户的id是否存在
            // 数据库中插入用户信息
            // 注册成功 分发cookie
            SignIn(new LoginUser());
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        [HttpGet]
        public string GetStringTest()
        {
            var user = HttpContext.Current.User;
            var a = user.Identity;
            var b = user.IsInRole("Admin");
            var c = user.IsInRole("Hello");
            return "HHHHHHHHHHH";
        }
    }
}
