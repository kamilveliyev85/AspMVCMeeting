using AspMVCMeeting.Models;
using AspMVCMeeting.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AspMVCMeeting.Controllers
{
    public class ReportController : Controller
    {
        private MeetingDataModelCodeFirst db = new MeetingDataModelCodeFirst();

        [HttpGet]
        public ActionResult MeetingMaster(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEETING_MASTER meeting_master = db.MEETING_MASTER.Find(id);
            if (meeting_master == null)
            {
                return HttpNotFound();
            }

            VM_MEETING vm_meetings = new VM_MEETING();

            vm_meetings.MEETING_MASTER = meeting_master;

            ViewBag.MEETING_TYPE = new SelectList(db.MEETING_TYPE.Where(type => type.MTP_ACTIVE == true).ToList(), "ID", "MTP_NAME");
            var managerList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 4 ORDER BY PNAME, PSURNAME"))
                .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME + " (" + type.STATU_T + ")" })
                .ToList();
            ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");
            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).Select(model => new { MTL_TYPE = model.ID, MLN_NAME = model.MLN_NAME }).ToList(), "MTL_TYPE", "MLN_NAME");
            ViewBag.MTL_PROJECT_CODE = new SelectList(db.MEETING_PROJECTS.Select(model => new { model.ID, PRJ_NAME = model.PRJ_CODE + " " + model.PRJ_NAME }).ToList(), "ID", "PRJ_NAME");
            ViewBag.DEPT = new SelectList(db.DEPT.OrderBy(model => model.FIRMNAME).ToList(), "FIRMNAME", "FIRMNAME");
            ViewBag.DECISION_TYPES = new SelectList(db.DECISION_TYPES.Where(model => model.DCN_ACTIVE == true).Select(model => new { model.ID, model.DCN_NAME }).ToList(), "ID", "DCN_NAME");
            ViewBag.MTL_RELATED_FORM_REF = new SelectList(db.VW_MEETING_LINE.Select(model => new { model.ID, DESCRIPTION = model.MT_TITLE + "/" + model.MTL_DESCRIPTION }).ToList(), "ID", "DESCRIPTION");
            ViewBag.STATUS = new SelectList(db.MEETING_STATUS.Where(model => model.MST_ACTIVE == true).Where(model => model.MST_TYPE == 2).Select(model => new { model.ID, model.MST_NAME }).ToList(), "ID", "MST_NAME");
            ViewBag.MT_PLEACE = new SelectList(db.MEETING_PLACE.Where(model => model.MP_ACTIVE == true).Select(model => new { model.ID, MP_NAME = model.MP_NAME }).ToList(), "ID", "MP_NAME");

            vm_meetings.lst_MEETING_LINES =
            (from mtl in db.MEETING_LINES
             join mlt in db.MEETING_LINE_TYPE on mtl.MTL_TYPE equals mlt.ID
             join mst in db.MEETING_STATUS on mtl.MTL_STS equals mst.ID
             select new VM_MEETING_LINES { MEETING_LINES = mtl, MLN_NAME = mlt.MLN_NAME, MTL_STS_TEXT = mst.MST_NAME }).ToList();


            ViewBag.MT_NO = vm_meetings.MEETING_MASTER.MT_NO;

            return View("MeetingMaster", vm_meetings);
        }
    }
}