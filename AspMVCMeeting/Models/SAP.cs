namespace AspMVCMeeting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SAP")]
    public partial class SAP
    {
        [StringLength(50)]
        public string LREF { get; set; }

        [Key]
        [StringLength(50)]
        public string PERCODE { get; set; }

        [StringLength(80)]
        public string PNAME { get; set; }

        [StringLength(80)]
        public string PSURNAME { get; set; }

        [StringLength(80)]
        public string PFATHERNAME { get; set; }

        [StringLength(50)]
        public string STATUS { get; set; }

        [StringLength(50)]
        public string POSITIIONCODE { get; set; }

        [StringLength(80)]
        public string POSITIONNAME { get; set; }

        [StringLength(50)]
        public string JOBCODE { get; set; }

        [StringLength(80)]
        public string JOBCODE_T { get; set; }

        [StringLength(50)]
        public string JOB_FAMILY { get; set; }

        [StringLength(80)]
        public string JOB_FAMILY_T { get; set; }

        [StringLength(50)]
        public string TASK_GROUP { get; set; }

        [StringLength(80)]
        public string TASK_GROUP_T { get; set; }

        [StringLength(50)]
        public string STATU { get; set; }

        [StringLength(80)]
        public string STATU_T { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BIRTHDATE { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BEGINWORK { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FINISHWORK { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GROUPINDATE { get; set; }

        public int? YEAR_SENIORITY_YEAR { get; set; }

        public int? YEAR_SENIORITY_MONTH { get; set; }

        public int? YEAR_SENIORITY_DAY { get; set; }

        public int? SEX { get; set; }

        [StringLength(50)]
        public string PHONE { get; set; }

        public int? PERTYP { get; set; }

        [StringLength(50)]
        public string ORG_UNIT { get; set; }

        [StringLength(80)]
        public string ORG_UNIT_T { get; set; }

        [StringLength(20)]
        public string RANKCODE { get; set; }

        [StringLength(100)]
        public string EMAIL { get; set; }

        [StringLength(100)]
        public string ACCOUNTNAME { get; set; }

        [StringLength(50)]
        public string SHEFCODE { get; set; }

        [StringLength(50)]
        public string HRCODE1 { get; set; }

        [StringLength(50)]
        public string HRCODE2 { get; set; }

        [StringLength(50)]
        public string HRCODE3 { get; set; }

        [StringLength(50)]
        public string ACCOUNTPERCODE { get; set; }

        [StringLength(50)]
        public string ACCOUNTATTENDANT { get; set; }

        [StringLength(50)]
        public string HRSHEFCODE { get; set; }

        [StringLength(50)]
        public string GROUPHEADCODE { get; set; }

        [StringLength(50)]
        public string MCODE { get; set; }

        [StringLength(50)]
        public string TCODE { get; set; }

        [StringLength(50)]
        public string ICODE { get; set; }

        [StringLength(50)]
        public string KSCODE1 { get; set; }

        [StringLength(50)]
        public string KSCODE2 { get; set; }

        [StringLength(50)]
        public string SCODE { get; set; }

        [StringLength(50)]
        public string RCODE { get; set; }

        [StringLength(50)]
        public string DCODE { get; set; }

        [StringLength(50)]
        public string AIHS { get; set; }

        [StringLength(50)]
        public string WAREHOUSER { get; set; }

        [StringLength(50)]
        public string INPURCHASER { get; set; }

        [StringLength(50)]
        public string KSACCOUNTER { get; set; }

        [StringLength(50)]
        public string IKRESPONSIBLE { get; set; }

        [StringLength(50)]
        public string AUDITBEARER { get; set; }

        [StringLength(50)]
        public string FOREIGNSERVICER { get; set; }

        public double? ABSENCE_QUATO { get; set; }

        [StringLength(1)]
        public string SHEFABSENCE { get; set; }

        [StringLength(50)]
        public string OLDCODE { get; set; }

        public DateTime? TRANSFERDATE { get; set; }

        [StringLength(10)]
        public string PCOMPANYCODE { get; set; }

        [StringLength(200)]
        public string PCOMPANYNAME { get; set; }

        [StringLength(10)]
        public string WORKGRCODE { get; set; }

        [StringLength(150)]
        public string WORKGRNAME { get; set; }

        [StringLength(10)]
        public string WORKSUBGRCODE { get; set; }

        [StringLength(150)]
        public string WORKSUBGRNAME { get; set; }

        [StringLength(10)]
        public string SALARYTYP { get; set; }
    }
}
