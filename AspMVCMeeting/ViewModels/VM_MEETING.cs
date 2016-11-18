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
        public MEETING_MASTER_V MEETING_MASTER_V { get; set; }
        public VM_MEETING_LINES VM_MEETING_LINES { get; set; }

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

        public IList<VM_MEETING_NOTIFICATIONS> lst_MEETING_NOTIFICATIONS { get; set; }
    }


    public class VM_MEETING_LINES
    {
        public MEETING_LINES MEETING_LINES { get; set; }

        public string MLN_NAME { get; set; }

        public string MTL_TYPE_DESC { get; set; }

        public string MTL_STS_TEXT { get; set; }

        public string MTL_ACTION_TEXT { get; set; }
        public int? MTL_ACTION_TYPE { get; set; }

        public string MTL_ACTION_DESC { get; set; }

        public List<string> lst_MTL_EXECUTANT { get; set; }

        [Display(Name = "LBL_MTL_RELATED_FORM_REF", ResourceType = typeof(App_GlobalResources.Global))]
        public List<string> lst_MTL_RELATED_FORM_REF { get; set; }

        public string MTL_RESPONSIBLE_DESC { get; set; }

        public string MTL_CONFIRMER_DESC { get; set; }
        public string MT_FOLLOWER_DESC { get; set; }
        public string MTL_OFFER_USER_DESC { get; set; }
        public string MTL_EXECUTANT_DESC { get; set; }
        public string MTL_DECISION_TYPE_DESC { get; set; }
    }

    public class VM_MEETING_MASTER
    {
        public MEETING_MASTER MEETING_MASTER { get; set; }
        public string MT_TYPE_TEXT { get; set; }
        public string MT_STS_TEXT { get; set; }
        public string MT_STS_TEXT_INFO { get; set; }
    }

    public class VM_MEETING_LINES_DETAIL
    {
        public MEETING_LINES_DETAIL MEETING_LINES_DETAIL { get; set; }
        public string MLD_CREATE_USER_DESC { get; set; }
        public int detailFileCount { get; set; }
    }

    public class VM_MEETING_NOTIFICATIONS
    {
        public MEETING_NOTIFICATIONS MEETING_NOTIFICATIONS { get; set; }

        public VM_MEETING_LINES_DETAIL VM_MEETING_LINES_DETAIL { get; set; }
        public string MTL_DESCRIPTION { get; set; }
        public string MTL_TYPE_DESC { get; set; }

        public int? detailFileCount { get; set; }
    }

}