using AspMVCMeeting.Models;
using AspMVCMeeting.Reports;
using AspMVCMeeting.Reports.DS_MasterReportTableAdapters;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AspMVCMeeting.Reports.DS_MasterReport;

namespace AspMVCMeeting.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        MeetingDataModelCodeFirst db = new MeetingDataModelCodeFirst();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}