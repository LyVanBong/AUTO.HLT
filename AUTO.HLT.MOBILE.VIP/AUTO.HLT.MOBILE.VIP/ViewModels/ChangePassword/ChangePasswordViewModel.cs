﻿using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Helpers;
using AUTO.HLT.MOBILE.VIP.Models.User;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.Services.GoogleAdmob;
using AUTO.HLT.MOBILE.VIP.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.ChangePassword
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        private bool _isLoading;
        private IDatabaseService _databaseService;
        private IPageDialogService _pageDialogService;
        private string _newPassword;
        private string _currentPassword;
        private IUserService _userService;
        private IGoogleAdmobService _googleAdmobService;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand ChangePassworkCommand { get; private set; }

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public string CurrentPassword
        {
            get => _currentPassword;
            set => SetProperty(ref _currentPassword, value);
        }

        public ChangePasswordViewModel(INavigationService navigationService, IDatabaseService databaseService, IPageDialogService pageDialogService, IUserService userService, IGoogleAdmobService googleAdmobService) : base(navigationService)
        {
            _googleAdmobService = googleAdmobService;
            _userService = userService;
            _databaseService = databaseService;
            _pageDialogService = pageDialogService;
            ChangePassworkCommand = new AsyncCommand(async () => await ChangePasswork());
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null && parameters.ContainsKey(AppConstants.AddAdmod))
            {
            }
        }

        private async Task ChangePasswork()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (!string.IsNullOrEmpty(CurrentPassword) && !string.IsNullOrEmpty(NewPassword))
                {
                    if (NewPassword.Equals(CurrentPassword))
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Mật khẩu mới trùng với mật khẩu hiện tại", "OK");
                        CurrentPassword = NewPassword = null;
                    }
                    else
                    {
                        var userLocal = await _databaseService.GetAccountUser();
                        var pass = HashFunctionHelper.GetHashCode(CurrentPassword, 1);
                        if (userLocal.Password.Equals(pass))
                        {
                            var user = new UserModel()
                            {
                                ID = userLocal.ID,
                                Age = userLocal.Age,
                                DateCreate = userLocal.DateCreate,
                                Email = userLocal.Email,
                                IdDevice = userLocal.IdDevice,
                                IsActive = userLocal.IsActive,
                                UserName = userLocal.UserName,
                                Role = userLocal.Role,
                                Name = userLocal.Name,
                                NumberPhone = userLocal.NumberPhone,
                                Price = userLocal.Price,
                                Password = HashFunctionHelper.GetHashCode(NewPassword, 1),
                                Sex = userLocal.Sex,
                            };
                            var updatePass = await _userService.UpdateUser(user);
                            if (updatePass != null)
                            {
                                await _pageDialogService.DisplayAlertAsync("Thông báo", "Thành công", "OK");
                                Preferences.Clear();
                                _databaseService.RemoveDatabaseLocal();
                                await NavigationService.NavigateAsync("/LoginPage");
                            }
                            else
                            {
                                await _pageDialogService.DisplayAlertAsync("Thông báo", "Đổi mật khẩu lỗi", "OK");
                            }
                        }
                        else
                        {
                            CurrentPassword = null;
                            await _pageDialogService.DisplayAlertAsync("Thông báo", "Mật khẩu hiện tại sai", "OK");
                        }
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Dữ liệu chưa đủ", "OK");
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