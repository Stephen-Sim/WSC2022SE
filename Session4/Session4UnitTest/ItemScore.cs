//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Session4UnitTest
{
    using System;
    using System.Collections.Generic;
    
    public partial class ItemScore
    {
        public long ID { get; set; }
        public System.Guid GUID { get; set; }
        public long UserID { get; set; }
        public long ItemID { get; set; }
        public long ScoreID { get; set; }
        public long Value { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Score Score { get; set; }
        public virtual User User { get; set; }
    }
}
