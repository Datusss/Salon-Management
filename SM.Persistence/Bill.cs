//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SM.Persistence
{
    using System;
    using System.Collections.Generic;
    
    public partial class Bill
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bill()
        {
            this.BillServices = new HashSet<BillService>();
        }
    
        public long Id { get; set; }
        public string Notice { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> DiscountPrice { get; set; }
        public string DiscountType { get; set; }
        public Nullable<byte> DiscountRatio { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public Nullable<int> StaffId { get; set; }
        public Nullable<int> SubStaffId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Staff Staff1 { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillService> BillServices { get; set; }
    }
}
