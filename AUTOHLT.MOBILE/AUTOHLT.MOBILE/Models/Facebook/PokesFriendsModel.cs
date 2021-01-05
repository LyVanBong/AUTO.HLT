using Prism.Mvvm;

namespace AUTOHLT.MOBILE.Models.Facebook
{
    public class PokesFriendsModel : BindableBase
    {
        private bool _isPokes;
        public string UId { get; set; }
        public string Avatar { get; set; }
        public string FullName { get; set; }

        public bool IsPokes
        {
            get => _isPokes;
            set => SetProperty(ref _isPokes, value);
        }

        public string UriPokes { get; set; }
    }
}