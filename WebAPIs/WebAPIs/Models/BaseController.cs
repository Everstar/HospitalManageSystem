using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace WebAPIs.Controllers
{
    public class BaseController : ApiController
    {
        [NonAction]
        protected bool GenerateUserInfoByCookie()
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
    }
}
