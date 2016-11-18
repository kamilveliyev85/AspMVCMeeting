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

        [Display(Name = "LBL_MTL_TYPE", ResourceType = typeof(App_GlobalResources.Global))]
        public int? MTL_TYPE { get; set; }

        [Display(Name = "LBL_MTL_START_DATE", ResourceType = typeof(App_GlobalResources.Global))]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? MTL_START_DATE { get; set; }
        
        [Display(Name = "LBL_MTL_FINISH_DATE", ResourceType = typeof(App_GlobalResources.Global))]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? MTL_FINISH_DATE { get; set; }

        [Display(Name = "LBL_MTL_RESPONSIBLE", ResourceType = typeof(App_GlobalResources.Global))]
        [StringLength(50)]
        public string MTL_RESPONSIBLE { get; set; }

        [StringLength(1000)]
        [Display(Name = "LBL_MT_TAGS", ResourceType = typeof(App_GlobalResources.Global))]
        public string MTL_TAGS { get; set; }

        [StringLength(1000)]
        [Display(Name = "LBL_MTL_DESCRIPTION", ResourceType = typeof(App_GlobalResources.Global))]
        public string MTL_DESCRIPTION { get; set; }

        [Display(Name = "LBL_MTL_PROJECT_CODE", ResourceType = typeof(App_GlobalResources.Global))]
        public int? MTL_PROJECT_CODE { get; set; }

        [StringLength(100)]
        [Display(Name = "LBL_MTL_DEPARTMENT", ResourceType = typeof(App_GlobalResources.Global))]
        public string MTL_DEPARTMENT { get; set; }

        [Display(Name = "LBL_MT_STS", ResourceType = typeof(App_GlobalResources.Global))]
        public int? MTL_STS { get; set; }

        [StringLength(50)]
        [Display(Name = "LBL_MTL_CONFIRMER", ResourceType = typeof(App_GlobalResources.Global))]
        public string MTL_CONFIRMER { get; set; }

        [Display(Name = "LBL_MTL_DECISION_TYPE", ResourceType = typeof(App_GlobalResources.Global))]
        public int? MTL_DECISION_TYPE { get; set; }

        [StringLength(50)]
        [Display(Name = "LBL_MTL_OFFER_USER", ResourceType = typeof(App_GlobalResources.Global))]
        public string MTL_OFFER_USER { get; set; }

        [Display(Name = "LBL_MTL_OFFER_OK", ResourceType = typeof(App_GlobalResources.Global))]
        public bool? MTL_OFFER_OK { get; set; }

        [StringLength(500)]
        [Display(Name = "LBL_MTL_RELATED_FORM_REF", ResourceType = typeof(App_GlobalResources.Global))]
        public string MTL_RELATED_FORM_REF { get; set; }

        [StringLength(500)]
        [Display(Name = "LBL_MTL_RELATED_FORM_DESCRIPTION", ResourceType = typeof(App_GlobalResources.Global))]
        public string MTL_RELATED_FORM_DESCRIPTION { get; set; }

        [StringLength(50)]
        [Display(Name = "LBL_MT_SCODE1", ResourceType = typeof(App_GlobalResources.Global))]
        public string MTL_SCODE1 { get; set; }

        [StringLength(50)]
        [Display(Name = "LBL_MT_SCODE2", ResourceType = typeof(App_GlobalResources.Global))]
        public string MTL_SCODE2 { get; set; }

        [StringLength(50)]
        [Display(Name = "LBL_MT_SCODE3", ResourceType = typeof(App_GlobalResources.Global))]
        public string MTL_SCODE3 { get; set; }

        [StringLength(50)]
        [Display(Name = "LBL_MT_SCODE4", ResourceType = typeof(App_GlobalResources.Global))]
        public string MTL_SCODE4 { get; set; }

        [StringLength(50)]
        [Display(Name = "LBL_MT_SCODE5", ResourceType = typeof(App_GlobalResources.Global))]
        public string MTL_SCODE5 { get; set; }

        [NotMapped]
        public DateTime? MTL_CREATEDATE { get; set; }

        [StringLength(100)]
        public string MTL_CREATE_USERID { get; set; }

        [NotMapped]
        public DateTime? MTL_UPDATEDATE { get; set; }

        public int? MTL_UPDATE_USERID { get; set; }

        [StringLength(4000)]
        [Display(Name = "LBL_MTL_EXECUTANT", ResourceType = typeof(App_GlobalResources.Global))]
        public string MTL_EXECUTANT { get; set; }

        public bool? MTL_DELETED { get; set; }

        [StringLength(100)]
        public string MTL_NO { get; set; }

    }
}
