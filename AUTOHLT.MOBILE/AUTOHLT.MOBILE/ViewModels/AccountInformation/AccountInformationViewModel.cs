using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.AccountInformation
{
    public class AccountInformationViewModel : ViewModelBase
    {
        private string _userName;
        private string _name;
        private string _phoneNumber;
        private string _email;
        private string _age;
        private bool _isEnabledButton;
        private bool _isLoading;
        private IDatabaseService _databaseService;
        private UserModel _user;
        private IUserService _userService;
        private IPageDialogService _pageDialogService;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand UpdateInfoAccountCommand { get; private set; }
        public bool IsEnabledButton
        {
            get => _isEnabledButton;
            set => SetProperty(ref _isEnabledButton, value);
        }

        public string Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ICommand UnfocusedCommand { get; private set; }
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public AccountInformationViewModel(INavigationService navigationService, IDatabaseService databaseService, IUserService userService,IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _userService = userService;
            _databaseService = databaseService;
            IsLoading = true;
            UpdateInfoAccountCommand = new Command(UpdateInfoAccount);
            UnfocusedCommand = new Command<string>(Unfocused);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await InitializationData();
            IsLoading = false;
        }

        private async Task InitializationData()
        {
            try
            {
                _user = await _databaseService.GetAccountUser();
                if (_user != null)
                {
                    UserName = _user.UserName;
                    Name = _user.Name;
                    PhoneNumber = _user.NumberPhone + "";
                    Age = _user.Age + "";
                    Email = _user.Email;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async void UpdateInfoAccount()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                var data = new UserModel
                {
                    UserName = UserName,
                    Name = Name,
                    Password = _user.Password,
                    Email = Email,
                    NumberPhone = PhoneNumber,
                    Sex = _user.Sex,
                    Role = _user.Role,
                    IsActive = _user.IsActive,
                    Age = Age,
                    Price = _user.Price,
                    IdDevice = _user.IdDevice,
                };
                var update = await _userService.UpdateUser(data.UserName, data.Name, data.Password,
                    data.Email, data.NumberPhone.ToString(), data.Sex.ToString(), data.Role.ToString(),
                    data.IsActive.ToString(), data.Age.ToString(), data.Price.ToString(), data.IdDevice);
                if (update != null && update.Code > 0)
                {
                    await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000040, "OK");
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000041, "OK");
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void Unfocused(string key)
        {

            if (Name != null && PhoneNumber != null && Age != null && Email != null)
                IsEnabledButton = true;
            else
                IsEnabledButton = false;
        }
    }
}