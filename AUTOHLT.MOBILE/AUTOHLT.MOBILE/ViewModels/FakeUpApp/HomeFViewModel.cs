using System;
using System.Collections.Generic;
using System.Windows.Input;
using AUTOHLT.MOBILE.Views.AccountInformation;
using AUTOHLT.MOBILE.Views.ChangePassword;
using AUTOHLT.MOBILE.Views.FakeUpApp;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.FakeUpApp
{
    public class HomeFViewModel : ViewModelBase
    {
        private IPageDialogService _pageDialogService;
        public ICommand NavigationCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        public HomeFViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            LogoutCommand = new Command(Logout);
            NavigationCommand = new Command<string>(NavigationApp);
        }

        private async void NavigationApp(string key)
        {
            var para = new NavigationParameters();
            switch (key)
            {
                case "0":
                    para.Add("Title", "Truyển dữ liệu");
                    para.Add("Uri", @"https://vi.wikipedia.org/wiki/Truy%E1%BB%81n_d%E1%BB%AF_li%E1%BB%87u#:~:text=Truy%E1%BB%81n%20d%E1%BB%AF%20li%E1%BB%87u%20hay%20truy%E1%BB%81n,%C4%91%C6%A1n%20%C4%91i%E1%BB%83m%20%C4%91%E1%BA%BFn%20%C4%91a%20%C4%91i%E1%BB%83m).");
                    await NavigationService.NavigateAsync(nameof(WebviewPage), para);
                    break;
                case "1":
                    await NavigationService.NavigateAsync(nameof(ChangePasswordPage));
                    break;
                case "2":
                    para.Add("Title", "Thông báo");
                    para.Add("Uri", @"https://thuvienphapluat.vn/tintuc/vn/thoi-su-phap-luat/thoi-su/33283/thong-bao-lich-nghi-tet-am-lich-2021-tai-ha-noi");
                    await NavigationService.NavigateAsync(nameof(WebviewPage), para);
                    break;
                case "3":
                    await NavigationService.NavigateAsync(nameof(AccountInformationPage));
                    break;
                case "4":
                    para.Add("Title", "Buff like");
                    para.Add("Uri", @"https://onesignal.com/");
                    await NavigationService.NavigateAsync(nameof(WebviewPage), para);
                    break;
                case "5":
                    para.Add("Title", "Buff view");
                    para.Add("Uri", @"https://www.vieweyecare.com/");
                    await NavigationService.NavigateAsync(nameof(WebviewPage), para);
                    break;
                case "6":
                    para.Add("Title", "Buff share");
                    para.Add("Uri", @"http://www.share-project.org/data-access.html");
                    await NavigationService.NavigateAsync(nameof(WebviewPage), para);
                    break;
                case "7":
                    para.Add("Title", "Buff comment");
                    para.Add("Uri", @"https://docs.oracle.com/cd/B19306_01/server.102/b14200/statements_4009.htm");
                    await NavigationService.NavigateAsync(nameof(WebviewPage), para);
                    break;
                case "8":
                    para.Add("Title", "Buff follow");
                    para.Add("Uri", @"https://en.wikipedia.org/wiki/Friending_and_following");
                    await NavigationService.NavigateAsync(nameof(WebviewPage), para);
                    break;
                case "9":
                    para.Add("Title", "Tương tác");
                    para.Add("Uri", @"https://www.interactivebrokers.com/en/home.php");
                    await NavigationService.NavigateAsync(nameof(WebviewPage), para);
                    break;
            }
        }

        private async void Logout()
        {
            await NavigationService.NavigateAsync("/LoginPage");
        }
    }
}