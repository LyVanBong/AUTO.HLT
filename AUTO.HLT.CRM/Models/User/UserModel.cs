using System;
using System.Collections.Generic;
using System.Globalization;

namespace AUTO.HLT.CRM.Models.User
{
    public class UserModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<InfoUser> Data { get; set; }
    }

    public class InfoUser
    {
        private string _role;
        public int Stt { get; set; }
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string NumberPhone { get; set; }
        public string Email { get; set; }
        public int Sex { get; set; }

        public string Role
        {
            get
            {
                if (_role=="0")
                {
                    return "Admin";
                }else if (_role=="2")
                {
                    return "Khách hàng";
                }

                return "user";
            }
            set => _role = value;
        }

        public bool IsActive { get; set; }
        public string Age { get; set; }
        public DateTime DateCreate { get; set; }
        public float Price { get; set; }
        public string PriceExtend => string.Format(new CultureInfo("en-US"), "{0:0,0}", Price);
        public string IdDevice { get; set; }
    }
}