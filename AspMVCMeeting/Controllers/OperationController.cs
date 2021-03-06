﻿using AngularMVCFileUpload.Models;
using AspMVCMeeting.Models;
using AspMVCMeeting.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AspMVCMeeting.Controllers
{
    [Authorize]
    public class OperationController : BaseController
    {
        MeetingDataModelCodeFirst db = new MeetingDataModelCodeFirst();
        string userName = string.IsNullOrEmpty(System.Web.HttpContext.Current.User.Identity.Name) ? "empty" : System.Web.HttpContext.Current.User.Identity.Name;
        int[] allowedStatus = new[] { 5, 9, 12, 13 };

        // GET: Operation
        public ActionResult Index()
        {
            VM_MEETING vm_meetings = new VM_MEETING();

            var managerList = db.Database
            .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 4 ORDER BY PNAME, PSURNAME"))
            .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME + " (" + type.STATU_T + ")" })
            .ToList();
            ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");

            var managerEmployeesList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "SHEFCODE =(SELECT LREF FROM  DBHR.SAPHR.dbo.PERINFO WHERE ACCOUNTNAME ='" + userName + "') OR ACCOUNTNAME ='" + userName + "'"))
                .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME })
                .OrderBy(model => model.FULLNAME)
                .ToList();

            ViewBag.EMPLOYEES = new SelectList(managerEmployeesList, "ID", "FULLNAME"); ;

            ViewBag.STATUS = new SelectList(db.MEETING_LOG_TYPE.Where(model => model.MLG_ACTIVE == true).Where(model => model.MLG_PAGE == "Operation").Select(model => new { model.ID, model.MLG_NAME }).ToList(), "ID", "MLG_NAME");

            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).ToList(), "ID", "MLN_NAME");
            ViewBag.MTL_PROJECT_CODE = new SelectList(db.MEETING_PROJECTS.Select(model => new { model.ID, PRJ_NAME = model.PRJ_CODE + " " + model.PRJ_NAME }).ToList(), "ID", "PRJ_NAME");
            ViewBag.DEPT = new SelectList(db.DEPT.OrderBy(model => model.FIRMNAME).ToList(), "FIRMNAME", "FIRMNAME");
            ViewBag.DECISION_TYPES = new SelectList(db.DECISION_TYPES.Where(model => model.DCN_ACTIVE == true).Select(model => new { model.ID, model.DCN_NAME }).ToList(), "ID", "DCN_NAME");
            ViewBag.MTL_RELATED_FORM_REF = new SelectList(db.VW_MEETING_LINE.Select(model => new { model.ID, DESCRIPTION = model.MT_TITLE + "/" + model.MTL_DESCRIPTION }).ToList(), "ID", "DESCRIPTION");


            return View(vm_meetings);
        }

        [HttpGet]
        public ActionResult Operations(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VM_MEETING vm_meeting = new VM_MEETING();
            vm_meeting.MEETING_LINES = new MEETING_LINES();

            //vm_meeting.VM_MEETING_LINES =
            //(from mtl in db.MEETING_LINES.Where(model => model.ID == id)
            // select new VM_MEETING_LINES
            // {
            //     MEETING_LINES = mtl
            // }).FirstOrDefault();

            vm_meeting.MEETING_LINES = db.MEETING_LINES.Where(model => model.ID == id).FirstOrDefault();

            return View(vm_meeting);
        }

        [HttpGet]
        public ActionResult Approve()
        {
            VM_MEETING vm_meetings = new VM_MEETING();

            var managerList = db.Database
            .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 4 ORDER BY PNAME, PSURNAME"))
            .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME + " (" + type.STATU_T + ")" })
            .ToList();
            ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");

            ViewBag.STATUS = new SelectList(db.MEETING_LOG_TYPE.Where(model => model.MLG_ACTIVE == true).Where(model => model.MLG_PAGE == "Operation").Select(model => new { model.ID, model.MLG_NAME }).ToList(), "ID", "MLG_NAME");

            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).ToList(), "ID", "MLN_NAME");
            ViewBag.MTL_PROJECT_CODE = new SelectList(db.MEETING_PROJECTS.Select(model => new { model.ID, PRJ_NAME = model.PRJ_CODE + " " + model.PRJ_NAME }).ToList(), "ID", "PRJ_NAME");
            ViewBag.DEPT = new SelectList(db.DEPT.OrderBy(model => model.FIRMNAME).ToList(), "FIRMNAME", "FIRMNAME");
            ViewBag.DECISION_TYPES = new SelectList(db.DECISION_TYPES.Where(model => model.DCN_ACTIVE == true).Select(model => new { model.ID, model.DCN_NAME }).ToList(), "ID", "DCN_NAME");
            ViewBag.MTL_RELATED_FORM_REF = new SelectList(db.VW_MEETING_LINE.Select(model => new { model.ID, DESCRIPTION = model.MT_TITLE + "/" + model.MTL_DESCRIPTION }).ToList(), "ID", "DESCRIPTION");

            return View();
        }

        [HttpGet]
        public ActionResult Notification()
        {
            VM_MEETING vm_meetings = new VM_MEETING();

            return View(vm_meetings);
        }

        [HttpGet]
        public ActionResult Follow()
        {
            VM_MEETING vm_meetings = new VM_MEETING();

            var managerList = db.Database
            .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 4 ORDER BY PNAME, PSURNAME"))
            .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME + " (" + type.STATU_T + ")" })
            .ToList();
            ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");

            var managerEmployeesList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "SHEFCODE =(SELECT LREF FROM  DBHR.SAPHR.dbo.PERINFO WHERE ACCOUNTNAME ='" + userName + "') OR ACCOUNTNAME ='" + userName + "'"))
                .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME })
                .OrderBy(model => model.FULLNAME)
                .ToList();

            ViewBag.EMPLOYEES = new SelectList(managerEmployeesList, "ID", "FULLNAME"); ;

            ViewBag.STATUS = new SelectList(db.MEETING_LOG_TYPE.Where(model => model.MLG_ACTIVE == true).Where(model => model.MLG_PAGE == "Operation").Select(model => new { model.ID, model.MLG_NAME }).ToList(), "ID", "MLG_NAME");

            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).ToList(), "ID", "MLN_NAME");
            ViewBag.MTL_PROJECT_CODE = new SelectList(db.MEETING_PROJECTS.Select(model => new { model.ID, PRJ_NAME = model.PRJ_CODE + " " + model.PRJ_NAME }).ToList(), "ID", "PRJ_NAME");
            ViewBag.DEPT = new SelectList(db.DEPT.OrderBy(model => model.FIRMNAME).ToList(), "FIRMNAME", "FIRMNAME");
            ViewBag.DECISION_TYPES = new SelectList(db.DECISION_TYPES.Where(model => model.DCN_ACTIVE == true).Select(model => new { model.ID, model.DCN_NAME }).ToList(), "ID", "DCN_NAME");
            ViewBag.MTL_RELATED_FORM_REF = new SelectList(db.VW_MEETING_LINE.Select(model => new { model.ID, DESCRIPTION = model.MT_TITLE + "/" + model.MTL_DESCRIPTION }).ToList(), "ID", "DESCRIPTION");


            return View(vm_meetings);
        }

        [HttpGet]
        public ActionResult Offer()
        {
            VM_MEETING vm_meetings = new VM_MEETING();

            var managerList = db.Database
            .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 4 ORDER BY PNAME, PSURNAME"))
            .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME + " (" + type.STATU_T + ")" })
            .ToList();
            ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");

            var managerEmployeesList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "SHEFCODE =(SELECT LREF FROM  DBHR.SAPHR.dbo.PERINFO WHERE ACCOUNTNAME ='" + userName + "') OR ACCOUNTNAME ='" + userName + "'"))
                .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME })
                .OrderBy(model => model.FULLNAME)
                .ToList();

            ViewBag.EMPLOYEES = new SelectList(managerEmployeesList, "ID", "FULLNAME"); ;

            ViewBag.STATUS = new SelectList(db.MEETING_LOG_TYPE.Where(model => model.MLG_ACTIVE == true).Where(model => model.MLG_PAGE == "Operation").Select(model => new { model.ID, model.MLG_NAME }).ToList(), "ID", "MLG_NAME");

            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).ToList(), "ID", "MLN_NAME");
            ViewBag.MTL_PROJECT_CODE = new SelectList(db.MEETING_PROJECTS.Select(model => new { model.ID, PRJ_NAME = model.PRJ_CODE + " " + model.PRJ_NAME }).ToList(), "ID", "PRJ_NAME");
            ViewBag.DEPT = new SelectList(db.DEPT.OrderBy(model => model.FIRMNAME).ToList(), "FIRMNAME", "FIRMNAME");
            ViewBag.DECISION_TYPES = new SelectList(db.DECISION_TYPES.Where(model => model.DCN_ACTIVE == true).Select(model => new { model.ID, model.DCN_NAME }).ToList(), "ID", "DCN_NAME");
            ViewBag.MTL_RELATED_FORM_REF = new SelectList(db.VW_MEETING_LINE.Select(model => new { model.ID, DESCRIPTION = model.MT_TITLE + "/" + model.MTL_DESCRIPTION }).ToList(), "ID", "DESCRIPTION");


            return View(vm_meetings);
        }

        private bool RemoveAllFilesByUserName(string userName)
        {
            string[] filePaths = Directory.GetFiles(HttpContext.Server.MapPath("~/UploadsTemp/"));

            foreach (var filePath in filePaths)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Name.StartsWith(userName + "_"))
                    System.IO.File.Delete(filePath);
            }

            return true;
        }


        private IList<FileInfoName> listFilesByUserName(string userName)
        {
            IList<FileInfoName> lst_fileInfo = new List<FileInfoName>();
            string[] filePaths = Directory.GetFiles(HttpContext.Server.MapPath("~/UploadsTemp/"));

            foreach (var filePath in filePaths)
            {
                FileInfoName fileInfoName = new FileInfoName();
                FileInfo fileInfo = new FileInfo(filePath);
                fileInfoName.fileName = fileInfo.Name;
                fileInfoName.fileSize = (Math.Round((double)fileInfo.Length / 1024, 0)).ToString() + " KB";
                lst_fileInfo.Add(fileInfoName);
            }

            return lst_fileInfo;
        }

        #region Angular
        [HttpPost]
        public JsonResult GetColumnValues(string columnName, string tableName)
        {
            var dbQuery = db.Database.SqlQuery<FilterModel>("select distinct " + columnName +
               " AS id, " + columnName +
               " AS title from " + tableName + " ORDER BY " + columnName);
            List<FilterModel> titles = dbQuery.ToList();

            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        #region LINE
        [HttpGet]
        public JsonResult GetLinesAll(int? id)
        {
            VM_MEETING vm_meetings = new VM_MEETING();

            var managerList = db.Database
            .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "SHEFCODE =(SELECT LREF FROM  PERINFO WHERE ACCOUNTNAME ='" + userName + "') OR ACCOUNTNAME ='" + userName + "'"))
            .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME })
            .OrderBy(model => model.FULLNAME)
            .ToList();

            var result = String.Join(", ", managerList.Select(model => model.ID).ToArray());

            //ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");
            //managerList.Select(model => model.ID).ToList().Contains(mtl.MTL_RESPONSIBLE)
            vm_meetings.lst_MEETING_LINES =
          (from mtl in db.MEETING_LINES.Where(model => result.Contains(model.MTL_RESPONSIBLE) && allowedStatus.Contains(model.MTL_STS.Value))
           join mlt in db.MEETING_LINE_TYPE on mtl.MTL_TYPE equals mlt.ID
           join mst in db.MEETING_STATUS on mtl.MTL_STS equals mst.ID
           join sap in db.SAP on mtl.MTL_RESPONSIBLE equals sap.ACCOUNTNAME
           join sap1 in db.SAP on mtl.MTL_CONFIRMER equals sap1.ACCOUNTNAME
           where mtl.MTL_DELETED == false && (mtl.MTL_STS == 5 && mtl.MTL_RESPONSIBLE == userName)

           select new VM_MEETING_LINES
           {
               MEETING_LINES = mtl,
               MLN_NAME = mlt.MLN_NAME,
               MTL_STS_TEXT = mst.MST_NAME,
               MTL_CONFIRMER_DESC = sap1.PNAME + " " + sap1.PSURNAME,
               MTL_RESPONSIBLE_DESC = sap.PNAME + " " + sap.PSURNAME
           }).ToList();

            return Json(vm_meetings.lst_MEETING_LINES, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetApproveAll(int? id)
        {
            VM_MEETING vm_meetings = new VM_MEETING();

            vm_meetings.lst_MEETING_LINES =
          (from mtl in db.MEETING_LINES.Where(m => allowedStatus.Contains(m.MTL_STS.Value))
           join mlg in db.MEETING_LOG on mtl.ID equals mlg.MLS_MLG_REF
           where mlg.ID == db.MEETING_LOG.Where(u => u.MLS_MLG_REF == mlg.MLS_MLG_REF).Max(u => u.ID)
           join mtm in db.MEETING_MASTER on mtl.MTL_MT_REF equals mtm.ID into m1
           from mtm in m1.DefaultIfEmpty()
           join mlt in db.MEETING_LINE_TYPE on mtl.MTL_TYPE equals mlt.ID
           join mst in db.MEETING_STATUS on mtl.MTL_STS equals mst.ID
           join lgt in db.MEETING_LOG_TYPE on mlg.MLS_LOG_TYPE equals lgt.ID
           join sap in db.SAP on mtl.MTL_RESPONSIBLE equals sap.ACCOUNTNAME
           join sap1 in db.SAP on mtl.MTL_CONFIRMER equals sap1.ACCOUNTNAME
           join sap2 in db.SAP on mtm.MT_FOLLOWER_USERID equals sap2.ACCOUNTNAME
           where mtl.MTL_DELETED == false && ((mtl.MTL_STS == 12 && mtl.MTL_CONFIRMER == userName)
           || ((mtl.MTL_STS == 13 || mtl.MTL_STS == 9) && mtm.MT_FOLLOWER_USERID == userName))
           select new VM_MEETING_LINES
           {
               MEETING_LINES = mtl,
               MLN_NAME = mlt.MLN_NAME,
               MTL_STS_TEXT = mst.MST_NAME,
               MTL_ACTION_DESC = mlg.MLS_DESCRIPTION,
               MTL_ACTION_TYPE = mlg.MLS_LOG_TYPE,
               MTL_ACTION_TEXT = lgt.MLG_CAPTION,
               MTL_CONFIRMER_DESC = sap1.PNAME + " " + sap1.PSURNAME,
               MTL_RESPONSIBLE_DESC = sap.PNAME + " " + sap.PSURNAME,
               MT_FOLLOWER_DESC = sap2.PNAME + " " + sap2.PSURNAME
           }).ToList();

            return Json(vm_meetings.lst_MEETING_LINES, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string UpdateStatus(VM_MEETING_LINES vm_meetings)
        {
            MEETING_LINES line = db.MEETING_LINES.Find(vm_meetings.MEETING_LINES.ID);

            if (line != null)
            {
                if (vm_meetings.MTL_ACTION_TYPE == 1)
                {
                    line.MTL_STS = 12;

                    MEETING_LOG log = new MEETING_LOG();
                    log.MLS_MLG_REF = line.ID;
                    log.MLS_CREATE_USERID = userName;
                    log.MLS_BROWSER = Request.UserAgent;
                    log.MLS_IP = Request.UserHostAddress;
                    log.MLS_LOG_TYPE = vm_meetings.MTL_ACTION_TYPE;
                    log.MLS_DESCRIPTION = vm_meetings.MTL_ACTION_DESC;

                    db.MEETING_LOG.Add(log);
                    db.SaveChanges();
                }
                else if (vm_meetings.MTL_ACTION_TYPE == 2)
                {
                    line.MTL_STS = 9;

                    MEETING_LOG log = new MEETING_LOG();
                    log.MLS_MLG_REF = line.ID;
                    log.MLS_CREATE_USERID = userName;
                    log.MLS_BROWSER = Request.UserAgent;
                    log.MLS_IP = Request.UserHostAddress;
                    log.MLS_DESCRIPTION = vm_meetings.MTL_ACTION_TEXT;
                    log.MLS_LOG_TYPE = vm_meetings.MTL_ACTION_TYPE;
                    log.MLS_DESCRIPTION = vm_meetings.MTL_ACTION_DESC;

                    db.MEETING_LOG.Add(log);
                    db.SaveChanges();
                }

                var local = db.Set<MEETING_LINES>().Local
                         .FirstOrDefault(f => f.ID == line.ID);
                if (local != null)
                {
                    db.Entry(local).State = EntityState.Detached;
                }
                db.Entry(line).State = EntityState.Modified;
                db.SaveChanges();
                return "Line Updated";
            }
            else
            {
                return "Invalid Line";
            }
        }

        [HttpPost]
        public string UpdateSelectedStatus(string items, int? status)
        {

            Dictionary<int, bool> values = JsonConvert.DeserializeObject<Dictionary<int, bool>>(items);

            if (values != null && status != null)
            {
                bool isSave = false;
                foreach (KeyValuePair<int, bool> item in values)
                {
                    if (item.Value)
                    {
                        var line = db.MEETING_LINES.Find(item.Key);
                        line.MTL_STS = status;
                        db.Entry(line).State = EntityState.Modified;
                        isSave = true;
                    }
                }
                if (isSave) db.SaveChanges();
                return "Line Changed";
            }
            else
            {
                return "Invalid Line";
            }
        }

        [HttpPost]
        public string UpdateStatusApprove(VM_MEETING_LINES vm_meetings)
        {
            MEETING_LINES line = db.MEETING_LINES.Find(vm_meetings.MEETING_LINES.ID);

            if (line != null)
            {
                if (vm_meetings.MTL_ACTION_TYPE == 1)
                {
                    if (line.MTL_STS == 9)
                    {
                        line.MTL_STS = 5;
                    }
                    else if (line.MTL_STS == 12)
                    {
                        line.MTL_STS = 13;
                    }
                    else if (line.MTL_STS == 13)
                    {
                        line.MTL_STS = 6;
                    }
                }
                else if (vm_meetings.MTL_ACTION_TYPE == 2)
                {
                    if (line.MTL_STS == 12)
                    {
                        line.MTL_STS = 5;
                    }
                    else if (line.MTL_STS == 13)
                    {
                        line.MTL_STS = 12;
                    }
                }

                MEETING_LOG log = new MEETING_LOG();
                log.MLS_MLG_REF = line.ID;
                log.MLS_CREATE_USERID = userName;
                log.MLS_BROWSER = Request.UserAgent;
                log.MLS_IP = Request.UserHostAddress;
                log.MLS_LOG_TYPE = vm_meetings.MTL_ACTION_TYPE;
                log.MLS_DESCRIPTION = vm_meetings.MTL_ACTION_DESC;
                db.MEETING_LOG.Add(log);
                db.SaveChanges();

                var local = db.Set<MEETING_LINES>().Local
                         .FirstOrDefault(f => f.ID == line.ID);
                if (local != null)
                {
                    db.Entry(local).State = EntityState.Detached;
                }
                db.Entry(line).State = EntityState.Modified;
                db.SaveChanges();

                MeetingMasterController.updateMasterStatus(line.MTL_MT_REF);

                return "Line Updated";
            }
            else
            {
                return "Invalid Line";
            }

        }

        [HttpPost]
        public string UpdateSelectedStatusApprove(string items, int? status, string desc)
        {

            Dictionary<int, bool> values = JsonConvert.DeserializeObject<Dictionary<int, bool>>(items);

            if (values != null && status != null)
            {
                foreach (KeyValuePair<int, bool> item in values)
                {
                    if (item.Value)
                    {
                        var line = db.MEETING_LINES.Find(item.Key);

                        if (line != null)
                        {
                            if (status == 1)
                            {
                                if (line.MTL_STS == 9)
                                {
                                    line.MTL_STS = 5;
                                }
                                else if (line.MTL_STS == 12)
                                {
                                    line.MTL_STS = 13;
                                }
                                else if (line.MTL_STS == 13)
                                {
                                    line.MTL_STS = 6;
                                }
                            }
                            else if (status == 2)
                            {
                                if (line.MTL_STS == 12)
                                {
                                    line.MTL_STS = 5;
                                }
                                else if (line.MTL_STS == 13)
                                {
                                    line.MTL_STS = 12;
                                }
                            }

                            MEETING_LOG log = new MEETING_LOG();
                            log.MLS_MLG_REF = line.ID;
                            log.MLS_CREATE_USERID = userName;
                            log.MLS_BROWSER = Request.UserAgent;
                            log.MLS_IP = Request.UserHostAddress;
                            log.MLS_LOG_TYPE = status;
                            log.MLS_DESCRIPTION = desc;
                            db.MEETING_LOG.Add(log);
                            db.SaveChanges();

                            db.Entry(line).State = EntityState.Modified;
                            db.SaveChanges();

                            MeetingMasterController.updateMasterStatus(line.MTL_MT_REF);
                        }
                    }
                }
                return "Line Changed";
            }
            else
            {
                return "Invalid Line";
            }
        }


        #endregion LINE

        #region DETAIL
        [HttpGet]
        public JsonResult GetDetailAll(int? id)
        {
            VM_MEETING vm_meetings = new VM_MEETING();

            vm_meetings.lst_MEETING_LINES_DETAIL =
           (from mtd in db.MEETING_LINES_DETAIL
            join sap in db.SAP on mtd.MLD_CREATE_USER equals sap.ACCOUNTNAME into p
            from sap in p.DefaultIfEmpty()
            where mtd.MLD_MTL_REF == id && mtd.MLD_DELETED == false
            orderby mtd.MLD_CREATEDATE descending
            select new VM_MEETING_LINES_DETAIL
            {
                MEETING_LINES_DETAIL = mtd,
                MLD_CREATE_USER_DESC = sap.PNAME + " " + sap.PSURNAME
            }).ToList();

            return Json(vm_meetings.lst_MEETING_LINES_DETAIL, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getDetailById(int? id)
        {
            VM_MEETING vm_meetings = new VM_MEETING();
            MEETING_LINES_DETAIL meeting_details = db.MEETING_LINES_DETAIL.Where(model => model.ID == id).FirstOrDefault();

            RemoveAllFilesByUserName(userName);

            vm_meetings.MEETING_LINES_DETAIL = meeting_details;

            return Json(vm_meetings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string UpdateDetail(VM_MEETING_LINES_DETAIL vm_detail)
        {
            MEETING_LINES_DETAIL detail = vm_detail.MEETING_LINES_DETAIL;

            if (detail != null)
            {
                var local = db.Set<MEETING_LINES>().Local
                         .FirstOrDefault(f => f.ID == detail.ID);
                if (local != null)
                {
                    db.Entry(local).State = EntityState.Detached;
                }
                db.Entry(detail).State = EntityState.Modified;
                db.Entry(detail).Property(m => m.MLD_CREATEDATE).IsModified = false;

                db.SaveChanges();

                return "Line Updated";
            }
            else
            {
                return "Invalid Line";
            }
        }

        [HttpPost]
        public string DeleteDetail(VM_MEETING_LINES_DETAIL vm_detail)
        {
            MEETING_LINES_DETAIL detail = vm_detail.MEETING_LINES_DETAIL;

            if (detail != null)
            {
                var dbQuery = db.Database.SqlQuery<DateTime>("SELECT getdate() ");

                detail.MLD_DELETED = true;
                detail.MLD_DELETED_DATE = dbQuery.AsEnumerable().First();
                db.Entry(detail).State = EntityState.Modified;
                db.SaveChanges();

                notifyUsers(detail);

                return "Line Deleted";
            }
            else
            {
                return "Invalid Line";
            }
        }

        private void notifyUsers(MEETING_LINES_DETAIL detail)
        {
            //BEGIN Notification
            MEETING_LINES line = db.MEETING_LINES.Find(detail.MLD_MTL_REF);
            MEETING_MASTER master = db.MEETING_MASTER.Find(line.MTL_MT_REF);

            MEETING_NOTIFICATIONS notification = new MEETING_NOTIFICATIONS();
            notification.NTF_REFID = detail.ID;
            notification.NTF_SEEN = false;
            notification.NTF_READ = false;
            notification.NTF_DELETED = false;
            var dbQuery = db.Database.SqlQuery<DateTime>("SELECT getdate() ");
            notification.NTF_CREATEDATE = dbQuery.AsEnumerable().First();

            //Comfirmer
            notification.NTF_USERID = line.MTL_CONFIRMER;
            db.MEETING_NOTIFICATIONS.Add(notification);

            //Executant
            var lst_MTL_EXECUTANT = line.MTL_EXECUTANT != null ? line.MTL_EXECUTANT.Split(',').ToList() : Enumerable.Empty<string>();
            foreach (var executant in lst_MTL_EXECUTANT)
            {
                notification.NTF_USERID = executant;
                db.MEETING_NOTIFICATIONS.Add(notification);
            }

            //Follower
            if (master != null)
            {
                notification.NTF_USERID = master.MT_FOLLOWER_USERID;
                db.MEETING_NOTIFICATIONS.Add(notification);
            }

            db.SaveChanges();
            //END Notification
        }

        [HttpPost]
        public string AddDetail(VM_MEETING_LINES_DETAIL vm_detail)
        {
            MEETING_LINES_DETAIL detail = vm_detail.MEETING_LINES_DETAIL;

            if (detail != null)
            {
                db.Entry(detail).State = EntityState.Added;
                var dbQuery = db.Database.SqlQuery<DateTime>("SELECT getdate() ");
                detail.MLD_CREATEDATE = dbQuery.AsEnumerable().First();
                detail.MLD_DELETED = false;
                detail.MLD_CREATE_USER = userName;

                //db.MEETING_LINES_DETAIL.Add(detail);
                db.SaveChanges();

                //Move all files
                string[] filePaths = Directory.GetFiles(HttpContext.Server.MapPath("~/UploadsTemp/"));

                foreach (var filePath in filePaths)
                {
                    FileInfo fileInfo = new FileInfo(filePath);

                    if (fileInfo.Name.StartsWith(userName + "_"))
                    {
                        var fileName = fileInfo.Name.Substring(fileInfo.Name.IndexOf("_") + 1);
                        System.IO.File.Move(filePath, HttpContext.Server.MapPath("~/Uploads/") + fileName);
                        MEETING_FILES meeting_files = new MEETING_FILES();
                        meeting_files.MTF_MT_REF = detail.ID;
                        meeting_files.MTF_TYPE = 2;
                        meeting_files.MTF_CREATE_USERID = userName;
                        meeting_files.MTF_FILENAME = fileName;
                        dbQuery = db.Database.SqlQuery<DateTime>("SELECT getdate() ");
                        meeting_files.MTF_CREATEDATE = dbQuery.AsEnumerable().First();

                        db.MEETING_FILES.Add(meeting_files);
                        db.SaveChanges();
                    }
                }

                notifyUsers(detail);

                return "Line Added";
            }
            else
            {
                return "Invalid Line";
            }
        }

        #endregion DETAIL

        #region DETAILFILES
        //BEGIN Upload files for the Create page in Meeting Master
        [HttpPost]

        public JsonResult UploadDetailFileCreate(HttpPostedFileBase aFile)
        {
            if (aFile != null)
            {
                //TODO user session id should include
                var fileName = userName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + aFile.FileName;
                string path = HttpContext.Server.MapPath("~/UploadsTemp/");
                aFile.SaveAs(path + fileName);

                IList<FileInfoName> lst_fileInfo = listFilesByUserName(userName);

                return Json(lst_fileInfo, JsonRequestBehavior.AllowGet);
            }
            return Json("File is not available", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult RemoveDetailFile(string fileName)
        {
            var fileLoc = HttpContext.Server.MapPath("~/UploadsTemp/") + fileName;
            if (System.IO.File.Exists(fileLoc))
            {
                System.IO.File.Delete(fileLoc);
            }

            IList<FileInfoName> lst_fileInfo = listFilesByUserName(userName);

            return Json(lst_fileInfo, JsonRequestBehavior.AllowGet);
        }

        //END Upload files for the Create page in Meeting Master


        //BEGIN Upload files for the Edit page in Meeting Master

        [HttpPost]

        public string UploadDetailFileEdit(HttpPostedFileBase aFile, int? detailId)
        {
            if (aFile != null)
            {
                //TODO user session id should include
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + aFile.FileName;
                string path = HttpContext.Server.MapPath("~/Uploads/");
                aFile.SaveAs(path + fileName);

                MEETING_FILES meeting_files = new MEETING_FILES();
                meeting_files.MTF_MT_REF = detailId;
                meeting_files.MTF_TYPE = 2;
                meeting_files.MTF_CREATE_USERID = userName;
                meeting_files.MTF_FILENAME = fileName;
                var dbQuery = db.Database.SqlQuery<DateTime>("SELECT getdate() ");
                meeting_files.MTF_CREATEDATE = dbQuery.AsEnumerable().First();

                db.MEETING_FILES.Add(meeting_files);
                db.SaveChanges();

                return "successfully uploaded";
            }
            return "could not upload";
        }

        [HttpGet]

        public JsonResult GetAllDetailFiles(int? id)
        {
            var meeting_files = db.MEETING_FILES.Where(model => model.MTF_MT_REF == id && model.MTF_TYPE == 2).Select(model => new { model.MTF_FILENAME, model.ID }).ToList();
            return Json(meeting_files, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public string removeDetailFileByID(int? fileId)
        {
            var file = db.MEETING_FILES.Find(fileId);
            if (file != null)
            {
                db.MEETING_FILES.Remove(file);
                db.SaveChanges();

                var fileLoc = HttpContext.Server.MapPath("~/Uploads/") + file.MTF_FILENAME;
                if (System.IO.File.Exists(fileLoc))
                {
                    System.IO.File.Delete(fileLoc);
                }
            }

            return "successfully deleted";
        }

        //END Upload files for the Edit page in Meeting Master

        #endregion DETAILFILES


        #region OFFER
        [HttpGet]
        public JsonResult GetOfferAll()
        {
            VM_MEETING vm_meetings = new VM_MEETING();

            vm_meetings.lst_MEETING_LINES =
          (from mtl in db.MEETING_LINES.Where(m => allowedStatus.Contains(m.MTL_STS.Value))
           join mlg in db.MEETING_LOG on mtl.ID equals mlg.MLS_MLG_REF
           where mlg.ID == db.MEETING_LOG.Where(u => u.MLS_MLG_REF == mlg.MLS_MLG_REF).Max(u => u.ID)
           join mtm in db.MEETING_MASTER on mtl.MTL_MT_REF equals mtm.ID into m1
           from mtm in m1.DefaultIfEmpty()
           join mlt in db.MEETING_LINE_TYPE on mtl.MTL_TYPE equals mlt.ID
           join mst in db.MEETING_STATUS on mtl.MTL_STS equals mst.ID
           join sap in db.SAP on mtl.MTL_RESPONSIBLE equals sap.ACCOUNTNAME
           join sap1 in db.SAP on mtl.MTL_CONFIRMER equals sap1.ACCOUNTNAME
           join lgt in db.MEETING_LOG_TYPE on mlg.MLS_LOG_TYPE equals lgt.ID
           where mtl.MTL_DELETED == false && mtl.MTL_OFFER_USER == userName
           select new VM_MEETING_LINES
           {
               MEETING_LINES = mtl,
               MLN_NAME = mlt.MLN_NAME,
               MTL_STS_TEXT = mst.MST_NAME,
               MTL_ACTION_DESC = mlg.MLS_DESCRIPTION,
               MTL_ACTION_TYPE = mlg.MLS_LOG_TYPE,
               MTL_ACTION_TEXT = lgt.MLG_CAPTION,
               MTL_CONFIRMER_DESC = sap1.PNAME + " " + sap1.PSURNAME,
               MTL_RESPONSIBLE_DESC = sap.PNAME + " " + sap.PSURNAME
           }).ToList();

            return Json(vm_meetings.lst_MEETING_LINES, JsonRequestBehavior.AllowGet);
        }
        #endregion OFFER

        #region FOLLOW
        [HttpGet]
        public JsonResult GetFollowAll()
        {
            VM_MEETING vm_meetings = new VM_MEETING();

            vm_meetings.lst_MEETING_LINES =
          (from mtl in db.MEETING_LINES.Where(m => allowedStatus.Contains(m.MTL_STS.Value))
           join mlg in db.MEETING_LOG on mtl.ID equals mlg.MLS_MLG_REF
           where mlg.ID == db.MEETING_LOG.Where(u => u.MLS_MLG_REF == mlg.MLS_MLG_REF).Max(u => u.ID)
           join mtm in db.MEETING_MASTER on mtl.MTL_MT_REF equals mtm.ID into m1
           from mtm in m1.DefaultIfEmpty()
           join mlt in db.MEETING_LINE_TYPE on mtl.MTL_TYPE equals mlt.ID
           join mst in db.MEETING_STATUS on mtl.MTL_STS equals mst.ID
           join sap in db.SAP on mtl.MTL_RESPONSIBLE equals sap.ACCOUNTNAME
           join sap1 in db.SAP on mtl.MTL_CONFIRMER equals sap1.ACCOUNTNAME
           join lgt in db.MEETING_LOG_TYPE on mlg.MLS_LOG_TYPE equals lgt.ID
           where mtl.MTL_DELETED == false && (mtl.MTL_RESPONSIBLE == userName
           || mtl.MTL_CONFIRMER == userName || mtm.MT_FOLLOWER_USERID == userName)
           select new VM_MEETING_LINES
           {
               MEETING_LINES = mtl,
               MLN_NAME = mlt.MLN_NAME,
               MTL_STS_TEXT = mst.MST_NAME,
               MTL_ACTION_DESC = mlg.MLS_DESCRIPTION,
               MTL_ACTION_TYPE = mlg.MLS_LOG_TYPE,
               MTL_ACTION_TEXT = lgt.MLG_CAPTION,
               MTL_CONFIRMER_DESC = sap1.PNAME + " " + sap1.PSURNAME,
               MTL_RESPONSIBLE_DESC = sap.PNAME + " " + sap.PSURNAME
           }).ToList();

            return Json(vm_meetings.lst_MEETING_LINES, JsonRequestBehavior.AllowGet);
        }
        #endregion FOLLOW

        #region NOTIFICATION
        [HttpGet]
        public JsonResult GetNotificationAll()
        {
            VM_MEETING vm_meetings = new VM_MEETING();

            vm_meetings.lst_MEETING_NOTIFICATIONS =
           (from mtn in db.MEETING_NOTIFICATIONS
            join mtd in db.MEETING_LINES_DETAIL on mtn.NTF_REFID equals mtd.ID
            let fileCount =
            (
              from files in db.MEETING_FILES.Where(m => m.MTF_TYPE == 2)
              where mtd.ID == files.MTF_MT_REF
              select files
            ).Count()
            join mtl in db.MEETING_LINES on mtd.MLD_MTL_REF equals mtl.ID into m
            from mtl in m.DefaultIfEmpty()
            join mlt in db.MEETING_LINE_TYPE on mtl.MTL_TYPE equals mlt.ID into m1
            from mlt in m1.DefaultIfEmpty()
            join sap in db.SAP on mtd.MLD_CREATE_USER equals sap.ACCOUNTNAME into p
            from sap in p.DefaultIfEmpty()
            where mtn.NTF_DELETED == false && mtn.NTF_USERID == userName
            orderby mtn.ID descending
            select new VM_MEETING_NOTIFICATIONS
            {
                VM_MEETING_LINES_DETAIL = new VM_MEETING_LINES_DETAIL
                {
                    MEETING_LINES_DETAIL = mtd,
                    MLD_CREATE_USER_DESC = sap.PNAME + " " + sap.PSURNAME
                },
                MEETING_NOTIFICATIONS = mtn,
                MTL_DESCRIPTION = mtl.MTL_DESCRIPTION,
                MTL_TYPE_DESC = mlt.MLN_NAME,
                detailFileCount = fileCount
            }).ToList();

            return Json(vm_meetings.lst_MEETING_NOTIFICATIONS, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getNotificationById(int? id)
        {
            VM_MEETING vm_meetings = new VM_MEETING();
            MEETING_LINES_DETAIL meeting_details = db.MEETING_LINES_DETAIL.Where(model => model.ID == id).FirstOrDefault();

            RemoveAllFilesByUserName(userName);

            vm_meetings.MEETING_LINES_DETAIL = meeting_details;

            return Json(vm_meetings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string removeNotification(int? id)
        {
            MEETING_NOTIFICATIONS notification = db.MEETING_NOTIFICATIONS.Find(id);
            if (notification != null)
            {
                var dbQuery = db.Database.SqlQuery<DateTime>("SELECT getdate() ");
                notification.NTF_DELETED = true;
                notification.NTF_DELETED_DATE = dbQuery.AsEnumerable().First();
                db.Entry(notification).State = EntityState.Modified;
                db.SaveChanges();
                return "Notification Deleted";
            }
            else
            {
                return "Invalid Notification";
            }
        }

        [HttpPost]
        public string replyNotification(int? id)
        {
            MEETING_NOTIFICATIONS notification = db.MEETING_NOTIFICATIONS.Find(id);
            if (notification != null)
            {
                //var dbQuery = db.Database.SqlQuery<DateTime>("SELECT getdate() ");
                //notification.NTF_DELETED = true;
                //notification.NTF_DELETED_DATE = dbQuery.AsEnumerable().First();
                //db.Entry(notification).State = EntityState.Modified;
                //db.SaveChanges();
                return "Notification replied";
            }
            else
            {
                return "Invalid Notification";
            }
        }

        [HttpPost]
        public JsonResult getLineById(int? NTF_REFID)
        {
            MEETING_LINES_DETAIL detail = db.MEETING_LINES_DETAIL.Find(NTF_REFID);
            int? id = detail.MLD_MTL_REF;

            VM_MEETING_LINES vm_meetings = (from mtl in db.MEETING_LINES
                                            join mtm in db.MEETING_MASTER on mtl.MTL_MT_REF equals mtm.ID into m2
                                            from mtm in m2.DefaultIfEmpty()
                                            join mlt in db.MEETING_LINE_TYPE on mtl.MTL_TYPE equals mlt.ID into m1
                                            from mlt in m1.DefaultIfEmpty()
                                            join sap in db.SAP on mtl.MTL_RESPONSIBLE equals sap.ACCOUNTNAME into p
                                            from sap in p.DefaultIfEmpty()
                                            join sap1 in db.SAP on mtl.MTL_CONFIRMER equals sap1.ACCOUNTNAME into p1
                                            from sap1 in p1.DefaultIfEmpty()
                                            join sap2 in db.SAP on mtm.MT_FOLLOWER_USERID equals sap2.ACCOUNTNAME into p2
                                            from sap2 in p2.DefaultIfEmpty()
                                            join sap3 in db.SAP on mtl.MTL_OFFER_USER equals sap3.ACCOUNTNAME into p3
                                            from sap3 in p3.DefaultIfEmpty()
                                            join sap4 in db.SAP on mtl.MTL_OFFER_USER equals sap4.ACCOUNTNAME into p4
                                            from sap4 in p4.DefaultIfEmpty()
                                            join dct in db.DECISION_TYPES on mtl.MTL_DECISION_TYPE equals dct.ID into d
                                            from dct in d.DefaultIfEmpty()

                                            where mtl.ID == id
                                            select new VM_MEETING_LINES
                                            {
                                                MEETING_LINES = mtl,
                                                MTL_TYPE_DESC = mlt.MLN_NAME,
                                                MTL_DECISION_TYPE_DESC = dct.DCN_NAME,
                                                MTL_CONFIRMER_DESC = sap1.PNAME + " " + sap1.PSURNAME,
                                                MTL_RESPONSIBLE_DESC = sap.PNAME + " " + sap.PSURNAME,
                                                MT_FOLLOWER_DESC = sap2.PNAME + " " + sap2.PSURNAME,
                                                MTL_EXECUTANT_DESC = sap4.PNAME + " " + sap4.PSURNAME,
                                                MTL_OFFER_USER_DESC = sap3.PNAME + " " + sap3.PSURNAME
                                            }).FirstOrDefault();
            return Json(vm_meetings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDetailAllByDetailId(int? id, int? NTF_REFID)
        {
            MEETING_NOTIFICATIONS notification = db.MEETING_NOTIFICATIONS.Find(id);
            if (notification.NTF_READ == false || notification.NTF_SEEN == false)
            {
                notification.NTF_READ = true;
                notification.NTF_SEEN = true;
                db.Entry(notification).State = EntityState.Modified;
                db.SaveChanges();
            }

            MEETING_LINES_DETAIL detail = db.MEETING_LINES_DETAIL.Find(NTF_REFID);
            id = detail.MLD_MTL_REF;

            VM_MEETING vm_meetings = new VM_MEETING();

            vm_meetings.lst_MEETING_LINES_DETAIL =
           (from mtd in db.MEETING_LINES_DETAIL
            let fileCount =
            (
              from files in db.MEETING_FILES.Where(m => m.MTF_TYPE == 2)
              where mtd.ID == files.MTF_MT_REF
              select files
            ).Count()
            join sap in db.SAP on mtd.MLD_CREATE_USER equals sap.ACCOUNTNAME into p
            from sap in p.DefaultIfEmpty()
            where mtd.MLD_MTL_REF == id && mtd.MLD_DELETED == false
            orderby mtd.MLD_CREATEDATE descending
            select new VM_MEETING_LINES_DETAIL
            {
                MEETING_LINES_DETAIL = mtd,
                MLD_CREATE_USER_DESC = sap.PNAME + " " + sap.PSURNAME,
                detailFileCount = fileCount
            }).ToList();

            return Json(vm_meetings.lst_MEETING_LINES_DETAIL, JsonRequestBehavior.AllowGet);
        }

        #endregion NOTIFICATION

        #endregion Angular

    }
}