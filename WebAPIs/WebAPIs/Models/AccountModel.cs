using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Collections;
using WebAPIs.Models.UnifiedTable;

namespace WebAPIs.Models
{
    public class AccountModel
    {

        // 参考这篇文章
        // http://blog.csdn.net/zjlovety/article/details/17095627
        /// <summary>  
        /// 创建登录用户的票据信息  
        /// </summary>  
        /// <param name="strUserName"></param>
        internal void CreateLoginUserTicket(string strUserName, string strPassword)
        {
            string authRole = GetUserAuthorities(strUserName);
            //构造Form验证的票据信息  
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, strUserName, DateTime.Now, DateTime.Now.AddMinutes(90),
                true, string.Format("{0}:{1},{2}", strUserName, strPassword, authRole), FormsAuthentication.FormsCookiePath);

            string ticString = FormsAuthentication.Encrypt(ticket);

            //把票据信息写入Cookie
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, ticString));
        }

        /// <summary>
        /// 获取用户权限列表数据  
        /// </summary>  
        /// <param name="userName"></param>  
        /// <returns></returns>  
        internal static string GetUserAuthorities(string account)
        {
            string strAuth = "";
            Hashtable authTable = new Hashtable();
            authTable.Add("Patient", "Patient");
            authTable.Add("Doctor", "Doctor");
            authTable.Add("Examiner", "Examiner");
            authTable.Add("Nurse", "Nurse");
            authTable.Add("Pharmacist", "Pharmacist");
            authTable.Add("ManagementStaff", "ManagementStaff");
            try
            {
                if (account.Length == 9)
                {
                    // 病人权限
                    strAuth = (string)authTable["Patient"];
                }
                else if (account.Length == 5)
                {
                    // 医护人员权限
                    // employee表中找到对应emp_id的元组
                    // 取出post
                    string post = "";
                    EmployeeInfo userInfo = UserHelper.GetEmployeeInfo(account);
                    strAuth = (string)authTable[userInfo.post];
                }
            }
            catch (Exception e)
            {
                strAuth = "fail";
            }

            return strAuth;
        }

        /// <summary>  
        /// 读取数据库用户表数据，判断用户密码是否匹配  
        /// </summary>  
        /// <param name="name">用户名</param>  
        /// <param name="password">密码</param>  
        /// <returns></returns>  
        internal bool ValidateUserLogin(string name, string password)
        {
            string passwordInDatabase = "";
            // 如果用户是病人
            if (name.Length == 9)
            {
                // TODO:从数据库取病人的信息
                passwordInDatabase = UserHelper.GetPwOfPatient(name);

            }
            else if (name.Length == 5)
            {
                // TODO:从数据库获取雇员的信息
                passwordInDatabase = UserHelper.GetPwOfEmployee(name);
            }
            else
            {
                return false;
            } 
            return password.Equals(passwordInDatabase);
        }

        /// <summary>  
        /// 用户注销执行的操作  
        /// </summary>  
        internal void Logout()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            // clear authentication cookie
            //HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            //cookie1.Expires = DateTime.Now.AddYears(-1);
            //HttpContext.Current.Response.Cookies.Add(cookie1);
        }
    }
}