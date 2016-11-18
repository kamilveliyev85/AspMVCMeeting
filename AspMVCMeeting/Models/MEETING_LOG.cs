namespace AspMVCMeeting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MEETING_LOG
    {
        public int ID { get; set; }

        public int? MLS_MLG_REF { get; set; }

        [NotMapped]
        public DateTime? MLS_CREATE_DATE { get; set; }

        [StringLength(100)]
        public string MLS_CREATE_USERID { get; set; }

        [StringLength(50)]
        public string MLS_IP { get; set; }

        [StringLength(500)]
        public string MLS_DESCRIPTION { get; set; }

        [StringLength(1500)]
        public string MLS_BROWSER { get; set; }

        public int? MLS_LOG_TYPE { get; set; }
    }
}
