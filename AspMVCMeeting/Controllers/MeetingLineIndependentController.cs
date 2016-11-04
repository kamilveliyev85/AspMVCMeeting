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
using System.Web;
using System.Web.Mvc;

namespace AspMVCMeeting.Controllers
{
    [Authorize]
    public class MeetingLineIndependentController : BaseController
    {
        MeetingDataModelCodeFirst db = new MeetingDataModelCodeFirst();

        // Create: MeetingLineIndependent
        [HttpGet]
        public ActionResult Create()
        {
            VM_MEETING vm_meeting = new VM_MEETING();
            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).Select(model => new { MTL_TYPE = model.ID, MLN_NAME = model.MLN_NAME }).ToList(), "MTL_TYPE", "MLN_NAME");
            var managerList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 4 ORDER BY PNAME, PSURNAME"))
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


        [HttpPost]
        public ActionResult Create(VM_MEETING vm_meeting)
        {
            ViewBag.MEETING_LINE_TYPES = new SelectList(db.MEETING_LINE_TYPE.Where(type => type.MLN_ACTIVE == true).Select(model => new { MTL_TYPE = model.ID, MLN_NAME = model.MLN_NAME }).ToList(), "MTL_TYPE", "MLN_NAME");
            var managerList = db.Database
                .SqlQuery<SAP>("SAPHR_PERCODE @where", new SqlParameter("@where", "CAST(RANKCODE AS FLOAT) >= 4 ORDER BY PNAME, PSURNAME"))
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
            join mlt in db.MEETING_LINE_TYPE on mtl.MTL_TYPE equals mlt.ID
            join mst in db.MEETING_STATUS on mtl.MTL_STS equals mst.ID
            where mtl.MTL_MT_REF == id && mtl.MTL_DELETED == false
            && (mtl.MTL_CREATE_USERID == userName || mtl.MTL_CONFIRMER == userName)
            orderby mtl.ID descending
            select new VM_MEETING_LINES { MEETING_LINES = mtl, MLN_NAME = mlt.MLN_NAME, MTL_STS_TEXT = mst.MST_NAME }).ToList();

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
        public string PublishSelected(string items)
        {
            Dictionary<int, bool> values = JsonConvert.DeserializeObject<Dictionary<int, bool>>(items);

            if (values != null)
            {
                foreach (KeyValuePair<int, bool> item in values)
                {
                    if (item.Value)
                    {
                        var line = db.MEETING_LINES.Find(item.Key);
                        line.MTL_STS = 5;
                        db.Entry(line).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return "Line Changed";
            }
            else
            {
                return "Invalid Line";
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
                return "Line Published";
            }
            else
            {
                return "Invalid Line";
            }
        }


        [HttpPost]

        public string AddLine(VM_MEETING_LINES line)
        {
            if (line != null)
            {
                line.MEETING_LINES.MTL_DELETED = false;
                line.MEETING_LINES.MTL_STS = 4;
                db.MEETING_LINES.Add(line.MEETING_LINES);
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
                        meeting_files.MTF_MT_REF = line.MEETING_LINES.ID;
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

        #endregion Angular
    }
}