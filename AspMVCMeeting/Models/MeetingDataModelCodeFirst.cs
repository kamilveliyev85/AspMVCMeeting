namespace AspMVCMeeting.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MeetingDataModelCodeFirst : DbContext
    {
        public MeetingDataModelCodeFirst()
            : base("name=MeetingDataModelCodeFirst")
        {
        }

        public virtual DbSet<CURRENCY> CURRENCies { get; set; }
        public virtual DbSet<MEETING_FILE_CATEGORY> MEETING_FILE_CATEGORY { get; set; }
        public virtual DbSet<MEETING_FILES> MEETING_FILES { get; set; }
        public virtual DbSet<MEETING_LINES> MEETING_LINES { get; set; }
        public virtual DbSet<MEETING_LOG> MEETING_LOG { get; set; }
        public virtual DbSet<MEETING_LOG_TYPE> MEETING_LOG_TYPE { get; set; }
        public virtual DbSet<MEETING_MASTER> MEETING_MASTER { get; set; }
        public virtual DbSet<MEETING_MASTER_V> MEETING_MASTER_V { get; set; }
        public virtual DbSet<MEETING_PARTICIPANTS> MEETING_PARTICIPANTS { get; set; }
        public virtual DbSet<MEETING_PROJECTS> MEETING_PROJECTS { get; set; }
        public virtual DbSet<MEETING_STATUS> MEETING_STATUS { get; set; }
        public virtual DbSet<MEETING_TYPE> MEETING_TYPE { get; set; }
        public virtual DbSet<MEETING_TYPE_PERMISSION> MEETING_TYPE_PERMISSION { get; set; }
        public virtual DbSet<MEETING_LINE_TYPE> MEETING_LINE_TYPE { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<SAP> SAP { get; set; }
        public virtual DbSet<DEPT> DEPT { get; set; }
        public virtual DbSet<DECISION_TYPES> DECISION_TYPES { get; set; }
        public virtual DbSet<VW_MEETING_LINE> VW_MEETING_LINE { get; set; }
        public virtual DbSet<MEETING_PLACE> MEETING_PLACE { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CURRENCY>()
                .Property(e => e.prb_kod)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CURRENCY>()
                .Property(e => e.prb_tanim)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.LREF)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.PERCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.STATUS)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.POSITIIONCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.JOBCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.JOB_FAMILY)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.TASK_GROUP)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.STATU)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.PHONE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.ORG_UNIT)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.RANKCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.ACCOUNTNAME)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.SHEFCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.HRCODE1)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.HRCODE2)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.HRCODE3)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.ACCOUNTPERCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.ACCOUNTATTENDANT)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.HRSHEFCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.GROUPHEADCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.MCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.TCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.ICODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.KSCODE1)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.KSCODE2)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.SCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.RCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.DCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.AIHS)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.WAREHOUSER)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.INPURCHASER)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.KSACCOUNTER)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.IKRESPONSIBLE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.AUDITBEARER)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.FOREIGNSERVICER)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.SHEFABSENCE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.OLDCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.PCOMPANYCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.WORKGRCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.WORKGRNAME)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.WORKSUBGRCODE)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.WORKSUBGRNAME)
                .IsUnicode(false);

            modelBuilder.Entity<SAP>()
                .Property(e => e.SALARYTYP)
                .IsUnicode(false);
        }

        public System.Data.Entity.DbSet<AspMVCMeeting.Models.MEETING_LINES_DETAIL> MEETING_LINES_DETAIL { get; set; }
    }
}
