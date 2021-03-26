using System;

namespace AUTO.SEND.MESSAGE.FACEBOOK.TOOL.Models
{
    public class HistoryModel
    {
        public int Stt { get; set; }
        public string Id { get; set; }
        public string NameFriend { get; set; }
        public string Message { get; set; }
        public DateTime TimeSend { get; set; }
        public string IdFriend { get; set; }
        public string Note { get; set; }
    }
}