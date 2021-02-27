using AUTO.HLT.MOBILE.VIP.Views.Login;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        private View _contentLoginPage;
        private string _userName;
        private string _passwd;
        private bool _isSavePasswd;
        private string _fullName;
        private string _phoneNumber;
        private string _nguoiGioiThieu;

        public View ContentLoginPage
        {
            get => _contentLoginPage;
            set => SetProperty(ref _contentLoginPage, value);
        }

        public ICommand FunctionExecuteCommand { get; private set; }
        /// <summary>
        /// ten dung de dang nhap
        /// </summary>
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        /// <summary>
        /// mat khau
        /// </summary>
        public string Passwd
        {
            get => _passwd;
            set => SetProperty(ref _passwd, value);
        }
        /// <summary>
        /// luu mat khau tai khoan
        /// </summary>
        public bool IsSavePasswd
        {
            get => _isSavePasswd;
            set => SetProperty(ref _isSavePasswd, value);
        }
        /// <summary>
        /// ten nguoi dung
        /// </summary>
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => _phoneNumber = value;
        }

        public string NguoiGioiThieu
        {
            get => _nguoiGioiThieu;
            set => SetProperty(ref _nguoiGioiThieu, value);
        }

        public LoginViewModel(INavigationService navigationService) : base(navigationService)
        {
            ContentLoginPage = new LoginView();
            FunctionExecuteCommand = new AsyncCommand<string>(async (key) => await FunctionExecute(key));
        }

        private async Task FunctionExecute(string key)
        {
            switch (key)
            {
                case "0":
                    await NavigationService.NavigateAsync("/HomePage", null, false, true);
                    break;
                case "1":
                    ContentLoginPage = new SigupView();
                    break;
                case "2":
                    await NavigationService.NavigateAsync("/HomePage", null, false, true);
                    break;
                case "3":
                    ContentLoginPage = new LoginView();
                    break;
                default:
                    break;
            }
        }
    }
}