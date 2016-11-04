using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace AspMVCMeeting.Controllers
{

    public class AuthenticationController : BaseController
    {
        private static bool IsAuthenticated(string usr, string pwd)
        {
            bool authenticated = false;
            try
            {
                //DirectoryEntry entry = new DirectoryEntry(ConfigurationManager.AppSettings["LdapEntry"], usr, pwd);
                //object nativeObject = entry.NativeObject;
                authenticated = true;
            }
            catch (DirectoryServicesCOMException cex)
            {
                //Param.WriteError(cex.Message);
                //not authenticated; reason why is in cex
            }
            catch (Exception ex)
            {
                //Param.WriteError(ex.Message);
                //not authenticated due to some other exception [this is optional]
            }
            return authenticated;
        }

        // GET: Authentication
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            return View();
        }

        [HttpPost]
        public ActionResult Index(string userName, string userPassword)
        {
            //EmployeeBusinessLayer bal = new EmployeeBusinessLayer();
            if (IsValidUser(userName, userPassword))
            {
                userName = userName.Replace('i', 'I').Replace('İ', 'I').ToUpper();
                userName = userName.Split('@')[0] + "@SGOFC.COM";
                //userName = "RESAD.YUSIFZADE@SGOFC.COM";
                FormsAuthentication.SetAuthCookie(userName, false);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CredentialError", "İsdifadəçi adı və şifrə yalnışdır.");
                ViewBag.IsError = 1;
                return View();
            }
        }

        public bool IsValidUser(string userName, string userPassword)
        {
            if (IsAuthenticated(userName, userPassword))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}