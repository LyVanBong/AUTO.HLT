using System.Collections.Generic;

namespace AUTOHLT.MOBILE.Models.Facebook
{
    public class InviteFriendsLikePageModel
    {
        public Input input { get; set; }
    }

    public class Input
    {
        public string client_mutation_id { get; set; }
        public string actor_id { get; set; }
        public bool in_messenger { get; set; }
        public object invitee_id { get; set; }
        public List<string> invitee_ids { get; set; }
        public string invitee_type { get; set; }
        public string page_id { get; set; }
        public string referrer { get; set; }
    }
}