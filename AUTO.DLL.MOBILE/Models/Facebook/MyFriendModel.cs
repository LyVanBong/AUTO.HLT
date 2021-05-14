namespace AUTO.DLL.MOBILE.Models.Facebook
{
    public class MyFriendModel
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public int Interactive { get; set; }
        public string Status { get; set; }
        public bool IsSelected { get; set; }
        public string Picture { get; set; }

        public MyFriendModel(int id, string uid, string name, int interactive, string status, bool isSelected, string picture)
        {
            Id = id;
            Uid = uid;
            Name = name;
            Interactive = interactive;
            Status = status;
            IsSelected = isSelected;
            Picture = picture;
        }
    }
}