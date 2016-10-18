using AspMVCMeeting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspMVCMeeting.ViewModels
{
    public class VM_MEETING_MASTER_V
    {
        public NEW_MEETING_MASTER_V NEW_MEETING_MASTER_V { get; set; }

        public IList<NEW_MEETING_MASTER_V> lst_NEW_MEETING_MASTER_V { get; set; }

    }
}

public class NEW_MEETING_MASTER_V
{
    public MEETING_MASTER_V MEETING_MASTER_V { get; set; }

    public List<string> lst_MT_USER_PARTICIPANTS { get; set; }

    public List<string> lst_MT_USER_CC { get; set; }
}
