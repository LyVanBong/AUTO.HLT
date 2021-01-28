namespace AUTO.HLT.ADMIN.Models.Facebook
{
    public class FriendsModel
    {
        public Datum[] data { get; set; }
        public Paging paging { get; set; }
        public Summary summary { get; set; }

        public class Paging
        {
            public Cursors cursors { get; set; }
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
            public string name { get; set; }
            public Picture picture { get; set; }
            public string id { get; set; }
        }

        public class Picture
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public string url { get; set; }
        }
    }
}