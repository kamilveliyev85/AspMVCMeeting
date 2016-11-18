using AspMVCMeeting.Models;
using AspMVCMeeting.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;


namespace AspMVCMeeting.Controllers
{

    public class AuthenticationController : BaseController
    {
        MeetingDataModelCodeFirst db = new MeetingDataModelCodeFirst();

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
        public ActionResult Index(string returnUrl)
        {
            Logoff(Session, Response);

            ViewBag.RETURN_URL = returnUrl;
            return View();
        }

        public static void Logoff(HttpSessionStateBase session, HttpResponseBase response)
        {
            // Delete the user details from cache.
            session.Abandon();
            // Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();
            // Clear authentication cookie.
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            response.Cookies.Add(cookie);
        }

        [HttpPost]
        public ActionResult Index(string userName, string userPassword, string returnUrl)
        {
            //EmployeeBusinessLayer bal = new EmployeeBusinessLayer();
            if (IsValidUser(userName, userPassword))
            {
                userName = userName.Replace('i', 'I').Replace('İ', 'I').ToUpper();
                userName = userName.Split('@')[0] + "@SGOFC.COM";
                //userName = "RESAD.YUSIFZADE@SGOFC.COM";
                
                SAP sap = db.SAP.Where(m => m.ACCOUNTNAME == userName).FirstOrDefault();
                User user = new User();
                user.FullName = sap.PNAME + " " + sap.PSURNAME;
                user.UserName = userName;
                user.Code = sap.PERCODE;

                var dbQuery = db.Database.SqlQuery<byte[]>(@"SELECT[PHOTO]
                              FROM[DBHR].[SAPHR].[dbo].[PERINFO_PHOTO] PHOTO
                              where PHOTO.CODE = '" + user.Code + "'");
                //var base64 = Convert.ToBase64String(dbQuery.AsEnumerable().First());
                //imgSrc = String.Format("data:image/png;base64,{0}", base64);

                //Save file to profiles
                var fileName = user.Code + ".png";
                string path = HttpContext.Server.MapPath("~/assets/profiles/") + fileName;
                System.IO.File.WriteAllBytes(path, dbQuery.AsEnumerable().First());

                // Create the authentication ticket with custom user data.
                string userData = JsonConvert.SerializeObject(user);

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                   userName,
                   DateTime.Now,
                   DateTime.Now.AddYears(1),
                   false,
                   userData,
                   FormsAuthentication.FormsCookiePath);

                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                //FormsAuthentication.SetAuthCookie(userName, false);


                //Redirect after login
                string decodedUrl = "";
                if (!string.IsNullOrEmpty(returnUrl))
                    decodedUrl = Server.UrlDecode(returnUrl);

                if (Url.IsLocalUrl(decodedUrl))
                {
                    return Redirect(decodedUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

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