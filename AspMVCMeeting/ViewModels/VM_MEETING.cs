using AspMVCMeeting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspMVCMeeting.ViewModels
{
    public class VM_MEETING
    {
        public MEETING_MASTER MEETING_MASTER { get; set; }
        public MEETING_LINES MEETING_LINES { get; set; }


    }
}