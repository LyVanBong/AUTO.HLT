namespace AUTO.DLL.MOBILE.Models.Facebook
{
    public class MyFriendModel : BindableBase
    {
        private int _interactive;
        private string _status;
        private bool _isSelected;
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }

        public int Interactive
        {
            get => _interactive;
            set => SetProperty(ref _interactive, value);
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

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