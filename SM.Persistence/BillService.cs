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
    
    public partial class BillService
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public long BillId { get; set; }
        public Nullable<double> DiscountPrice { get; set; }
        public Nullable<int> DiscountType { get; set; }
        public Nullable<int> DiscountRatio { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> RealPrice { get; set; }
    
        public virtual Service Service { get; set; }
        public virtual Bill Bill { get; set; }
    }
}
