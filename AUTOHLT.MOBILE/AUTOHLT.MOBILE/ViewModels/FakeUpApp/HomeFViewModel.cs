using System;
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
        public ICommand NavigationCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        public HomeFViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDatabaseService databaseService) : base(navigationService)
        {
            _databaseService = databaseService;
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
                    await NavigationService.NavigateAsync(nameof(TransferPage), para);
                    break;
                case "1":
                    await NavigationService.NavigateAsync(nameof(ChangePasswordPage));
                    break;
                case "2":
                    await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                        $"Bạn đã hoàn thành 100 ngày làm nhiệm vụ {Title}, điểm thưởng sẽ được thêm vào tài khoản vào lúc {DateTime.Now.ToString("F")} chúc bạn một ngày làm việc tốt lành !!!",
                        "OK");
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
                    await NavigationService.NavigateAsync(nameof(PokesPage), para);
                    break;
                case "6":
                    para.Add("Title", "Buff share");
                    para.Add("Uri", @"icon_share.png");
                    await NavigationService.NavigateAsync(nameof(FilterFriendPage), para);
                    break;
                case "7":
                    para.Add("Title", "Buff comment");
                    para.Add("Uri", @"icon_comments.png");
                    await NavigationService.NavigateAsync(nameof(UnLockFbPage), para);
                    break;
                case "8":
                    para.Add("Title", "Buff follow");
                    para.Add("Uri", @"icon_follow.png");
                    await NavigationService.NavigateAsync(nameof(SuportCustumerPage), para);
                    break;
                case "9":
                    Logout();
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