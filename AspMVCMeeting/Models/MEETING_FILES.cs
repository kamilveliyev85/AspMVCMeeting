namespace AspMVCMeeting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MEETING_FILES
    {
        public int ID { get; set; }

        public int? MTF_MT_REF { get; set; }

        public int? MTF_TYPE { get; set; }

        public int? MTF_CATEGORY { get; set; }

        [StringLength(100)]
        public string MTF_FILENAME { get; set; }

        public DateTime? MTF_CREATEDATE { get; set; }

        [StringLength(100)]
        public string MTF_CREATE_USERID { get; set; }

        [StringLength(1000)]
        public string MTF_DESCRIPTION { get; set; }
    }
}
