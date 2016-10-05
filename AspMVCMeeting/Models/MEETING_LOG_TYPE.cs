namespace AspMVCMeeting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MEETING_LOG_TYPE
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string MLG_NAME { get; set; }

        public bool? MLG_ACTIVE { get; set; }
    }
}
