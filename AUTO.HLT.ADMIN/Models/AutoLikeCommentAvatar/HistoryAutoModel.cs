using System;

namespace AUTO.HLT.ADMIN.Models.AutoLikeCommentAvatar
{
    public class HistoryAutoModel
    {
        public string ID { get; set; }
        public string UId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string TypeAuto { get; set; }
        public string UId_Friend { get; set; }
        public string Name_Friend { get; set; }
        public string Avatar_Friend { get; set; }
        public DateTime DateCreate { get; set; }
    }
}