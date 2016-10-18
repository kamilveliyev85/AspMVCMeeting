using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AspMVCMeeting.Models
{
    [Table("DEPT")]
    public partial class DEPT
    {
        [Key]
        public int FIRMNR { get; set; }

        
        [StringLength(150)]
        public string FIRMNAME { get; set; }
    }
}