//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FreshDesktop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CancellationRefundFee
    {
        public long ID { get; set; }
        public System.Guid GUID { get; set; }
        public long CancellationPolicyID { get; set; }
        public int DaysLeft { get; set; }
        public decimal PenaltyPercentage { get; set; }
    
        public virtual CancellationPolicy CancellationPolicy { get; set; }
    }
}
