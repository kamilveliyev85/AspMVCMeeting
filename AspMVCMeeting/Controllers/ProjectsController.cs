using AspMVCMeeting.Models;
using AspMVCMeeting.ViewModels;
using System;
using System.Collections.Generic;
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
            return View();
        }


        [HttpPost]
        public ActionResult Create(VM_PROJECTS vm_projects)
        {
            return View();
        }
    }
}