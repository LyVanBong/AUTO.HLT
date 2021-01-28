using Prism.Mvvm;

namespace AUTO.HLT.ADMIN.Models.Facebook
{
    public class PokesFriendsModel : BindableBase
    {
        private bool _isPokes;
        public string UId { get; set; }
        public string FullName { get; set; }

        public bool IsPokes
        {
            get => _isPokes;
            set => SetProperty(ref _isPokes, value);
        }

        public string DomIdReplace { get; set; }
        public string IsHide => "0";
        public string PokeTarget { get; set; }
        public string Ext { get; set; }
        public string Hash { get; set; }
    }
}