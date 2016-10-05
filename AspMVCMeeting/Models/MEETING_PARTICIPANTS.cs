namespace AspMVCMeeting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MEETING_PARTICIPANTS
    {
        public int ID { get; set; }

        public int? MTP_REF { get; set; }

        [StringLength(100)]
        public string MTP_USER { get; set; }

        public int? MTP_TYPE { get; set; }

        public DateTime? MTP_CREATEDATE { get; set; }

        [StringLength(100)]
        public string MTP_CREATE_USERID { get; set; }
    }
}
