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
    
    public partial class ErrorScreenshot
    {
        public long Id { get; set; }
        public string TheUser { get; set; }
        public byte[] screenshot { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<int> custID { get; set; }
        public Nullable<int> errorId { get; set; }
        public string errorMessage { get; set; }
    }
}
