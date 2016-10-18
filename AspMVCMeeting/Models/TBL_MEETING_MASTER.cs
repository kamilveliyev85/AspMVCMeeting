using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AspMVCMeeting.Models
{
    public class TBL_MEETING_MASTER
    {
            public int ID { get; set; }
            public Nullable<int> MT_TYPE { get; set; }
            public string MT_TITLE { get; set; }
            public Nullable<System.DateTime> MT_DATE { get; set; }
            public string MT_MANAGER { get; set; }
            public string MT_DESCIPTION { get; set; }
            public string MT_NO { get; set; }
            public string MT_TAGS { get; set; }
            public string MT_PLACE { get; set; }
            public Nullable<System.TimeSpan> MT_START_TIME { get; set; }
            public Nullable<System.TimeSpan> MT_FINISH_TIME { get; set; }
            public string MT_SCODE1 { get; set; }
            public string MT_SCODE2 { get; set; }
            public string MT_SCODE3 { get; set; }
            public string MT_SCODE4 { get; set; }
            public string MT_SCODE5 { get; set; }
            [NotMapped]
            public Nullable<System.DateTime> MT_CREATEDATE { get; set; }
            public string MT_CREATE_USERID { get; set; }
            public string MT_FOLLOWER_USERID { get; set; }
            public Nullable<System.DateTime> MT_UPDATEDATE { get; set; }
            public string MT_UPDATE_USERID { get; set; }
            public Nullable<int> MT_STS { get; set; }
        }
    }
    