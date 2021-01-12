using System;
using System.Collections.Generic;
using System.Windows.Input;
using AUTOHLT.MOBILE.Views.AccountInformation;
using AUTOHLT.MOBILE.Views.ChangePassword;
using AUTOHLT.MOBILE.Views.FakeUpApp;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;
using ContentPage = AUTOHLT.MOBILE.Views.FakeUpApp.ContentPage;

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
                    para.Add("Uri", "icon_transfer.png");
                    await NavigationService.NavigateAsync(nameof(ContentPage), para);
                    break;
                case "1":
                    await NavigationService.NavigateAsync(nameof(ChangePasswordPage));
                    break;
                case "2":
                    para.Add("Title", "Thông báo");
                    para.Add("Uri", @"icon_notification.png");
                    await NavigationService.NavigateAsync(nameof(ContentPage), para);
                    break;
                case "3":
                    await NavigationService.NavigateAsync(nameof(AccountInformationPage));
                    break;
                case "4":
                    para.Add("Title", "Buff like");
                    para.Add("Uri", @"icon_like.png");
                    await NavigationService.NavigateAsync(nameof(ContentPage), para);
                    break;
                case "5":
                    para.Add("Title", "Buff view");
                    para.Add("Uri", @"icon_view.png");
                    await NavigationService.NavigateAsync(nameof(ContentPage), para);
                    break;
                case "6":
                    para.Add("Title", "Buff share");
                    para.Add("Uri", @"icon_share.png");
                    await NavigationService.NavigateAsync(nameof(ContentPage), para);
                    break;
                case "7":
                    para.Add("Title", "Buff comment");
                    para.Add("Uri", @"icon_comments.png");
                    await NavigationService.NavigateAsync(nameof(ContentPage), para);
                    break;
                case "8":
                    para.Add("Title", "Buff follow");
                    para.Add("Uri", @"icon_follow.png");
                    await NavigationService.NavigateAsync(nameof(ContentPage), para);
                    break;
                case "9":
                    para.Add("Title", "Tương tác");
                    para.Add("Uri", @"icon_Interactive.png");
                    await NavigationService.NavigateAsync(nameof(ContentPage), para);
                    break;
            }
        }

        private async void Logout()
        {
            await NavigationService.NavigateAsync("/LoginPage");
        }
    }
}