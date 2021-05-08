namespace AUTO.CONSOLE.CORE
{
    public class IdPostModel
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
        public string id { get; set; }
    }

}