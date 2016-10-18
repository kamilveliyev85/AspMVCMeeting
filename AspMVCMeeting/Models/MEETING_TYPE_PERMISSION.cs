namespace AspMVCMeeting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MEETING_TYPE_PERMISSION
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string MPR_USER { get; set; }

        public int? MPR_MT_TYPE { get; set; }

        public bool? MPR_ACTIVE { get; set; }
    }
}
