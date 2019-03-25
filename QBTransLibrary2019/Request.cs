//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QBTransLibrary2019
{
    using System;
    using System.Collections.Generic;
    
    public partial class Request
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Request()
        {
            this.CheckRequests = new HashSet<CheckRequest>();
            this.CheckRequestStatusMessages = new HashSet<CheckRequestStatusMessage>();
        }
    
        public int ID { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string Creator { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateToBeRun { get; set; }
        public Nullable<bool> Satisfied { get; set; }
        public Nullable<bool> SentToAccounting { get; set; }
        public Nullable<bool> transactionError { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckRequest> CheckRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckRequestStatusMessage> CheckRequestStatusMessages { get; set; }
    }
}