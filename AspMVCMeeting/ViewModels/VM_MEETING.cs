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
        public MEETING_LINES_DETAIL MEETING_LINES_DETAIL { get; set; }
        public MEETING_PARTICIPANTS MEETING_PARTICIPANTS { get; set; }

        public IList<VM_MEETING_MASTER> lst_MEETING_MASTER { get; set; }

        public IList<VM_MEETING_LINES> lst_MEETING_LINES { get; set; }

        public IList<VM_MEETING_LINES_DETAIL> lst_MEETING_LINES_DETAIL { get; set; }

        public List<string> lst_MT_USER_PARTICIPANTS { get; set; }

        public List<string> lst_MT_USER_CC { get; set; }

        public List<string> lst_MTL_EXECUTANT { get; set; }

        public List<string> lst_MTL_RELATED_FORM_REF { get; set; }
    }


    public class VM_MEETING_LINES
    {
        public MEETING_LINES MEETING_LINES { get; set; }

        public string MLN_NAME { get; set; }

        public string MTL_STS_TEXT { get; set; }

        public List<string> lst_MTL_EXECUTANT { get; set; }

        [Display(Name = "LBL_MTL_RELATED_FORM_REF", ResourceType = typeof(App_GlobalResources.Global))]
        public List<string> lst_MTL_RELATED_FORM_REF { get; set; }
    }

    public class VM_MEETING_MASTER
    {
        public MEETING_MASTER MEETING_MASTER { get; set; }
    }

    public class VM_MEETING_LINES_DETAIL
    {
        public MEETING_LINES_DETAIL MEETING_LINES_DETAIL { get; set; }
    }
}