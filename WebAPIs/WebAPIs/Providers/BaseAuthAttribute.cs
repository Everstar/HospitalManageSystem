using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebAPIs.Providers
{
    public class BaseAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
        }
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var b = HttpContext.Current.User;
            var a = actionContext.RequestContext.Principal.Identity;
            return b.IsInRole(this.Roles);
        }
    }
}