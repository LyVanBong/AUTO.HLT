using System;
using System.Threading;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Views.AccountInformation;
using AUTOHLT.MOBILE.Views.ChangePassword;
using Prism.Navigation;
using Prism.Services;
using System.Windows.Input;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Views.FakeUpApp;
using AUTOHLT.MOBILE.Views.FilterFriend;
using AUTOHLT.MOBILE.Views.Pokes;
using AUTOHLT.MOBILE.Views.SuportCustumer;
using AUTOHLT.MOBILE.Views.Transfers;
using AUTOHLT.MOBILE.Views.UnLockFb;
using Xamarin.Forms;
using ContentPage = AUTOHLT.MOBILE.Views.FakeUpApp.ContentPage;

namespace AUTOHLT.MOBILE.ViewModels.FakeUpApp
{
    public class HomeFViewModel : ViewModelBase
    {
        private IPageDialogService _pageDialogService;
        private IDatabaseService _databaseService;
        private Color _bgColor;
        public ICommand NavigationCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        public Color BgColor
        {
            get => _bgColor;
            set => SetProperty(ref _bgColor, value);
        }

        public HomeFViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDatabaseService databaseService) : base(navigationService)
        {
            _databaseService = databaseService;
            _pageDialogService = pageDialogService;
            LogoutCommand = new Command(Logout);
            NavigationCommand = new Command<string>(NavigationApp);
            new Thread(() =>
            {
                var color = new string[] { "78c4d4", "78c4d4", "fa1e0e", "763857", "28527a", "4a3933", "f14668", "f14668", "85603f", "e40017", "ff75a0", "91091e", "91091e", "0a043c", "fcd1d1" };
                Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                {
                    BgColor=Color.FromHex(color[(new Random()).Next(0,14)]);
                    return true;
                } );
            }).Start();
        }

        private async void NavigationApp(string key)
        {
            var para = new NavigationParameters();
            switch (key)
            {
                case "0":
                    para.Add("Title", "Truyển dữ liệu");
                    para.Add("Uri", "icon_transfer.png");
                    await NavigationService.NavigateAsync(nameof(FContentPage7), para);
                    break;
                case "1":
                    await NavigationService.NavigateAsync(nameof(ChangePasswordPage));
                    break;
                case "2":
                    await NavigationService.NavigateAsync(nameof(FContentPage8));
                    break;
                case "3":
                    await NavigationService.NavigateAsync(nameof(AccountInformationPage));
                    break;
                case "4":
                    await NavigationService.NavigateAsync(nameof(ContentPage), para);
                    break;
                case "5":
                    await NavigationService.NavigateAsync(nameof(FContentPage2), para);
                    break;
                case "6":
                    await NavigationService.NavigateAsync(nameof(FContentPage3), para);
                    break;
                case "7":
                    para.Add("Title", "Buff comment");
                    para.Add("Uri", @"icon_comments.png");
                    await NavigationService.NavigateAsync(nameof(FContentPage4), para);
                    break;
                case "8":
                    para.Add("Title", "Buff follow");
                    para.Add("Uri", @"icon_follow.png");
                    await NavigationService.NavigateAsync(nameof(FContentPage5), para);
                    break;
                case "9":
                    await NavigationService.NavigateAsync(nameof(FContentPage6), para);
                    break;
            }
        }

        private async void Logout()
        {
            await _databaseService.DeleteAccontUser();
            await NavigationService.NavigateAsync("/LoginPage");
        }
    }
}