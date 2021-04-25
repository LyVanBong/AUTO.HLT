namespace AUTO.ALL.IN.APP.Models
{
    public class DataFacebookModel
    {
        public Friends friends { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public Picture picture { get; set; }
        public string email { get; set; }
    }

    public class Friends
    {
        public Datum[] data { get; set; }
        public Paging paging { get; set; }
        public Summary summary { get; set; }
    }

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
        public bool IsDone { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Picture
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public int height { get; set; }
        public bool is_silhouette { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }
}