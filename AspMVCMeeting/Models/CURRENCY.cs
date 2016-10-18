namespace AspMVCMeeting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CURRENCY")]
    public partial class CURRENCY
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(10)]
        public string prb_kod { get; set; }

        [StringLength(50)]
        public string prb_tanim { get; set; }

        public bool? prb_ana_para { get; set; }

        public int? prb_sira { get; set; }

        public bool? prb_aktif { get; set; }

        [StringLength(5)]
        public string prb_SAP_para_birimi { get; set; }

        [StringLength(5)]
        public string prb_unity_para_birimi { get; set; }
    }
}
