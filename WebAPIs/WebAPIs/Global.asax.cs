using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace WebAPIs
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_PostAuthorizeRequest(Object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;
            if (context == null)
                throw new ArgumentException("Context invalid!");
            HttpCookie cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                return;
            try
            {
                // 2. 解密Cookie值，获取FormsAuthenticationTicket对象
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                string[] roles = ticket.UserData.Split(',');
                IIdentity identity = new FormsIdentity(ticket);
                IPrincipal principal = new GenericPrincipal(identity, roles);
                HttpContext.Current.User = principal;
                context.User = principal;

                //if (ticket != null && string.IsNullOrEmpty(ticket.UserData) == false)
                //    // 3. 还原用户数据
                //    userData = (new JavaScriptSerializer()).Deserialize<TUserData>(ticket.UserData);

                //if (ticket != null && userData != null)
                //    // 4. 构造我们的MyFormsPrincipal实例，重新给context.User赋值。
                //    context.User = new MyFormsPrincipal<TUserData>(ticket, userData);
            }
            catch { /* 有异常也不要抛出，防止攻击者试探。 */ }
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;
            if (context == null)
                throw new ArgumentException("Context invalid!");
            HttpCookie cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                return;
            try
            {
                // 2. 解密Cookie值，获取FormsAuthenticationTicket对象
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                string[] roles = ticket.UserData.Split(',');
                IIdentity identity = new FormsIdentity(ticket);
                IPrincipal principal = new GenericPrincipal(identity, roles);
                HttpContext.Current.User = principal;
                context.User = principal;

                //if (ticket != null && string.IsNullOrEmpty(ticket.UserData) == false)
                //    // 3. 还原用户数据
                //    userData = (new JavaScriptSerializer()).Deserialize<TUserData>(ticket.UserData);

                //if (ticket != null && userData != null)
                //    // 4. 构造我们的MyFormsPrincipal实例，重新给context.User赋值。
                //    context.User = new MyFormsPrincipal<TUserData>(ticket, userData);
            }
            catch { /* 有异常也不要抛出，防止攻击者试探。 */ }
            
        }

        void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;
            if (context == null)
                throw new ArgumentException("Context invalid!");
            HttpCookie cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                return;
            try
            {
                // 2. 解密Cookie值，获取FormsAuthenticationTicket对象
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                string[] roles = ticket.UserData.Split(',');
                IIdentity identity = new FormsIdentity(ticket);
                IPrincipal principal = new GenericPrincipal(identity, roles);
                HttpContext.Current.User = principal;
                context.User = principal;

                //if (ticket != null && string.IsNullOrEmpty(ticket.UserData) == false)
                //    // 3. 还原用户数据
                //    userData = (new JavaScriptSerializer()).Deserialize<TUserData>(ticket.UserData);

                //if (ticket != null && userData != null)
                //    // 4. 构造我们的MyFormsPrincipal实例，重新给context.User赋值。
                //    context.User = new MyFormsPrincipal<TUserData>(ticket, userData);
            }
            catch { /* 有异常也不要抛出，防止攻击者试探。 */ }
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }
    }
}
