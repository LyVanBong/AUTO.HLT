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
    
    public partial class GetAllUser_Result
    {
        public System.Guid ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int NumberPhone { get; set; }
        public string Email { get; set; }
        public int Sex { get; set; }
        public Nullable<int> Role { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public int Age { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<int> Price { get; set; }
        public string IdDevice { get; set; }
    }
}
