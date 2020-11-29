using System.Windows.Input;
using AUTOHLT.MOBILE.Views.Home;
using AUTOHLT.MOBILE.Views.Login;
using Prism.Navigation;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        private bool _isCheckSavePassword;
        private string _password;
        private string _userName;
        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsCheckSavePassword
        {
            get => _isCheckSavePassword;
            set => SetProperty(ref _isCheckSavePassword, value);
        }

        public ICommand SignUpCommand { get; private set; }
        public ICommand LoginCommand { get; private set; }
        public LoginViewModel(INavigationService navigationService) : base(navigationService)
        {
            SignUpCommand = new Command(SignUpAccount);
            LoginCommand = new Command(LoginAccount);
            IsLoading = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey("SignUp"))
                {
                    if (parameters.ContainsKey("UserName"))
                        UserName = parameters["UserName"] as string;
                }
            }
            IsLoading = false;
        }

        private void LoginAccount()
        {
            if (IsLoading) return;
            IsLoading = true;
            NavigationService.NavigateAsync(nameof(HomePage), null, true, true);
        }

        private void SignUpAccount()
        {
            if (IsLoading) return;
            IsLoading = true;
            NavigationService.NavigateAsync(nameof(SignUpPage), null, true, true);
        }
    }
}