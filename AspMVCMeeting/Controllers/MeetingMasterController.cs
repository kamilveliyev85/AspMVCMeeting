using AspMVCMeeting.Models;
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
        public ActionResult Create(MEETING_MASTER meeting_master) {

            

            db.MEETING_MASTER.Add(meeting_master);
            db.SaveChanges();

            ViewBag.MEETING_TYPE = new SelectList(db.MEETING_TYPE.Where(type => type.MTP_ACTIVE == true).ToList(), "ID", "MTP_NAME");
            var managerList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 5 ORDER BY PNAME, PSURNAME"))
                .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME + " (" + type.STATU_T + ")" })
                .ToList();
            ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");

            return View(meeting_master);
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