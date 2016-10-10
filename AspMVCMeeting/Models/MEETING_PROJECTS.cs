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
        public string PRJ_CODE { get; set; }

        [StringLength(100)]
        public string PRJ_NAME { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Column(TypeName = "date")]
        public DateTime? PRJ_START_DATE { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Column(TypeName = "date")]
        public DateTime? PRJ_FINISH_DATE { get; set; }

        [StringLength(100)]
        public string PRJ_RESPONSIBLE { get; set; }

        [StringLength(4000)]
        public string PRJ_MEMBERS { get; set; }

        public double? PRJ_BUDGET { get; set; }

        public int? PRJ_BUDGET_CURRENCY { get; set; }

        public int? PRJ_MAN_DAY { get; set; }

        [StringLength(300)]
        public string PRJ_DEPARTMENT { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? PRJ_CREATEDATE { get; set; }

        [StringLength(100)]
        public string PRJ_CREATE_USERID { get; set; }

        public DateTime? PRJ_UPDATEDATE { get; set; }

        [StringLength(100)]
        public string PRJ_UPDATE_USERID { get; set; }

        public bool? PRJ_STS { get; set; }
    }
}
