namespace AspMVCMeeting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MEETING_FILE_CATEGORY
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string MFC_NAME { get; set; }

        public bool? MFC_ACTIVE { get; set; }
    }
}
