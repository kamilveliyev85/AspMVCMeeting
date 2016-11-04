using AspMVCMeeting.Models;
using AspMVCMeeting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspMVCMeeting.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            MeetingDataModelCodeFirst db = new MeetingDataModelCodeFirst();
            string userName = string.IsNullOrEmpty(System.Web.HttpContext.Current.User.Identity.Name) ? "empty" : System.Web.HttpContext.Current.User.Identity.Name;

            int count =
          (from mtl in db.MEETING_LINES
           join mlt in db.MEETING_LINE_TYPE on mtl.MTL_TYPE equals mlt.ID
           join mst in db.MEETING_STATUS on mtl.MTL_STS equals mst.ID
           join mtm in db.MEETING_MASTER on mtl.MTL_MT_REF equals mtm.ID
           where mtl.MTL_DELETED == false && ((mtl.MTL_STS == 12 && mtl.MTL_CONFIRMER == userName)
           || ((mtl.MTL_STS == 13 || mtl.MTL_STS == 9) && mtm.MT_FOLLOWER_USERID == userName))
           select new VM_MEETING_LINES { MEETING_LINES = mtl, MLN_NAME = mlt.MLN_NAME, MTL_STS_TEXT = mst.MST_NAME }).Count();

            ViewBag.MESSAGECOUNT = count;
        }
    }
}