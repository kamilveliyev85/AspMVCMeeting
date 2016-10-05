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

        public int? MT_TYPE { get; set; }

        [StringLength(400)]
        public string MT_TITLE { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MT_DATE { get; set; }

        [StringLength(100)]
        public string MT_MANAGER { get; set; }

        [StringLength(1000)]
        public string MT_DESCIPTION { get; set; }

        [StringLength(50)]
        public string MT_NO { get; set; }

        [StringLength(1000)]
        public string MT_TAGS { get; set; }

        [StringLength(200)]
        public string MT_PLACE { get; set; }

        public TimeSpan? MT_START_TIME { get; set; }

        public TimeSpan? MT_FINISH_TIME { get; set; }

        [StringLength(50)]
        public string MT_SCODE1 { get; set; }

        [StringLength(50)]
        public string MT_SCODE2 { get; set; }

        [StringLength(50)]
        public string MT_SCODE3 { get; set; }

        [StringLength(50)]
        public string MT_SCODE4 { get; set; }

        [StringLength(50)]
        public string MT_SCODE5 { get; set; }

        [NotMapped]
        public DateTime? MT_CREATEDATE { get; set; }

        [StringLength(100)]
        public string MT_CREATE_USERID { get; set; }

        [StringLength(100)]
        public string MT_FOLLOWER_USERID { get; set; }

        public DateTime? MT_UPDATEDATE { get; set; }

        [StringLength(100)]
        public string MT_UPDATE_USERID { get; set; }

        public int? MT_STS { get; set; }
    }
}
