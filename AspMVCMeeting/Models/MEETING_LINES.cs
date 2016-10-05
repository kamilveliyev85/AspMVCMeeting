namespace AspMVCMeeting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MEETING_LINES
    {
        public int ID { get; set; }

        public int? MTL_MT_REF { get; set; }

        public int? MT_LINENR { get; set; }

        public int? MTL_TYPE { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MTL_START_DATE { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MTL_FINISH_DATE { get; set; }

        [StringLength(50)]
        public string MTL_RESPONSIBLE { get; set; }

        [StringLength(1000)]
        public string MTL_TAGS { get; set; }

        [StringLength(1000)]
        public string MTL_DESCRIPTION { get; set; }

        public int? MTL_PROJECT_CODE { get; set; }

        [StringLength(100)]
        public string MTL_DEPARTMENT { get; set; }

        public int? MTL_STS { get; set; }

        [StringLength(50)]
        public string MTL_CONFIRMER { get; set; }

        public int? MTL_DECISION_TYPE { get; set; }

        [StringLength(50)]
        public string MTL_OFFER_USER { get; set; }

        public bool? MTL_OFFER_OK { get; set; }

        [StringLength(500)]
        public string MTL_RELATED_FORM_REF { get; set; }

        [StringLength(500)]
        public string MTL_RELATED_FORM_DESCRIPTION { get; set; }

        [StringLength(50)]
        public string MTL_SCODE1 { get; set; }

        [StringLength(50)]
        public string MTL_SCODE2 { get; set; }

        [StringLength(50)]
        public string MTL_SCODE3 { get; set; }

        [StringLength(50)]
        public string MTL_SCODE4 { get; set; }

        [StringLength(50)]
        public string MTL_SCODE5 { get; set; }

        public DateTime? MTL_CREATEDATE { get; set; }

        [StringLength(100)]
        public string MTL_CREATE_USERID { get; set; }

        public DateTime? MTL_UPDATEDATE { get; set; }

        public int? MTL_UPDATE_USERID { get; set; }
    }
}
