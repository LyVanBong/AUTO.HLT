namespace AUTOHLT.MOBILE.Models.Facebook
{
    public class FriendBirthdayModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public bool IsSendMessage { get; set; }
        public bool IsPost { get; set; }
        public string MessageContent { get; set; }
    }
}