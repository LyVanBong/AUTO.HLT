namespace AUTO.HLT.MOBILE.VIP.Models.Facebook
{
    public class FriendsModel
    {
        public Datum[] data { get; set; }
        public Paging paging { get; set; }
        public Summary summary { get; set; }
    }

    public class Paging
    {
        public Cursors cursors { get; set; }
        public string next { get; set; }
    }

    public class Cursors
    {
        public string before { get; set; }
        public string after { get; set; }
    }

    public class Summary
    {
        public int total_count { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public string name { get; set; }
        public Pictures picture { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
    }

    public class Pictures
    {
        public Datas data { get; set; }
    }

    public class Datas
    {
        public string url { get; set; }
    }
}