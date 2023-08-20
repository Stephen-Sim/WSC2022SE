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
    
    public partial class ItemPrice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ItemPrice()
        {
            this.BookingDetails = new HashSet<BookingDetail>();
        }
    
        public long ID { get; set; }
        public System.Guid GUID { get; set; }
        public long ItemID { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Price { get; set; }
        public long CancellationPolicyID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual CancellationPolicy CancellationPolicy { get; set; }
        public virtual Item Item { get; set; }
    }
}
