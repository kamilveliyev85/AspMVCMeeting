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
    
    public partial class MEETING_LINES
    {
        public int ID { get; set; }
        public Nullable<int> MTL_MT_REF { get; set; }
        public Nullable<int> MT_LINENR { get; set; }
        public Nullable<int> MTL_TYPE { get; set; }
        public Nullable<System.DateTime> MTL_START_DATE { get; set; }
        public Nullable<System.DateTime> MTL_FINISH_DATE { get; set; }
        public string MTL_RESPONSIBLE { get; set; }
        public string MTL_TAGS { get; set; }
        public string MTL_DESCRIPTION { get; set; }
        public Nullable<int> MTL_PROJECT_CODE { get; set; }
        public string MTL_DEPARTMENT { get; set; }
        public Nullable<int> MTL_STS { get; set; }
        public string MTL_CONFIRMER { get; set; }
        public Nullable<int> MTL_DECISION_TYPE { get; set; }
        public string MTL_OFFER_USER { get; set; }
        public Nullable<bool> MTL_OFFER_OK { get; set; }
        public string MTL_RELATED_FORM_REF { get; set; }
        public string MTL_RELATED_FORM_DESCRIPTION { get; set; }
        public string MTL_SCODE1 { get; set; }
        public string MTL_SCODE2 { get; set; }
        public string MTL_SCODE3 { get; set; }
        public string MTL_SCODE4 { get; set; }
        public string MTL_SCODE5 { get; set; }
        public Nullable<System.DateTime> MTL_CREATEDATE { get; set; }
        public string MTL_CREATE_USERID { get; set; }
        public Nullable<System.DateTime> MTL_UPDATEDATE { get; set; }
        public Nullable<int> MTL_UPDATE_USERID { get; set; }
    }
}
