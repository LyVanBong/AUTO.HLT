using System;

namespace AUTO.HLT.ADMIN.Models.Facebook
{
    public class PostIdMyFriendModel
    {
        public Posts posts { get; set; }
        public string id { get; set; }
    }

    public class Posts
    {
        public Datum[] data { get; set; }
        public Paging paging { get; set; }
    }

    public class Paging
    {
        public string previous { get; set; }
        public string next { get; set; }
    }

    public class Datum
    {
        public DateTime created_time { get; set; }
        public string message { get; set; }
        public string id { get; set; }
    }
}