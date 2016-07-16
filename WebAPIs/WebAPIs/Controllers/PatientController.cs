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
    public class PatientController : BaseController
    {
        public string accessRoles = "Patient";

        public IEnumerable<Clinic> GetAllClinic()
        {
            GenerateUserInfoByCookie();
            // 如果用户权限正确
            if (HttpContext.Current.User.IsInRole(accessRoles))
            {
                return new Clinic[1];
            }
            else
            {
                return null;
            }
        }
        
    }
}
