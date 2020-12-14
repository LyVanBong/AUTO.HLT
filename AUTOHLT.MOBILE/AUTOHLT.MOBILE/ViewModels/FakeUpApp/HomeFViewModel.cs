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
            NavigationCommand = new Command(NavigationApp);
        }

        private async void NavigationApp()
        {
            await _pageDialogService.DisplayAlertAsync("Thông báo",
                $"Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi\nCông việc đã hoàn thành.\n{DateTime.Now.ToString("F")}", "OK");
        }

        private async void Logout()
        {
            await NavigationService.NavigateAsync("/LoginPage");
        }
    }
}