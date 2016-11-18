using AspMVCMeeting.Models;
using AspMVCMeeting.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;

namespace AspMVCMeeting.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            MeetingDataModelCodeFirst db = new MeetingDataModelCodeFirst();
            string userName = string.IsNullOrEmpty(System.Web.HttpContext.Current.User.Identity.Name) ? "empty" : System.Web.HttpContext.Current.User.Identity.Name;
            var allowedStatus = new[] { 5, 9, 12, 13 };

            //APPROVE
            int countApprove =
          (from mtl in db.MEETING_LINES.Where(m => allowedStatus.Contains(m.MTL_STS.Value))
           join mtm in db.MEETING_MASTER on mtl.MTL_MT_REF equals mtm.ID into m1
           from mtm in m1.DefaultIfEmpty()
           where mtl.MTL_DELETED == false && ((mtl.MTL_STS == 12 && mtl.MTL_CONFIRMER == userName)
           || ((mtl.MTL_STS == 13 || mtl.MTL_STS == 9) && mtm.MT_FOLLOWER_USERID == userName))
           select new { MEETING_LINES = mtl }).Count();

            //OFFER
            int countOffer =
             (from mtl in db.MEETING_LINES.Where(m => allowedStatus.Contains(m.MTL_STS.Value))
              where mtl.MTL_DELETED == false && mtl.MTL_OFFER_USER == userName
              select new
              {
                  MEETING_LINES = mtl
              }).Count();

            //FOLLOW
            int countFollow =
         (from mtl in db.MEETING_LINES.Where(m=> allowedStatus.Contains(m.MTL_STS.Value))
          join mtm in db.MEETING_MASTER on mtl.MTL_MT_REF equals mtm.ID into m1
          from mtm in m1.DefaultIfEmpty()
          where mtl.MTL_DELETED == false && (mtl.MTL_RESPONSIBLE == userName
          || mtl.MTL_CONFIRMER == userName || mtm.MT_FOLLOWER_USERID == userName)
          select new { MEETING_LINES = mtl }).Count();

            //TASKS
            var managerList = db.Database
           .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "SHEFCODE =(SELECT LREF FROM  PERINFO WHERE ACCOUNTNAME ='" + userName + "') OR ACCOUNTNAME ='" + userName + "'"))
           .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME })
           .OrderBy(model => model.FULLNAME)
           .ToList();

            var result = String.Join(", ", managerList.Select(model => model.ID).ToArray());

            int countTask =
          (from mtl in db.MEETING_LINES.Where(model => result.Contains(model.MTL_RESPONSIBLE) && allowedStatus.Contains(model.MTL_STS.Value))
           where mtl.MTL_DELETED == false && (mtl.MTL_STS == 5 && mtl.MTL_RESPONSIBLE == userName)

           select new
           {
               MEETING_LINES = mtl
           }).Count();


            //APPROVE
            int countNotification =
          (from mtn in db.MEETING_NOTIFICATIONS
           where mtn.NTF_DELETED == false && mtn.NTF_SEEN == false && mtn.NTF_USERID == userName
           select new { MEETING_NOTIFICATIONS = mtn }).Count();
           


            ViewBag.FOLLOWCOUNT = countFollow;
            ViewBag.OFFERCOUNT = countOffer;
            ViewBag.TASKCOUNT = countTask;
            ViewBag.APPROVECOUNT = countApprove;
            ViewBag.NOTIFICATIONCOUNT = countNotification;

            var imgSrc = "/assets/layouts/layout/img/avatar3_small.jpg";
            if (Request.IsAuthenticated)
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

                string userData = ticket.UserData;
                var user = JsonConvert.DeserializeObject<User>(userData);

                //var dbQuery = db.Database.SqlQuery<byte[]>(@"SELECT[PHOTO]
                //              FROM[DBHR].[SAPHR].[dbo].[PERINFO_PHOTO] PHOTO
                //              where PHOTO.CODE = '" + user.Code + "'");
                //var base64 = Convert.ToBase64String(dbQuery.AsEnumerable().First());

                imgSrc = "/assets/profiles/" + user.Code + ".png"; 

                ViewBag.UserName = user.FullName;
                ViewBag.imgSrc = imgSrc;
            }
            else
            {
                ViewBag.UserName = "";
                ViewBag.imgSrc = imgSrc;
            }
        }
    }
}