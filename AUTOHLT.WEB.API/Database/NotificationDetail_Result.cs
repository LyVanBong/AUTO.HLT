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
    
    public partial class NotificationDetail_Result
    {
        public System.Guid ID { get; set; }
        public string NotificationContent { get; set; }
        public System.Guid IDSend { get; set; }
        public System.Guid IDReceive { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public int NotificationType { get; set; }
    }
}
