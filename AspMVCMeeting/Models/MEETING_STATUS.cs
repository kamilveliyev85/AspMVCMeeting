namespace AspMVCMeeting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MEETING_STATUS
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string MST_NAME { get; set; }

        public int? MST_TYPE { get; set; }

        public bool? MST_ACTIVE { get; set; }
    }
}
