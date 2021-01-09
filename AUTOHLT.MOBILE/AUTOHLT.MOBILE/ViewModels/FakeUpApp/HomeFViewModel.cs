using System;
using System.Windows.Input;
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
            await _pageDialogService.DisplayAlertAsync("Buff like",
                $"bạn đã sử dụng tính năng buff like thành công", "OK");
            switch (key)
            {
                case "0":
                    await _pageDialogService.DisplayAlertAsync("Buff view",
                        $"Đã buff view thành công", "OK");
                    break;
                case "1":
                    await _pageDialogService.DisplayAlertAsync("Buff share",
                        $"Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi\nCông việc đã hoàn thành.\n{DateTime.Now.ToString("F")}", "OK");
                    break;
                case "2":
                    await _pageDialogService.DisplayAlertAsync("Buff comment",
                        $"Đã comment tất cả các bài viết xong", "OK");
                    break;
                case "3":
                    await _pageDialogService.DisplayAlertAsync("Buff follower",
                        $"Đã tăng lượng follow theo yêu cầu của bạn thành công", "OK");
                    break;
                case "4":
                    await _pageDialogService.DisplayAlertAsync("tuong tac",
                        $"Đã tương tác với tất cả bạn bè xong", "OK");
                    break;
                case "5":
                    await _pageDialogService.DisplayAlertAsync("Thông báo",
                        $"Nhiệm vụ mới đã được thêm vào nhiệm vụ của bạn", "OK");
                    break;
                case "6":
                    await _pageDialogService.DisplayAlertAsync("Thông báo",
                        $"Thời gian hiện tại là: {DateTime.Now.ToString("F")}", "OK");
                    break;
            }
        }

        private async void Logout()
        {
            await NavigationService.NavigateAsync("/LoginPage");
        }
    }
}