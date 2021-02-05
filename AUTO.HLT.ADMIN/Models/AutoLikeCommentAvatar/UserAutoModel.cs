using System;

namespace AUTO.HLT.ADMIN.Models.AutoLikeCommentAvatar
{
    public class UserAutoModel
    {
        public string Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int ExpiredTime { get; set; }
        public string F_Cookie { get; set; }
        public string F_Token { get; set; }
        public string UId { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public bool? IsRunWork { get; set; }
    }
}