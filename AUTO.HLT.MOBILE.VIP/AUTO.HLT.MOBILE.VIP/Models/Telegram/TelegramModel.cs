namespace AUTO.HLT.MOBILE.VIP.Models.Telegram
{
    public class TelegramModel
    {
        public bool ok { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public int message_id { get; set; }
        public From from { get; set; }
        public Chat chat { get; set; }
        public int date { get; set; }
        public string text { get; set; }
    }

    public class From
    {
        public int id { get; set; }
        public bool is_bot { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
    }

    public class Chat
    {
        public long id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
    }
}