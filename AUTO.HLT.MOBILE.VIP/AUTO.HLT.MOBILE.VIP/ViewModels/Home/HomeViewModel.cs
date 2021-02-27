using System.Collections.Generic;
using System.Collections.ObjectModel;
using AUTO.HLT.MOBILE.VIP.Models.Home;
using Prism.Navigation;
using Xamarin.Forms;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.Home
{
    public class HomeViewModel : ViewModelBase
    {
        public ObservableCollection<ItemMenuModel> ListItemMenus { get; set; }

        public HomeViewModel(INavigationService navigationService) : base(navigationService)
        {
            ListItemMenus = new ObservableCollection<ItemMenuModel>(GetItemMenu());
        }

        private List<ItemMenuModel> GetItemMenu()
        {
            return new List<ItemMenuModel>
            {
                new ItemMenuModel()
                {
                    Id = 1,
                    TitleItem = "Tăng like bài viết",
                    Role = 99,
                    BgColor = Color.FromHex("e40017"),
                    Icon = "icon_like.png"
                },
                new ItemMenuModel()
                {
                    Id = 2,
                    TitleItem = "Tăng lượt theo dõi",
                    Role = 99,
                    BgColor = Color.FromHex("8ac4d0"),
                    Icon = "icon_follow.png"
                },
                new ItemMenuModel()
                {
                    Id = 3,
                    TitleItem = "Tự động thả tim",
                    Role = 99,
                    BgColor = Color.FromHex("f0a500"),
                    Icon = "icon_auto_boot_hear.png"
                },
                new ItemMenuModel()
                {
                    Id = 4,
                    TitleItem = "Lọc bạn bè",
                    Role = 99,
                    BgColor = Color.FromHex("6ddccf"),
                    Icon = "icon_filter_friends.png"
                },
                new ItemMenuModel()
                {
                    Id = 5,
                    TitleItem = "Chọc bạn bè",
                    Role = 99,
                    BgColor = Color.FromHex("383e56"),
                    Icon = "icon_pokes.png"
                },
                new ItemMenuModel()
                {
                    Id = 6,
                    TitleItem = "Chúc mừng sinh nhật bạn bè",
                    Role = 99,
                    BgColor = Color.FromHex("161d6f"),
                    Icon = "icon_birthday.png"
                },
                new ItemMenuModel()
                {
                    Id = 7,
                    TitleItem = "Tự động tương tác avatar",
                    Role = 99,
                    BgColor = Color.FromHex("6930c3"),
                    Icon = "icon_Interactive.png"
                },
                new ItemMenuModel()
                {
                    Id = 8,
                    TitleItem = "Hố trợ",
                    Role = 99,
                    BgColor = Color.FromHex("ff4b5c"),
                    Icon = "icon_customer_support.png"
                },

            };
        }
    }
}