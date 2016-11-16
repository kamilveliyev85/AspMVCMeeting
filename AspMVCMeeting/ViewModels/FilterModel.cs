using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspMVCMeeting.Models
{
    public class FilterModel
    {
        [Required]
        public string id { get; set; }
        public string title { get; set; }
    }
}