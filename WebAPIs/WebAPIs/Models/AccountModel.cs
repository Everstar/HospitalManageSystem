using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;


namespace WebAPIs.Models
{
    public class AccountModel : IRequiresSessionState
    {
        /// <summary>  
        /// 创建登录用户的票据信息  
        /// </summary>  
        /// <param name="strUserName"></param>  
        internal void CreateLoginUserTicket(string strUserName, string strPassword)
        {
            //构造Form验证的票据信息  
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, strUserName, DateTime.Now, DateTime.Now.AddMinutes(90),
                true, string.Format("{0}:{1}", strUserName, strPassword), FormsAuthentication.FormsCookiePath);

            string ticString = FormsAuthentication.Encrypt(ticket);

            //把票据信息写入Cookie和Session  
            //SetAuthCookie方法用于标识用户的Identity状态为true  
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, ticString));
            FormsAuthentication.SetAuthCookie(strUserName, true);
            HttpContext.Current.Session["USER_LOGON_TICKET"] = ticString;

            //重写HttpContext中的用户身份，可以封装自定义角色数据；  
            //判断是否合法用户，可以检查：HttpContext.User.Identity.IsAuthenticated的属性值  
            string[] roles = ticket.UserData.Split(',');
            IIdentity identity = new FormsIdentity(ticket);
            IPrincipal principal = new GenericPrincipal(identity, roles);
            HttpContext.Current.User = principal;
        }

        /// <summary>  
        /// 读取数据库用户表数据，判断用户密码是否匹配  
        /// </summary>  
        /// <param name="name"></param>  
        /// <param name="password"></param>  
        /// <returns></returns>  
        internal bool ValidateUserLogin(string name, string password)
        {
            //bool isValid = password == passwordInDatabase;  
            return true;
        }
    }
}