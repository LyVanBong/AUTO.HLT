﻿using System;

namespace AUTO.HLT.MOBILE.VIP.Models.Login
{
    public class LoginModel
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string NumberPhone { get; set; }
        public string Email { get; set; }
        public int Sex { get; set; }
        public int Role { get; set; }
        public bool IsActive { get; set; }
        public string Age { get; set; }
        public DateTime DateCreate { get; set; }
        public float Price { get; set; }
        public string IdDevice { get; set; }
        public string Jwt { get; set; }
    }
}