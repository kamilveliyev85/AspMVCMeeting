//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AspMVCMeeting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class MEETING_LINES_DETAIL
    {
        public int ID { get; set; }
        public Nullable<int> MLD_MTL_REF { get; set; }
        [Display(Name = "LBL_MLD_DESCRIPTION", ResourceType = typeof(App_GlobalResources.Global))]
        public string MLD_DESCRIPTION { get; set; }
        public Nullable<System.DateTime> MLD_CREATEDATE { get; set; }
        public string MLD_CREATE_USER { get; set; }
        public Nullable<bool> MLD_DELETED { get; set; }
    }
}
