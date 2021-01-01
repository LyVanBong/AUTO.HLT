namespace AUTOHLT.MOBILE.Models.Facebook
{
    public class FriendsDoNotInteractModel
    {
        public string Id { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public int Reaction { get; set; }
        public int Comment { get; set; }
        public string Status { get; set; }
        public bool IsSelected { get; set; }
    }


    public class ReactionModel
    {
        public Timeline_Feed_Units timeline_feed_units { get; set; }
    }

    public class Timeline_Feed_Units
    {
        public Page_Info page_info { get; set; }
        public Edge[] edges { get; set; }
    }

    public class Page_Info
    {
        public string start_cursor { get; set; }
        public string end_cursor { get; set; }
        public bool has_next_page { get; set; }
        public bool has_previous_page { get; set; }
        public object delta_cursor { get; set; }
    }

    public class Edge
    {
        public Node node { get; set; }
    }

    public class Node
    {
        public string id { get; set; }
        public int creation_time { get; set; }
        public Feedback feedback { get; set; }
    }

    public class Feedback
    {
        public Reactors reactors { get; set; }
        public Commenters commenters { get; set; }
    }

    public class Reactors
    {
        public Node1[] nodes { get; set; }
    }

    public class Node1
    {
        public string id { get; set; }
    }

    public class Commenters
    {
        public Node2[] nodes { get; set; }
    }

    public class Node2
    {
        public string id { get; set; }
    }
}