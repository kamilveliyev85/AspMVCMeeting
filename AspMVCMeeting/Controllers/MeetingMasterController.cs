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

            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).ToList(), "ID", "MLN_NAME");
            ViewBag.MTL_PROJECT_CODE = new SelectList(db.MEETING_PROJECTS.Select(model => new { model.ID, PRJ_NAME = model.PRJ_CODE + " " + model.PRJ_NAME }).ToList(), "ID", "PRJ_NAME");
            ViewBag.DEPT = new SelectList(db.DEPT.OrderBy(model => model.FIRMNAME).ToList(), "FIRMNR", "FIRMNAME");
            ViewBag.DECISION_TYPES = new SelectList(db.DECISION_TYPES.Where(model => model.DCN_ACTIVE == true).Select(model => new { model.ID, model.DCN_NAME }).ToList(), "ID", "DCN_NAME");
            ViewBag.MTL_RELATED_FORM_REF = new SelectList(db.VW_MEETING_LINE.Select(model => new { model.ID, DESCRIPTION = model.MT_TITLE +"/"+ model.MTL_DESCRIPTION }).ToList(), "ID", "DESCRIPTION");
            return View(vm_meetings);
        }

        [HttpPost]
        public ActionResult CreateMeetingLines(VM_MEETING vm_meetings)
        {
            db.MEETING_LINES.Add(vm_meetings.MEETING_LINES);
            db.SaveChanges();

            return View(vm_meetings);
        }


        [HttpGet]
        public ActionResult Create() {

            ViewBag.MEETING_TYPE = new SelectList(db.MEETING_TYPE.Where(type => type.MTP_ACTIVE == true).ToList(), "ID", "MTP_NAME");
            var managerList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 5 ORDER BY PNAME, PSURNAME"))
                .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME + " (" + type.STATU_T + ")" })
                .ToList();
            ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");
            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).ToList(), "ID", "MLN_NAME");
            ViewBag.MTL_PROJECT_CODE = new SelectList(db.MEETING_PROJECTS.Select(model => new { model.ID, PRJ_NAME = model.PRJ_CODE + " " + model.PRJ_NAME }).ToList(), "ID", "PRJ_NAME");
            ViewBag.DEPT = new SelectList(db.DEPT.OrderBy(model => model.FIRMNAME).ToList(), "FIRMNR", "FIRMNAME");
            ViewBag.DECISION_TYPES = new SelectList(db.DECISION_TYPES.Where(model => model.DCN_ACTIVE == true).Select(model => new { model.ID, model.DCN_NAME }).ToList(), "ID", "DCN_NAME");
            ViewBag.MTL_RELATED_FORM_REF = new SelectList(db.VW_MEETING_LINE.Select(model => new { model.ID, DESCRIPTION = model.MT_TITLE + "/" + model.MTL_DESCRIPTION }).ToList(), "ID", "DESCRIPTION");

            VM_MEETING vm_meetings = new VM_MEETING();
            vm_meetings.lst_MEETING_LINES = db.MEETING_LINES.ToList();

            return View(vm_meetings);
        }

    }
}