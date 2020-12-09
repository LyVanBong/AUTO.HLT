using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTOHLT.MOBILE.Controls.Dialog.BuffService;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.Home
{
    public class HomeViewModel : ViewModelBase
    {
        private UserModel _userModel;
        private IDatabaseService _databaseService;
        private IUserService _userService;
        private string _moneyUser;
        private IPageDialogService _pageDialogService;
        private bool _isLoading;
        private int _permission;
        private IDialogService _dialogService;


        public ICommand BuffServiceCommand { get; private set; }
        public int Permission
        {
            get => _permission;
            set => SetProperty(ref _permission, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string MoneyUser
        {
            get => _moneyUser;
            set => SetProperty(ref _moneyUser, value);
        }

        public UserModel UserModel
        {
            get => _userModel;
            set => SetProperty(ref _userModel, value);
        }

        public ICommand LogoutCommand { get; private set; }
        public HomeViewModel(INavigationService navigationService, IDatabaseService databaseService, IUserService userService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            _pageDialogService = pageDialogService;
            _userService = userService;
            _databaseService = databaseService;
            LogoutCommand = new Command(LogoutAccount);
            IsLoading = true;
            BuffServiceCommand = new Command<string>(BuffService);
        }

        private async void BuffService(string key)
        {
            if (IsLoading) return;
            IsLoading = true;
            var para = new DialogParameters();
            para.Add("UserName", UserModel.UserName);
            switch (key)
            {
                case "0":
                    para.Add("ServiceName", Resource._1000031);
                    para.Add("IdProduct", "50d91145-9fb2-4be8-ac5f-1c7e5a97d34f");
                    break;
                case "1":
                    para.Add("ServiceName", Resource._1000032);
                    para.Add("IdProduct", "4f6225ee-c18c-4b14-b3cf-f7243d0f3dbf");
                    break;
                case "2":
                    para.Add("ServiceName", Resource._1000033);
                    para.Add("IdProduct", "1bddab36-9297-4698-9124-c977238a4a84");
                    break;
                default:
                    break;
            }

            await _dialogService.ShowDialogAsync(nameof(BuffDialog), para);
            IsLoading = false;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await InitializeDataHome();
            IsLoading = false;
        }

        private async Task InitializeDataHome()
        {
            try
            {
                var data = await _databaseService.GetAccountUser();
                if (data != null)
                {
                    UserModel = data;
                    Permission = data.Role;
                }

                var money = await _userService.GetMoneyUser(UserModel.UserName);
                if (money != null)
                {
                    if (money.Code > 0)
                    {
                        MoneyUser = money.Data;
                    }
                    else
                    {
                        MoneyUser = "0";
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async void LogoutAccount()
        {
            IsLoading = true;
            var res = await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000042, "OK", "Cancel");
            if (res)
            {
                Preferences.Clear();
                await _databaseService.DeleteAccontUser();
                await NavigationService.NavigateAsync("/LoginPage");
            }

            IsLoading = false;
        }
    }
}