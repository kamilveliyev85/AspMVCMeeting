namespace AspMVCMeeting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MEETING_MASTER
    {
        public int ID { get; set; }
        [Display(Name = "LBL_MT_TYPE", ResourceType = typeof(App_GlobalResources.Global))]
        public int? MT_TYPE { get; set; }

        [StringLength(400)]
        [Display(Name = "LBL_MT_TITLE", ResourceType = typeof(App_GlobalResources.Global))]
        public string MT_TITLE { get; set; }

        [Display(Name = "LBL_MT_DATE", ResourceType = typeof(App_GlobalResources.Global))]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? MT_DATE { get; set; }

        [StringLength(100)]
        [Display(Name = "LBL_MT_MANAGER", ResourceType = typeof(App_GlobalResources.Global))]
        public string MT_MANAGER { get; set; }

        [Display(Name = "LBL_MT_DESCIPTION", ResourceType = typeof(App_GlobalResources.Global))]
        [StringLength(1000)]
        public string MT_DESCIPTION { get; set; }

        [Display(Name = "LBL_MT_NO", ResourceType = typeof(App_GlobalResources.Global))]
        [StringLength(50)]
        public string MT_NO { get; set; }

        [StringLength(1000)]
        [Display(Name = "LBL_MT_TAGS", ResourceType = typeof(App_GlobalResources.Global))]
        public string MT_TAGS { get; set; }

        [Display(Name = "LBL_MT_PLACE", ResourceType = typeof(App_GlobalResources.Global))]
        public int? MT_PLACE { get; set; }

        [Display(Name = "LBL_MT_START_TIME", ResourceType = typeof(App_GlobalResources.Global))]
        public TimeSpan? MT_START_TIME { get; set; }

        [Display(Name = "LBL_MT_FINISH_TIME", ResourceType = typeof(App_GlobalResources.Global))]
        public TimeSpan? MT_FINISH_TIME { get; set; }

        [StringLength(50)]
        [Display(Name = "LBL_MT_SCODE1", ResourceType = typeof(App_GlobalResources.Global))]
        public string MT_SCODE1 { get; set; }

        [StringLength(50)]
        [Display(Name = "LBL_MT_SCODE2", ResourceType = typeof(App_GlobalResources.Global))]
        public string MT_SCODE2 { get; set; }

        [StringLength(50)]
        [Display(Name = "LBL_MT_SCODE3", ResourceType = typeof(App_GlobalResources.Global))]
        public string MT_SCODE3 { get; set; }

        [StringLength(50)]
        [Display(Name = "LBL_MT_SCODE4", ResourceType = typeof(App_GlobalResources.Global))]
        public string MT_SCODE4 { get; set; }

        [StringLength(50)]
        [Display(Name = "LBL_MT_SCODE5", ResourceType = typeof(App_GlobalResources.Global))]
        public string MT_SCODE5 { get; set; }

        [NotMapped]
        public DateTime? MT_CREATEDATE { get; set; }

        [StringLength(100)]
        public string MT_CREATE_USERID { get; set; }

        [StringLength(100)]
        [Display(Name = "LBL_MT_FOLLOWER_USERID", ResourceType = typeof(App_GlobalResources.Global))]
        public string MT_FOLLOWER_USERID { get; set; }

        public DateTime? MT_UPDATEDATE { get; set; }

        [StringLength(100)]
        public string MT_UPDATE_USERID { get; set; }

        [Display(Name = "LBL_MT_STS", ResourceType = typeof(App_GlobalResources.Global))]
        public int? MT_STS { get; set; }

        [StringLength(4000)]
        [Display(Name = "LBL_MT_USER_PARTICIPANTS", ResourceType = typeof(App_GlobalResources.Global))]
        public string MT_USER_PARTICIPANTS { get; set; }

        [StringLength(4000)]
        [Display(Name = "LBL_MT_USER_OUT", ResourceType = typeof(App_GlobalResources.Global))]
        public string MT_USER_OUT { get; set; }

        [StringLength(4000)]
        [Display(Name = "LBL_MT_USER_CC", ResourceType = typeof(App_GlobalResources.Global))]
        public string MT_USER_CC { get; set; }
        
        public bool? MT_DELETED { get; set; }

        [StringLength(100)]
        [Display(Name = "LBL_MTL_DEPARTMENT", ResourceType = typeof(App_GlobalResources.Global))]
        public string MT_DEPARTMENT { get; set; }

    }
}
