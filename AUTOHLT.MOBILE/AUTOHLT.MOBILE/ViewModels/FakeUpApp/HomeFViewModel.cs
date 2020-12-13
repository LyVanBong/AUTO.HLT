using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.FakeUpApp
{
    public class HomeFViewModel : ViewModelBase
    {
        public ICommand LogoutCommand { get; private set; }
        public HomeFViewModel(INavigationService navigationService) : base(navigationService)
        {
            LogoutCommand = new Command(Logout);
        }

        private async void Logout()
        {
            await NavigationService.NavigateAsync("/LoginPage");
        }
    }
}