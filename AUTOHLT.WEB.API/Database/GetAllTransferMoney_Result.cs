//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AUTOHLT.WEB.API.Database
{
    using System;
    
    public partial class GetAllTransferMoney_Result
    {
        public System.Guid ID { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public double Discount { get; set; }
        public int Price { get; set; }
        public System.Guid ID_Send { get; set; }
        public System.Guid ID_Receive { get; set; }
        public int MoneyTransferType { get; set; }
    }
}
