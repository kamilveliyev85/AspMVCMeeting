using AspMVCMeeting.Models;
using AspMVCMeeting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspMVCMeeting.Controllers
{
    public class MeetingLinesController : Controller
    {
        MeetingDataModelCodeFirst db = new MeetingDataModelCodeFirst();

        // GET: MeetingLines
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(VM_MEETING vm_meetings)
        {

            db.MEETING_MASTER.Add(vm_meetings.MEETING_MASTER);
            db.SaveChanges();

            return View();
        }
    }
}