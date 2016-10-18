using AspMVCMeeting.Models;
using AspMVCMeeting.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspMVCMeeting.Controllers
{
    public class MeetingLineIndependentController : Controller
    {
        MeetingDataModelCodeFirst db = new MeetingDataModelCodeFirst();

        // GET: MeetingLineIndependent
        public ActionResult Index()
        {
            VM_MEETING vm_meeting = new VM_MEETING();
            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).Select(model => new { MTL_TYPE = model.ID, MLN_NAME = model.MLN_NAME }).ToList(), "MTL_TYPE", "MLN_NAME");
            var managerList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 5 ORDER BY PNAME, PSURNAME"))
                .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME + " (" + type.STATU_T + ")" })
                .ToList();
            ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");
            ViewBag.MTL_PROJECT_CODE = new SelectList(db.MEETING_PROJECTS.Select(model => new { model.ID, PRJ_NAME = model.PRJ_CODE + " " + model.PRJ_NAME }).ToList(), "ID", "PRJ_NAME");
            ViewBag.STATUS = new SelectList(db.MEETING_STATUS.Where(model => model.MST_ACTIVE == true).Where(model => model.MST_TYPE == 2).Select(model => new { model.ID, model.MST_NAME }).ToList(), "ID", "MST_NAME");
            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).Select(model => new { MTL_TYPE = model.ID, MLN_NAME = model.MLN_NAME }).ToList(), "MTL_TYPE", "MLN_NAME");
            ViewBag.DEPT = new SelectList(db.DEPT.OrderBy(model => model.FIRMNAME).ToList(), "FIRMNR", "FIRMNAME");
            ViewBag.DECISION_TYPES = new SelectList(db.DECISION_TYPES.Where(model => model.DCN_ACTIVE == true).Select(model => new { model.ID, model.DCN_NAME }).ToList(), "ID", "DCN_NAME");
            ViewBag.MTL_RELATED_FORM_REF = new SelectList(db.VW_MEETING_LINE.Select(model => new { model.ID, DESCRIPTION = model.MT_TITLE + "/" + model.MTL_DESCRIPTION }).ToList(), "ID", "DESCRIPTION");

            return View(vm_meeting);
        }
    }
}