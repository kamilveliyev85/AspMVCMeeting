using AspMVCMeeting.Models;
using AspMVCMeeting.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspMVCMeeting.Controllers
{
    public class MeetingMasterController : Controller
    {
        MeetingDataModelCodeFirst db = new MeetingDataModelCodeFirst();

        // GET: MeetingMaster
        public ActionResult Index()
        {
            return View(db.MEETING_MASTER.ToList());
        }

        [HttpPost]
        public ActionResult Create(VM_MEETING vm_meetings) {

            db.MEETING_MASTER.Add(vm_meetings.MEETING_MASTER);
            db.SaveChanges();

            ViewBag.MEETING_TYPE = new SelectList(db.MEETING_TYPE.Where(type => type.MTP_ACTIVE == true).ToList(), "ID", "MTP_NAME");
            var managerList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 5 ORDER BY PNAME, PSURNAME"))
                .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME + " (" + type.STATU_T + ")" })
                .ToList();
            ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");

            return View(vm_meetings);
        }

        [HttpPost]
        public ActionResult CreateMeetingLines(VM_MEETING vm_meetings) {

            return View();
        }

        [HttpGet]
        public ActionResult Create() {

            ViewBag.MEETING_TYPE = new SelectList(db.MEETING_TYPE.Where(type => type.MTP_ACTIVE == true).ToList(), "ID", "MTP_NAME");
            var managerList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 5 ORDER BY PNAME, PSURNAME"))
                .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME + " (" + type.STATU_T + ")" })
                .ToList();
            ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");

            return View();
        }



    }
}