﻿using AspMVCMeeting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspMVCMeeting.ViewModels
{
    public class VM_PROJECTS
    {
        public IList<MEETING_PROJECTS> lst_MEETING_PROJECTS { get; set; }

        public MEETING_PROJECTS MEETING_PROJECTS { get; set; }
    }
}