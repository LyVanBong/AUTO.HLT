using System;

namespace AUTOHLT.WEB.API.Models.Version1
{
    public class AdduserAutoLikeCommentModel
    {
        public string Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int ExpiredTime { get; set; }
        public string F_Cookie { get; set; }
        public string F_Token { get; set; }
    }

    public class UpdateUserAutoLikeCommentModel
    {
        public string Id { get; set; }
        public string UId { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public bool IsRunWork { get; set; }
    }

    public class AddFUIdAutoLikeCommentModel
    {
        public string Id { get; set; }
        public string UId { get; set; }
    }
}