using AngularMVCFileUpload.Models;
using AspMVCMeeting.Models;
using AspMVCMeeting.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class MeetingMasterController : BaseController
    {
        private MeetingDataModelCodeFirst db = new MeetingDataModelCodeFirst();

        // GET: MeetingMaster
        public ActionResult Index()
        {
            VM_MEETING vm_meetings = new VM_MEETING();
            vm_meetings.lst_MEETING_MASTER =
            (from mt in db.MEETING_MASTER
             join mtt in db.MEETING_TYPE on mt.MT_TYPE equals mtt.ID
             join mts in db.MEETING_STATUS on mt.MT_STS equals mts.ID
             join mtl in db.MEETING_LINES.Where(m=>m.MTL_DELETED == false) on mt.ID equals mtl.MTL_MT_REF into countAll
             join mtl in db.MEETING_LINES.Where(m => m.MTL_STS == 6 && m.MTL_DELETED == false) on mt.ID equals mtl.MTL_MT_REF into countCompletes
             where mt.MT_DELETED == false
             select new VM_MEETING_MASTER
             {
                 MEETING_MASTER = mt,
                 MT_STS_TEXT = mts.MST_NAME,
                 MT_TYPE_TEXT = mtt.MTP_NAME,
                 MT_STS_TEXT_INFO = countCompletes.Count().ToString() + "/" + countAll.Count().ToString()
             }).ToList();
            
            //vm_meetings.lst_MEETING_MASTER = db.MEETING_MASTER.Where(model=>model.MT_DELETED == false).ToList();
            return View(vm_meetings);
        }

        //[HttpPost]
        //public ActionResult CreateMeetingLines(VM_MEETING vm_meetings)
        //{
        //    vm_meetings.MEETING_LINES.MTL_EXECUTANT = (vm_meetings.lst_MTL_EXECUTANT != null) ? String.Join(",", vm_meetings.lst_MTL_EXECUTANT.ToArray()) : string.Empty;

        //    db.MEETING_LINES.Add(vm_meetings.MEETING_LINES);
        //    db.SaveChanges();

        //    return View("Create", vm_meetings);
        //}

        [HttpPost]
        public ActionResult Create(VM_MEETING vm_meetings, IEnumerable<HttpPostedFileBase> files)
        {
            var user_participant = (vm_meetings.lst_MT_USER_PARTICIPANTS != null) ? String.Join(",", vm_meetings.lst_MT_USER_PARTICIPANTS.ToArray()) : string.Empty;
            var user_cc = (vm_meetings.lst_MT_USER_CC != null) ? String.Join(",", vm_meetings.lst_MT_USER_CC.ToArray()) : string.Empty;

            vm_meetings.MEETING_MASTER.MT_USER_PARTICIPANTS = user_participant;
            vm_meetings.MEETING_MASTER.MT_USER_CC = user_cc;

            vm_meetings.MEETING_MASTER.MT_DELETED = false;
            vm_meetings.MEETING_MASTER.MT_STS = 1;

            db.MEETING_MASTER.Add(vm_meetings.MEETING_MASTER);
            db.SaveChanges();

            var mtpType = db.MEETING_TYPE.Where(model => model.ID == vm_meetings.MEETING_MASTER.MT_TYPE).Select(model => model.MTP_NAME).FirstOrDefault();
            vm_meetings.MEETING_MASTER.MT_NO = mtpType + DateTime.Now.ToString("yyyyMMdd") + vm_meetings.MEETING_MASTER.ID;
            db.Entry(vm_meetings.MEETING_MASTER).State = EntityState.Modified;
            db.SaveChanges();


            //Move all files
            string[] filePaths = Directory.GetFiles(HttpContext.Server.MapPath("~/UploadsTemp/"));

            foreach (var filePath in filePaths)
            {
                FileInfo fileInfo = new FileInfo(filePath);

                if (fileInfo.Name.StartsWith(userName + "_"))
                {
                    System.IO.File.Move(filePath, HttpContext.Server.MapPath("~/Uploads/") + fileInfo.Name);
                    MEETING_FILES meeting_files = new MEETING_FILES();
                    meeting_files.MTF_MT_REF = vm_meetings.MEETING_MASTER.ID;
                    meeting_files.MTF_FILENAME = fileInfo.Name;
                    meeting_files.MTF_TYPE = 0;
                    meeting_files.MTF_CREATE_USERID = userName;

                    db.MEETING_FILES.Add(meeting_files);
                    db.SaveChanges();
                }
            }


            ViewBag.MEETING_TYPE = new SelectList(db.MEETING_TYPE.Where(type => type.MTP_ACTIVE == true).ToList(), "ID", "MTP_NAME");
            var managerList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 4 ORDER BY PNAME, PSURNAME"))
                .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME + " (" + type.STATU_T + ")" })
                .ToList();
            ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");

            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).ToList(), "ID", "MLN_NAME");
            ViewBag.MTL_PROJECT_CODE = new SelectList(db.MEETING_PROJECTS.Select(model => new { model.ID, PRJ_NAME = model.PRJ_CODE + " " + model.PRJ_NAME }).ToList(), "ID", "PRJ_NAME");
            ViewBag.DEPT = new SelectList(db.DEPT.OrderBy(model => model.FIRMNAME).ToList(), "FIRMNAME", "FIRMNAME");
            ViewBag.DECISION_TYPES = new SelectList(db.DECISION_TYPES.Where(model => model.DCN_ACTIVE == true).Select(model => new { model.ID, model.DCN_NAME }).ToList(), "ID", "DCN_NAME");
            ViewBag.MTL_RELATED_FORM_REF = new SelectList(db.VW_MEETING_LINE.Select(model => new { model.ID, DESCRIPTION = model.MT_TITLE + "/" + model.MTL_DESCRIPTION }).ToList(), "ID", "DESCRIPTION");
            ViewBag.MT_PLEACE = new SelectList(db.MEETING_PLACE.Where(model => model.MP_ACTIVE == true).Select(model => new { model.ID, MP_NAME = model.MP_NAME }).ToList(), "ID", "MP_NAME");

            vm_meetings.lst_MEETING_LINES =
            (from mtl in db.MEETING_LINES
             join mlt in db.MEETING_LINE_TYPE on mtl.MTL_TYPE equals mlt.ID
             join mst in db.MEETING_STATUS on mtl.MTL_STS equals mst.ID
             select new VM_MEETING_LINES { MEETING_LINES = mtl, MLN_NAME = mlt.MLN_NAME, MTL_STS_TEXT = mst.MST_NAME }).ToList();


            return RedirectToAction("Edit", "MeetingMaster", new { id = vm_meetings.MEETING_MASTER.ID });

            return View(vm_meetings);
        }


        [HttpGet]
        public ActionResult Create()
        {

            RemoveAllFilesByUserName(userName);

            ViewBag.MEETING_TYPE = new SelectList(db.MEETING_TYPE.Where(type => type.MTP_ACTIVE == true).ToList(), "ID", "MTP_NAME");
            var managerList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 4 ORDER BY PNAME, PSURNAME"))
                .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME + " (" + type.STATU_T + ")" })
                .ToList();
            ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");
            ViewBag.MT_PLEACE = new SelectList(db.MEETING_PLACE.Where(model => model.MP_ACTIVE == true).Select(model => new { model.ID, MP_NAME = model.MP_NAME }).ToList(), "ID", "MP_NAME");
            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).Select(model => new { MTL_TYPE = model.ID, MLN_NAME = model.MLN_NAME }).ToList(), "MTL_TYPE", "MLN_NAME");
            ViewBag.MTL_PROJECT_CODE = new SelectList(db.MEETING_PROJECTS.Select(model => new { model.ID, PRJ_NAME = model.PRJ_CODE + " " + model.PRJ_NAME }).ToList(), "ID", "PRJ_NAME");
            ViewBag.DEPT = new SelectList(db.DEPT.OrderBy(model => model.FIRMNAME).ToList(), "FIRMNAME", "FIRMNAME");
            ViewBag.DECISION_TYPES = new SelectList(db.DECISION_TYPES.Where(model => model.DCN_ACTIVE == true).Select(model => new { model.ID, model.DCN_NAME }).ToList(), "ID", "DCN_NAME");
            ViewBag.MTL_RELATED_FORM_REF = new SelectList(db.VW_MEETING_LINE.Select(model => new { model.ID, DESCRIPTION = model.MT_TITLE + "/" + model.MTL_DESCRIPTION }).ToList(), "ID", "DESCRIPTION");

            VM_MEETING vm_meetings = new VM_MEETING();


            vm_meetings.lst_MEETING_LINES =
            (from mtl in db.MEETING_LINES
             join mlt in db.MEETING_LINE_TYPE on mtl.MTL_TYPE equals mlt.ID
             select new VM_MEETING_LINES { MEETING_LINES = mtl, MLN_NAME = mlt.MLN_NAME }).ToList();


            ViewBag.MT_NO = string.Empty;

            return View(vm_meetings);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            RemoveAllFilesByUserName(userName);

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

            return View(vm_meetings);
        }

        [HttpPost]
        public ActionResult Edit(VM_MEETING vm_meetings)
        {
            var user_participant = (vm_meetings.lst_MT_USER_PARTICIPANTS != null) ? String.Join(",", vm_meetings.lst_MT_USER_PARTICIPANTS.ToArray()) : string.Empty;
            var user_cc = (vm_meetings.lst_MT_USER_CC != null) ? String.Join(",", vm_meetings.lst_MT_USER_CC.ToArray()) : string.Empty;

            vm_meetings.MEETING_MASTER.MT_USER_PARTICIPANTS = user_participant;
            vm_meetings.MEETING_MASTER.MT_USER_CC = user_cc;

            var mtpType = db.MEETING_TYPE.Where(model => model.ID == vm_meetings.MEETING_MASTER.MT_TYPE).Select(model => model.MTP_NAME).FirstOrDefault();
            vm_meetings.MEETING_MASTER.MT_NO = mtpType + DateTime.Now.ToString("yyyyMMdd") + vm_meetings.MEETING_MASTER.ID;
            db.Entry(vm_meetings.MEETING_MASTER).State = EntityState.Modified;
            db.SaveChanges();


            ViewBag.MEETING_TYPE = new SelectList(db.MEETING_TYPE.Where(type => type.MTP_ACTIVE == true).ToList(), "ID", "MTP_NAME");
            var managerList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 4 ORDER BY PNAME, PSURNAME"))
                .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME + " (" + type.STATU_T + ")" })
                .ToList();
            ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");

            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).ToList(), "ID", "MLN_NAME");
            ViewBag.MTL_PROJECT_CODE = new SelectList(db.MEETING_PROJECTS.Select(model => new { model.ID, PRJ_NAME = model.PRJ_CODE + " " + model.PRJ_NAME }).ToList(), "ID", "PRJ_NAME");
            ViewBag.DEPT = new SelectList(db.DEPT.OrderBy(model => model.FIRMNAME).ToList(), "FIRMNAME", "FIRMNAME");
            ViewBag.DECISION_TYPES = new SelectList(db.DECISION_TYPES.Where(model => model.DCN_ACTIVE == true).Select(model => new { model.ID, model.DCN_NAME }).ToList(), "ID", "DCN_NAME");
            ViewBag.MTL_RELATED_FORM_REF = new SelectList(db.VW_MEETING_LINE.Select(model => new { model.ID, DESCRIPTION = model.MT_TITLE + "/" + model.MTL_DESCRIPTION }).ToList(), "ID", "DESCRIPTION");
            ViewBag.STATUS = new SelectList(db.MEETING_STATUS.Where(model => model.MST_ACTIVE == true).Where(model => model.MST_TYPE == 2).Select(model => new { model.ID, model.MST_NAME }).ToList(), "ID", "MST_NAME");
            ViewBag.MT_PLEACE = new SelectList(db.MEETING_PLACE.Where(model => model.MP_ACTIVE == true).Select(model => new { model.ID, MP_NAME = model.MP_NAME }).ToList(), "ID", "MP_NAME");

            vm_meetings.lst_MEETING_LINES =
            (from mtl in db.MEETING_LINES
             join mlt in db.MEETING_LINE_TYPE on mtl.MTL_TYPE equals mlt.ID
             select new VM_MEETING_LINES { MEETING_LINES = mtl, MLN_NAME = mlt.MLN_NAME }).ToList();


            ViewBag.MT_NO = vm_meetings.MEETING_MASTER.MT_NO;

            return View(vm_meetings);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MEETING_MASTER item = db.MEETING_MASTER.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            item.MT_DELETED = true;

            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
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


        #region Angular

        #region LINE
        [HttpGet]
        public JsonResult GetLinesAll(int? id)
        {
            VM_MEETING vm_meetings = new VM_MEETING();

            vm_meetings.lst_MEETING_LINES =
           (from mtl in db.MEETING_LINES
            join mlt in db.MEETING_LINE_TYPE on mtl.MTL_TYPE equals mlt.ID into m
            from mlt in m.DefaultIfEmpty()
            join mst in db.MEETING_STATUS on mtl.MTL_STS equals mst.ID into m1
            from mst in m1.DefaultIfEmpty()
            join sap in db.SAP on mtl.MTL_RESPONSIBLE equals sap.ACCOUNTNAME into p
            from sap in p.DefaultIfEmpty()
            join sap1 in db.SAP on mtl.MTL_CONFIRMER equals sap1.ACCOUNTNAME into p1
            from sap1 in p1.DefaultIfEmpty()
            where mtl.MTL_MT_REF == id && mtl.MTL_DELETED == false
            orderby mtl.ID descending
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

        [HttpPost]
        public JsonResult getLineById(int? id)
        {
            VM_MEETING_LINES vm_meetings = new VM_MEETING_LINES();
            MEETING_LINES meeting_lines = db.MEETING_LINES.Where(model => model.ID == id).FirstOrDefault();

            RemoveAllFilesByUserName(userName);

            vm_meetings.MEETING_LINES = meeting_lines;
            vm_meetings.lst_MTL_EXECUTANT = vm_meetings.MEETING_LINES.MTL_EXECUTANT != null ? vm_meetings.MEETING_LINES.MTL_EXECUTANT.Split(',').ToList() : null;
            vm_meetings.lst_MTL_RELATED_FORM_REF = (vm_meetings.MEETING_LINES.MTL_RELATED_FORM_REF != null) ? vm_meetings.MEETING_LINES.MTL_RELATED_FORM_REF.Split(',').ToList() : null;

            return Json(vm_meetings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult findDepByAccount(string accountName)
        {
            string dep = "";

            if (string.IsNullOrEmpty(accountName))
            {
                return Json(dep, JsonRequestBehavior.AllowGet);
            }

            var dbQuery = db.Database.SqlQuery<string>(@"select FIRMNAME from DBHR.SAPHR.dbo.ORG_OLD O 
                            inner join PERINFO p on p.PERCODE = O.PERcode
                            where p.ACCOUNTNAME = '" + accountName + "'");
            dep = dbQuery.AsEnumerable().First();

            //string constr = ConfigurationManager.ConnectionStrings["MeetingDataModelCodeFirst"].ConnectionString;
            //using (SqlConnection con = new SqlConnection(constr))
            //{
            //    string query = @"select FIRMNAME from DBHR.SAPHR.dbo.ORG_OLD O 
            //                inner join PERINFO p on p.PERCODE = O.PERcode
            //                where p.ACCOUNTNAME = '" + accountName + "'";
            //    using (SqlCommand cmd = new SqlCommand(query))
            //    {
            //        cmd.Connection = con;
            //        con.Open();

            //        SqlDataReader reader = cmd.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            dep = reader.GetString(0);
            //        }


            //        con.Close();
            //    }
            //}

            return Json(dep, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string UpdateLine(VM_MEETING_LINES vm_meetings)
        {
            var mtl_executant = (vm_meetings.lst_MTL_EXECUTANT != null) ? String.Join(",", vm_meetings.lst_MTL_EXECUTANT.ToArray()) : string.Empty;
            var MTL_RELATED_FORM_REF = (vm_meetings.lst_MTL_RELATED_FORM_REF != null) ? String.Join(",", vm_meetings.lst_MTL_RELATED_FORM_REF.ToArray()) : string.Empty;

            MEETING_LINES line = vm_meetings.MEETING_LINES;
            line.MTL_EXECUTANT = mtl_executant;
            line.MTL_RELATED_FORM_REF = MTL_RELATED_FORM_REF;

            if (line != null)
            {
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
        public string PublishSelected(string items)
        {
            Dictionary<int, bool> values = JsonConvert.DeserializeObject<Dictionary<int, bool>>(items);

            if (values != null)
            {
                int? id = null;
                foreach (KeyValuePair<int, bool> item in values)
                {
                    if (item.Value)
                    {
                        var line = db.MEETING_LINES.Find(item.Key);
                        if (id == null) id = line.MTL_MT_REF;
                        line.MTL_STS = 5;
                        db.Entry(line).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                updateMasterStatus(id);
                return "Line Changed";
            }
            else
            {
                return "Invalid Line";
            }

        }

        public static void updateMasterStatus(int? ID)
        {
            MeetingDataModelCodeFirst db = new MeetingDataModelCodeFirst();

            var master = db.MEETING_MASTER.Find(ID);
            if (master.MT_STS == 1)
            {
                var count = db.MEETING_LINES.Count(m => m.MTL_MT_REF == ID && m.MTL_STS != 4 && m.MTL_DELETED == false);

                if (count > 0)
                {
                    db.Entry(master).State = EntityState.Modified;
                    master.MT_STS = 2;
                    db.SaveChanges();
                }
            }
            else if (master.MT_STS == 2)
            {
                var count = db.MEETING_LINES.Count(m => m.MTL_MT_REF == ID && m.MTL_DELETED == false && m.MTL_STS != 6);

                if (count == 0)
                {
                    db.Entry(master).State = EntityState.Modified;
                    master.MT_STS = 3;
                    db.SaveChanges();
                }
            }
        }

        [HttpPost]
        public string DeleteLine(VM_MEETING_LINES line)
        {
            if (line != null)
            {
                line.MEETING_LINES.MTL_DELETED = true;
                db.Entry(line.MEETING_LINES).State = EntityState.Modified;
                db.SaveChanges();
                return "Line Deleted";
            }
            else
            {
                return "Invalid Line";
            }
        }

        [HttpPost]
        public JsonResult CopyLine(VM_MEETING_LINES line)
        {
            if (line != null)
            {
                MEETING_LINES lineCopy = db.MEETING_LINES.Find(line.MEETING_LINES.ID);
                lineCopy.MTL_STS = 4;

                db.MEETING_LINES.Add(lineCopy);
                db.SaveChanges();

                VM_MEETING_LINES vm_lineCopy = new VM_MEETING_LINES();
                vm_lineCopy.MEETING_LINES = new MEETING_LINES();
                vm_lineCopy.MEETING_LINES = lineCopy;

                return Json(vm_lineCopy, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Invalid Line", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public string PublishLine(VM_MEETING_LINES line)
        {
            if (line != null)
            {
                line.MEETING_LINES.MTL_STS = 5;
                db.Entry(line.MEETING_LINES).State = EntityState.Modified;
                db.SaveChanges();
                updateMasterStatus(line.MEETING_LINES.MTL_MT_REF);
                return "Line Published";
            }
            else
            {
                return "Invalid Line";
            }
        }


        [HttpPost]
        public string AddLine(VM_MEETING_LINES vm_meetings)
        {
            var mtl_executant = (vm_meetings.lst_MTL_EXECUTANT != null) ? String.Join(",", vm_meetings.lst_MTL_EXECUTANT.ToArray()) : string.Empty;
            var MTL_RELATED_FORM_REF = (vm_meetings.lst_MTL_RELATED_FORM_REF != null) ? String.Join(",", vm_meetings.lst_MTL_RELATED_FORM_REF.ToArray()) : string.Empty;

            MEETING_LINES line = vm_meetings.MEETING_LINES;
            line.MTL_EXECUTANT = mtl_executant;
            line.MTL_RELATED_FORM_REF = MTL_RELATED_FORM_REF;

            if (line != null)
            {
                line.MTL_DELETED = false;
                line.MTL_STS = 4;
                db.MEETING_LINES.Add(line);
                db.SaveChanges();

                //Move all files
                string[] filePaths = Directory.GetFiles(HttpContext.Server.MapPath("~/UploadsTemp/"));

                foreach (var filePath in filePaths)
                {
                    FileInfo fileInfo = new FileInfo(filePath);

                    if (fileInfo.Name.StartsWith(userName + "_"))
                    {
                        System.IO.File.Move(filePath, HttpContext.Server.MapPath("~/Uploads/") + fileInfo.Name);
                        MEETING_FILES meeting_files = new MEETING_FILES();
                        meeting_files.MTF_MT_REF = line.ID;
                        meeting_files.MTF_FILENAME = fileInfo.Name;
                        meeting_files.MTF_TYPE = 1;
                        meeting_files.MTF_CREATE_USERID = userName;

                        db.MEETING_FILES.Add(meeting_files);
                        db.SaveChanges();
                    }
                }

                return "Line Added";
            }
            else
            {
                return "Invalid Line";
            }
        }

        string userName = string.IsNullOrEmpty(System.Web.HttpContext.Current.User.Identity.Name) ? "empty" : System.Web.HttpContext.Current.User.Identity.Name;

        #endregion LINE

        #region LINEFILES
        //BEGIN Upload files for the Create page in Meeting Master
        [HttpPost]

        public JsonResult UploadLineFileCreate(HttpPostedFileBase aFile)
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

        public JsonResult RemoveLineFile(string fileName)
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

        public string UploadLineFileEdit(HttpPostedFileBase aFile, int? lineId)
        {
            if (aFile != null)
            {
                //TODO user session id should include
                var fileName = userName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + aFile.FileName;
                string path = HttpContext.Server.MapPath("~/Uploads/");
                aFile.SaveAs(path + fileName);

                MEETING_FILES meeting_files = new MEETING_FILES();
                meeting_files.MTF_MT_REF = lineId;
                meeting_files.MTF_FILENAME = fileName;
                meeting_files.MTF_TYPE = 1;
                meeting_files.MTF_CREATE_USERID = userName;

                db.MEETING_FILES.Add(meeting_files);
                db.SaveChanges();

                return "successfully uploaded";
            }
            return "could not upload";
        }

        [HttpGet]

        public JsonResult GetAllLineFiles(int? id)
        {
            var meeting_files = db.MEETING_FILES.Where(model => model.MTF_MT_REF == id).Select(model => new { model.MTF_FILENAME, model.ID }).ToList();
            return Json(meeting_files, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public string removeLineFileByID(int? fileId)
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

        #endregion LINEFILES

        #region MASTERHISTORY
        [HttpPost]

        public JsonResult getMasterHistoryById(int? id)
        {
            VM_MEETING_MASTER_V vm_meeting_master_v = new VM_MEETING_MASTER_V();
            IList<MEETING_MASTER_V> lst_meeting_master_v = db.MEETING_MASTER_V.Where(model => model.MTV_MT_REF == id).ToList();

            IList<NEW_MEETING_MASTER_V> lst_new_meeting_master_v = new List<NEW_MEETING_MASTER_V>();

            foreach (MEETING_MASTER_V item in lst_meeting_master_v)
            {
                NEW_MEETING_MASTER_V new_meeting_master_v = new NEW_MEETING_MASTER_V();

                new_meeting_master_v.lst_MT_USER_PARTICIPANTS = item.MT_USER_PARTICIPANTS != null ? item.MT_USER_PARTICIPANTS.Split(',').ToList() : null;
                new_meeting_master_v.lst_MT_USER_CC = (item.MT_USER_CC != null) ? item.MT_USER_CC.Split(',').ToList() : null;

                new_meeting_master_v.MEETING_MASTER_V = item;

                lst_new_meeting_master_v.Add(new_meeting_master_v);
            }


            vm_meeting_master_v.lst_NEW_MEETING_MASTER_V = lst_new_meeting_master_v;

            return Json(vm_meeting_master_v, JsonRequestBehavior.AllowGet);
        }

        #endregion MASTERHISTORY

        #region MASTER

        [HttpPost]

        public JsonResult getMasterById(int? id)
        {
            VM_MEETING vm_meetings = new VM_MEETING();
            if (id != null)
            {
                MEETING_MASTER meeting_master = db.MEETING_MASTER.Where(model => model.ID == id).FirstOrDefault();

                vm_meetings.MEETING_MASTER = meeting_master;
                vm_meetings.lst_MT_USER_PARTICIPANTS = meeting_master.MT_USER_PARTICIPANTS != null ? meeting_master.MT_USER_PARTICIPANTS.Split(',').ToList() : null;
                vm_meetings.lst_MT_USER_CC = (meeting_master.MT_USER_CC != null) ? meeting_master.MT_USER_CC.Split(',').ToList() : null;

            }
            return Json(vm_meetings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public string UpdateMaster(VM_MEETING vm_meetings)
        {
            MEETING_MASTER master = vm_meetings.MEETING_MASTER;

            var user_participant = (vm_meetings.lst_MT_USER_PARTICIPANTS != null) ? String.Join(",", vm_meetings.lst_MT_USER_PARTICIPANTS.ToArray()) : string.Empty;
            var user_cc = (vm_meetings.lst_MT_USER_CC != null) ? String.Join(",", vm_meetings.lst_MT_USER_CC.ToArray()) : string.Empty;

            master.MT_USER_PARTICIPANTS = user_participant;
            master.MT_USER_CC = user_cc;

            if (master != null)
            {
                var local = db.Set<MEETING_MASTER>().Local
                         .FirstOrDefault(f => f.ID == master.ID);
                if (local != null)
                {
                    db.Entry(local).State = EntityState.Detached;
                }
                db.Entry(master).State = EntityState.Modified;
                db.SaveChanges();
                return "Master Updated";
            }
            else
            {
                return "Invalid Master";
            }
        }

        #endregion MASTER

        #region MASTERFILES
        //BEGIN Upload files for the Create page in Meeting Master
        [HttpPost]

        public JsonResult Upload(HttpPostedFileBase aFile)
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

        public JsonResult RemoveFile(string fileName)
        {
            var fileLoc = HttpContext.Server.MapPath("~/UploadsTemp/") + fileName;
            if (System.IO.File.Exists(fileLoc))
            {
                System.IO.File.Delete(fileLoc);
            }

            IList<FileInfoName> lst_fileInfo = listFilesByUserName(userName);

            return Json(lst_fileInfo, JsonRequestBehavior.AllowGet);
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
        //END Upload files for the Create page in Meeting Master


        //BEGIN Upload files for the Edit page in Meeting Master

        [HttpPost]

        public string UploadFile(HttpPostedFileBase aFile, int? MT_REF)
        {
            if (aFile != null)
            {
                //TODO user session id should include
                var fileName = userName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + aFile.FileName;
                string path = HttpContext.Server.MapPath("~/Uploads/");
                aFile.SaveAs(path + fileName);

                MEETING_FILES meeting_files = new MEETING_FILES();
                meeting_files.MTF_MT_REF = MT_REF;
                meeting_files.MTF_FILENAME = fileName;
                meeting_files.MTF_TYPE = 0;
                meeting_files.MTF_CREATE_USERID = userName;

                db.MEETING_FILES.Add(meeting_files);
                db.SaveChanges();

                return "successfully uploaded";
            }
            return "could not upload";
        }

        [HttpGet]

        public JsonResult GetAllFiles(int? id)
        {
            var meeting_files = db.MEETING_FILES.Where(model => model.MTF_MT_REF == id).Select(model => new { model.MTF_FILENAME, model.ID }).ToList();
            return Json(meeting_files, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public string removeFileByID(int? fileId)
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
        #endregion MASTERFILES


        #endregion Angular

    }
}