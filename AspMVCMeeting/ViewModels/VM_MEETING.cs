using AspMVCMeeting.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspMVCMeeting.ViewModels
{
    public class VM_MEETING
    {
        public MEETING_MASTER MEETING_MASTER { get; set; }
        public MEETING_LINES MEETING_LINES { get; set; }
        public MEETING_PARTICIPANTS MEETING_PARTICIPANTS { get; set; }

        public IList<VM_MEETING_LINES> lst_MEETING_LINES { get; set; }
        
        public List<string> lst_MT_USER_PARTICIPANTS { get; set; }

        public List<string> lst_MT_USER_CC { get; set; }

        public List<string> lst_MTL_EXECUTANT { get; set; }

        public List<string> lst_MTL_RELATED_FORM_REF { get; set; }
    }


    public class VM_MEETING_LINES {
        public MEETING_LINES MEETING_LINES { get; set; }

        public string MLN_NAME { get; set; }

        public List<string> lst_MTL_EXECUTANT { get; set; }

        public List<string> lst_MTL_RELATED_FORM_REF { get; set; }
    }
}