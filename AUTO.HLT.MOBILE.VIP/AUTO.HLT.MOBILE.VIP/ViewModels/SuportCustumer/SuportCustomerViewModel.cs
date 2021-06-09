using AUTO.HLT.MOBILE.VIP.Models.Login;
using AUTO.HLT.MOBILE.VIP.Services.Login;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Controls.GoogleAdmob;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.Services.GoogleAdmob;
using MarcTron.Plugin;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.SuportCustumer
{
    public class SuportCustomerViewModel : ViewModelBase
    {
        private bool _isLoading;
        private IPageDialogService _pageDialogService;
        private ILoginService _loginService;
        private InfoIntroducetorModel _introducetor;
        private IDatabaseService _databaseService;
        private string _versionApp;
        private ContentView _adModView;
        private IGoogleAdmobService _googleAdmobService;
        public ContentView AdModView
        {
            get => _adModView;
            set => SetProperty(ref _adModView, value);
        }
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public InfoIntroducetorModel Introducetor
        {
            get => _introducetor;
            set => SetProperty(ref _introducetor, value);
        }

        public ICommand SuportCommand { get; private set; }

        public string VersionApp
        {
            get => _versionApp;
            set => SetProperty(ref _versionApp, value);
        }

        public SuportCustomerViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ILoginService loginService, IDatabaseService databaseService, IGoogleAdmobService googleAdmobService) : base(navigationService)
        {
            _googleAdmobService = googleAdmobService;
            _databaseService = databaseService;
            _pageDialogService = pageDialogService;
            _loginService = loginService;
            SuportCommand = new AsyncCommand<string>(Suport);
            VersionApp = VersionTracking.CurrentVersion + " (" + VersionTracking.CurrentBuild + ")";
        }

        private async Task Suport(string arg)
        {
            try
            {
                switch (arg)
                {
                    case "0":
                        PhoneDialer.Open(Introducetor.NumberPhone);
                        break;
                    case "1":
                        var usr = await _databaseService.GetAccountUser();
                        await Sms.ComposeAsync(new SmsMessage($"Tôi {usr.Name} ({usr.UserName}) cần hỗ trợ về ứng dụng AUTOVIP", Introducetor.NumberPhone));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                await _pageDialogService.DisplayAlertAsync("Thông báo",
                      $"Liên hệ số điện thoại {Introducetor.NumberPhone} để được hỗ trợ ngay", "OK");
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            IsLoading = true;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            var data = await _loginService.Introducetor();
            Introducetor = data?.Data;
            IsLoading = false;
            if (parameters != null && parameters.ContainsKey(AppConstants.AddAdmod))
            {
                AdModView = new GoogleAdmobView();
                await Task.Delay(TimeSpan.FromSeconds(3));
                _googleAdmobService.ShowRewardedVideo();
            }
        }
    }
}