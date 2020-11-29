using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.Home
{
    public class HomeViewModel : ViewModelBase
    {
        public ICommand LogoutCommand { get; private set; }
        public HomeViewModel(INavigationService navigationService) : base(navigationService)
        {
            LogoutCommand=new Command(LogoutAccount);
        }

        private void LogoutAccount()
        {
            NavigationService.NavigateAsync("/LoginPage");
        }
    }
}