using AngularMVCFileUpload.Models;
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
    public class OperationController : Controller
    {
        MeetingDataModelCodeFirst db = new MeetingDataModelCodeFirst();
        string userName = string.IsNullOrEmpty(System.Web.HttpContext.Current.User.Identity.Name) ? "empty" : System.Web.HttpContext.Current.User.Identity.Name;

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

            ViewBag.STATUS = new SelectList(db.MEETING_STATUS.Where(model => model.MST_ACTIVE == true).Where(model => model.MST_TYPE == 2 && model.MST_PAGE == "Operation").Select(model => new { model.ID, model.MST_NAME }).ToList(), "ID", "MST_NAME");

            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).ToList(), "ID", "MLN_NAME");
            ViewBag.MTL_PROJECT_CODE = new SelectList(db.MEETING_PROJECTS.Select(model => new { model.ID, PRJ_NAME = model.PRJ_CODE + " " + model.PRJ_NAME }).ToList(), "ID", "PRJ_NAME");
            ViewBag.DEPT = new SelectList(db.DEPT.OrderBy(model => model.FIRMNAME).ToList(), "FIRMNR", "FIRMNAME");
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

            ViewBag.STATUS = new SelectList(db.MEETING_STATUS.Where(model => model.MST_ACTIVE == true).Where(model => model.MST_TYPE == 2 && model.MST_PAGE == "Approve").Select(model => new { model.ID, model.MST_NAME }).ToList(), "ID", "MST_NAME");

            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).ToList(), "ID", "MLN_NAME");
            ViewBag.MTL_PROJECT_CODE = new SelectList(db.MEETING_PROJECTS.Select(model => new { model.ID, PRJ_NAME = model.PRJ_CODE + " " + model.PRJ_NAME }).ToList(), "ID", "PRJ_NAME");
            ViewBag.DEPT = new SelectList(db.DEPT.OrderBy(model => model.FIRMNAME).ToList(), "FIRMNR", "FIRMNAME");
            ViewBag.DECISION_TYPES = new SelectList(db.DECISION_TYPES.Where(model => model.DCN_ACTIVE == true).Select(model => new { model.ID, model.DCN_NAME }).ToList(), "ID", "DCN_NAME");
            ViewBag.MTL_RELATED_FORM_REF = new SelectList(db.VW_MEETING_LINE.Select(model => new { model.ID, DESCRIPTION = model.MT_TITLE + "/" + model.MTL_DESCRIPTION }).ToList(), "ID", "DESCRIPTION");

            return View();
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

        #region LINE
        [HttpGet]
        public JsonResult GetLinesAll(int? id)
        {
            VM_MEETING vm_meetings = new VM_MEETING();

            var managerList = db.Database
            .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "SHEFCODE =(SELECT LREF FROM  PERINFO WHERE ACCOUNTNAME ='" + userName + "') OR ACCOUNTNAME ='" + userName + "'"))
            .Select(type => new { ID = type.ACCOUNTNAME, FULLNAME = type.PNAME + " " + type.PSURNAME })
            .OrderBy(model=> model.FULLNAME)
            .ToList();

            var result = String.Join(", ", managerList.Select(model => model.ID).ToArray());

            //ViewBag.MT_MANAGER = new SelectList(managerList, "ID", "FULLNAME");
            //managerList.Select(model => model.ID).ToList().Contains(mtl.MTL_RESPONSIBLE)
            vm_meetings.lst_MEETING_LINES =
          (from mtl in db.MEETING_LINES.Where(model => result.Contains(model.MTL_RESPONSIBLE))
           join mlt in db.MEETING_LINE_TYPE on mtl.MTL_TYPE equals mlt.ID
           join mst in db.MEETING_STATUS on mtl.MTL_STS equals mst.ID
           join sap in db.SAP on mtl.MTL_RESPONSIBLE equals sap.ACCOUNTNAME
           join sap1 in db.SAP on mtl.MTL_CONFIRMER equals sap1.ACCOUNTNAME
           where mtl.MTL_DELETED == false && (mtl.MTL_STS == 5 || mtl.MTL_STS == 11)
           
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
          (from mtl in db.MEETING_LINES
           join mlt in db.MEETING_LINE_TYPE on mtl.MTL_TYPE equals mlt.ID
           join mst in db.MEETING_STATUS on mtl.MTL_STS equals mst.ID
           where mtl.MTL_DELETED == false && mtl.MTL_STS == 6 && mtl.MTL_CONFIRMER == userName
           select new VM_MEETING_LINES { MEETING_LINES = mtl, MLN_NAME = mlt.MLN_NAME, MTL_STS_TEXT = mst.MST_NAME }).ToList();

            return Json(vm_meetings.lst_MEETING_LINES, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public string UpdateStatus(VM_MEETING_LINES vm_meetings)
        {
            MEETING_LINES line = vm_meetings.MEETING_LINES;

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

        #endregion LINE

        #region DETAIL
        [HttpGet]

        public JsonResult GetDetailAll(int? id)
        {
            VM_MEETING vm_meetings = new VM_MEETING();

            vm_meetings.lst_MEETING_LINES_DETAIL =
           (from mtd in db.MEETING_LINES_DETAIL
            where mtd.MLD_MTL_REF == id && mtd.MLD_DELETED == false
            select new VM_MEETING_LINES_DETAIL { MEETING_LINES_DETAIL = mtd }).ToList();

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
                detail.MLD_DELETED = true;
                db.Entry(detail).State = EntityState.Modified;
                db.SaveChanges();
                return "Line Deleted";
            }
            else
            {
                return "Invalid Line";
            }
        }

        [HttpPost]

        public string AddDetail(VM_MEETING_LINES_DETAIL vm_detail)
        {
            MEETING_LINES_DETAIL detail = vm_detail.MEETING_LINES_DETAIL;

            if (detail != null)
            {
                detail.MLD_DELETED = false;
                db.MEETING_LINES_DETAIL.Add(detail);
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
                        meeting_files.MTF_MT_REF = detail.ID;
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
                var fileName = userName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + aFile.FileName;
                string path = HttpContext.Server.MapPath("~/Uploads/");
                aFile.SaveAs(path + fileName);

                MEETING_FILES meeting_files = new MEETING_FILES();
                meeting_files.MTF_MT_REF = detailId;
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

        public JsonResult GetAllDetailFiles(int? id)
        {
            var meeting_files = db.MEETING_FILES.Where(model => model.MTF_MT_REF == id).Select(model => new { model.MTF_FILENAME, model.ID }).ToList();
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

        #endregion Angular

    }
}