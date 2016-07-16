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
    public class AccountController : ApiController
    {
        private bool GenerateUserInfoByCookie()
        {
            FormsAuthenticationTicket ticket = null;

            var cookies = Request.Headers.GetCookies();
            foreach (var perCookie in cookies[0].Cookies)
            {
                if (perCookie.Name == FormsAuthentication.FormsCookieName)
                {
                    ticket = FormsAuthentication.Decrypt(perCookie.Value);
                    break;
                }
            }
            if (ticket == null)
                return false;

            string[] roles = ticket.UserData.Split(',');
            IIdentity identity = new FormsIdentity(ticket);
            IPrincipal principal = new GenericPrincipal(identity, roles);
            HttpContext.Current.User = principal;

            return true;
        }
        [HttpPost]
        public HttpResponseMessage SignIn([FromBody]LoginUser user)
        {
            var accountModel = new AccountModel();
            
            if (accountModel.ValidateUserLogin(user.account, user.passwd))
            {
                //创建用户ticket信息
                accountModel.CreateLoginUserTicket(user.account, user.passwd);
                
                var response = Request.CreateResponse(HttpStatusCode.OK);
                //response.Headers.Add("FORCE_REDIRECT", "http://www.baidu.com");
                return response;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }
        [HttpPost]
        public string GetUserInfo()
        {
            GenerateUserInfoByCookie();
            string userAccount = HttpContext.Current.User.Identity.Name;
            // 如果用户是病人
            if (userAccount.Length == 9)
            {
                // TODO:从数据库取病人的信息
                return "";
            }
            else if (userAccount.Length == 5)
            {
                // TODO:从数据库获取雇员的信息
                return "";
            }
            else
            {
                return "Invalid account length";
            }
        }
        [HttpPost]
        public HttpResponseMessage SignUp([FromBody]SignUpUser user)
        {

            // 数据库中插入用户信息
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
