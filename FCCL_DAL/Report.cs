//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FCCL_DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Report
    {
        public int Id { get; set; }
        public System.DateTime Created { get; set; }
        public Nullable<System.DateTime> Updated { get; set; }
        public short Application { get; set; }
        public short ReportType { get; set; }
        public long ReportNumber { get; set; }
        public int ObjectId { get; set; }
        public string ObjectName { get; set; }
        public System.DateTime TestDate { get; set; }
        public System.DateTime PrintDate { get; set; }
        public Nullable<int> PageCount { get; set; }
        public Nullable<int> SampleCount { get; set; }
    }
}
