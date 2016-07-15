using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPIs.Models;

namespace WebAPIs.Controllers
{
    public class AccountController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SignIn([FromBody]LoginUser user)
        {
            var accountModel = new AccountModel();

            if (accountModel.ValidateUserLogin(user.account, user.passwd))
            {
                //创建用户ticket信息
                accountModel.CreateLoginUserTicket(user.account, user.passwd);

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("FORCE_REDIRECT", "http://www.baidu.com");
                return response;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }
    }
}
