using AUTO.HLT.MOBILE.VIP.Views.Login;
using Prism.Navigation;
using Xamarin.Forms;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        private View _contentLoginPage;

        public View ContentLoginPage
        {
            get => _contentLoginPage;
            set => SetProperty(ref _contentLoginPage, value);
        }

        public LoginViewModel(INavigationService navigationService) : base(navigationService)
        {
            ContentLoginPage = new LoginView();
        }
    }
}