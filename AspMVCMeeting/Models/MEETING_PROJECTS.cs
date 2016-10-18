namespace AspMVCMeeting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MEETING_PROJECTS
    {
        public int ID { get; set; }

        [StringLength(50)]
        [Display(Name = "LBL_PRJ_CODE", ResourceType = typeof(App_GlobalResources.Global))]
        public string PRJ_CODE { get; set; }

        [StringLength(100)]
        [Display(Name = "LBL_PRJ_NAME", ResourceType = typeof(App_GlobalResources.Global))]
        public string PRJ_NAME { get; set; }
        
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "LBL_MTL_START_DATE", ResourceType = typeof(App_GlobalResources.Global))]
        public DateTime? PRJ_START_DATE { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "LBL_MTL_FINISH_DATE", ResourceType = typeof(App_GlobalResources.Global))]
        public DateTime? PRJ_FINISH_DATE { get; set; }

        [StringLength(100)]
        [Display(Name = "LBL_MTL_RESPONSIBLE", ResourceType = typeof(App_GlobalResources.Global))]
        public string PRJ_RESPONSIBLE { get; set; }

        [StringLength(4000)]
        [Display(Name = "LBL_PRJ_MEMBERS", ResourceType = typeof(App_GlobalResources.Global))]
        public string PRJ_MEMBERS { get; set; }

        [Display(Name = "LBL_PRJ_BUDGET", ResourceType = typeof(App_GlobalResources.Global))]
        public double? PRJ_BUDGET { get; set; }

        [Display(Name = "LBL_PRJ_BUDGET_CURRENCY", ResourceType = typeof(App_GlobalResources.Global))]
        public int? PRJ_BUDGET_CURRENCY { get; set; }

        [Display(Name = "LBL_PRJ_MAN_DAY", ResourceType = typeof(App_GlobalResources.Global))]
        public int? PRJ_MAN_DAY { get; set; }

        [StringLength(300)]
        [Display(Name = "LBL_MTL_DEPARTMENT", ResourceType = typeof(App_GlobalResources.Global))]
        public string PRJ_DEPARTMENT { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PRJ_CREATEDATE { get; set; }

        [StringLength(100)]
        public string PRJ_CREATE_USERID { get; set; }

        public DateTime? PRJ_UPDATEDATE { get; set; }

        [StringLength(100)]
        public string PRJ_UPDATE_USERID { get; set; }

        [Display(Name = "LBL_PRJ_STS", ResourceType = typeof(App_GlobalResources.Global))]
        public bool? PRJ_STS { get; set; }
    }
}
