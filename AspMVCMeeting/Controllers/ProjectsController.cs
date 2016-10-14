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
    public class ProjectsController : Controller
    {
        MeetingDataModelCodeFirst db = new MeetingDataModelCodeFirst();

        // GET: Projects
        public ActionResult Index()
        {
            VM_PROJECTS vm_projects = new VM_PROJECTS();
            vm_projects.lst_MEETING_PROJECTS = db.MEETING_PROJECTS.ToList();
            return View(vm_projects);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var managerList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 5 ORDER BY PNAME, PSURNAME"))
                .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME + " (" + type.STATU_T + ")" })
                .ToList();
            ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");
            ViewBag.CURRENCY = new SelectList(db.CURRENCies.Select(model => new { model.ID, model.prb_kod }).ToList(), "ID", "prb_kod");
            ViewBag.DEPT = new SelectList(db.DEPT.OrderBy(model => model.FIRMNAME).ToList(), "FIRMNR", "FIRMNAME");

            return View();
        }


        [HttpPost]
        public ActionResult Create(VM_PROJECTS vm_projects)
        {
            vm_projects.MEETING_PROJECTS.PRJ_MEMBERS = String.Join(",", vm_projects.lst_PRJ_MEMBERS.ToArray());

            db.MEETING_PROJECTS.Add(vm_projects.MEETING_PROJECTS);
            db.SaveChanges();

            var managerList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 5 ORDER BY PNAME, PSURNAME"))
                .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME + " (" + type.STATU_T + ")" })
                .ToList();
            ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");
            ViewBag.CURRENCY = new SelectList(db.CURRENCies.Select(model => new { model.ID, model.prb_kod }).ToList(), "ID", "prb_kod");
            ViewBag.DEPT = new SelectList(db.DEPT.OrderBy(model => model.FIRMNAME).ToList(), "FIRMNR", "FIRMNAME");

            return View();
        }
    }
}