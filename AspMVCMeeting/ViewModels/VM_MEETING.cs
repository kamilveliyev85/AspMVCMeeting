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

        public IList<MEETING_LINES> lst_MEETING_LINES { get; set; }

        [StringLength(100)]
        public string MTP_USER_1 { get; set; }

        [StringLength(100)]
        public string MTP_USER_2 { get; set; }


    }
}