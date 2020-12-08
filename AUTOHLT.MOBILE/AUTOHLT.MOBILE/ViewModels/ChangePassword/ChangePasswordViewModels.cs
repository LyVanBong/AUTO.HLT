using System;
using System.Windows.Input;
using AUTOHLT.MOBILE.Helpers;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.ChangePassword
{
    public class ChangePasswordViewModels : ViewModelBase
    {
        private bool _isLoading;
        private bool _isEnabledButton;
        private string _reNewPassword;
        private string _newPassword;
        private string _currentPassword;
        private IDatabaseService _databaseService;
        private IPageDialogService _pageDialogService;
        private IUserService _userService;
        private UserModel _user;

        public string CurrentPassword
        {
            get => _currentPassword;
            set => SetProperty(ref _currentPassword, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public string ReNewPassword
        {
            get => _reNewPassword;
            set => SetProperty(ref _reNewPassword, value);
        }

        public ICommand UnfocusedCommand { get; private set; }
        public ICommand ChangePassworkCommand { get; private set; }
        public bool IsEnabledButton
        {
            get => _isEnabledButton;
            set => SetProperty(ref _isEnabledButton, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ChangePasswordViewModels(INavigationService navigationService, IDatabaseService databaseService, IPageDialogService pageDialogService, IUserService userService) : base(navigationService)
        {
            _userService = userService;
            _pageDialogService = pageDialogService;
            _databaseService = databaseService;
            IsLoading = true;
            UnfocusedCommand = new Command<string>(Unfocused);
            ChangePassworkCommand = new Command(ChangePasswork);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = false;
        }

        private async void Unfocused(string key)
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                _user = await _databaseService.GetAccountUser();
                if (_user == null)
                {
                    await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000036, "OK");
                }
                else
                {
                    switch (key)
                    {
                        case "0":
                            if (HashFunctionHelper.GetHashCode(CurrentPassword, 1) != _user.Password)
                            {
                                await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000039, "OK");
                                CurrentPassword = "";
                            }

                            break;
                        case "1":
                            if (!string.IsNullOrWhiteSpace(NewPassword) && !string.IsNullOrWhiteSpace(ReNewPassword))
                            {
                                if (NewPassword != ReNewPassword)
                                    ReNewPassword = "";
                            }
                            break;
                        case "2":
                            if (!string.IsNullOrWhiteSpace(NewPassword) && !string.IsNullOrWhiteSpace(ReNewPassword))
                            {
                                if (NewPassword != ReNewPassword)
                                    ReNewPassword = "";
                            }
                            break;
                        default:
                            break;
                    }

                    if (CurrentPassword != null && NewPassword != null && ReNewPassword != null)
                        IsEnabledButton = true;
                    else
                        IsEnabledButton = false;
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

        private async void ChangePasswork()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (!string.IsNullOrWhiteSpace(CurrentPassword) && !string.IsNullOrWhiteSpace(NewPassword) &&
                    !string.IsNullOrWhiteSpace(ReNewPassword))
                {
                    if (_user != null)
                    {
                        var data = new UserModel
                        {
                            UserName = _user.UserName,
                            Name = _user.Name,
                            Password = HashFunctionHelper.GetHashCode(NewPassword, 1),
                            Email = _user.Email,
                            NumberPhone = _user.NumberPhone,
                            Sex = _user.Sex,
                            Role = _user.Role,
                            IsActive = _user.IsActive,
                            Age = _user.Age,
                            Price = _user.Price,
                            IdDevice = _user.IdDevice,
                        };
                        var res = await _userService.UpdateUser(data.UserName, data.Name, data.Password, data.Email,
                            data.NumberPhone.ToString(), data.Sex.ToString(), data.Role.ToString(),
                            data.IsActive.ToString(), data.Age.ToString(), data.Price.ToString(), data.IdDevice);
                        if (res != null && res.Code > 0)
                        {
                            await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000040, "OK");
                            Preferences.Clear();
                            await _databaseService.DeleteAccontUser();
                            await NavigationService.NavigateAsync("/LoginPage");
                        }
                        else
                        {
                            await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000036, "OK");
                        }
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000036, "OK");
                    }
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
    }
}