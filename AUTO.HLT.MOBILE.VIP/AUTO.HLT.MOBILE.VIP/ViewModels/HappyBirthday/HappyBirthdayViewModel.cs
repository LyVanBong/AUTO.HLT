﻿using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Controls.ConnectFacebook;
using AUTO.HLT.MOBILE.VIP.Models.Facebook;
using AUTO.HLT.MOBILE.VIP.Services.Facebook;
using AUTO.HLT.MOBILE.VIP.Services.GoogleAdmob;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.Services.User;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.HappyBirthday
{
    public class HappyBirthdayViewModel : ViewModelBase
    {
        private ObservableCollection<FriendBirthdayModel> _lsBirthday;
        private IFacebookService _facebookService;
        private IPageDialogService _pageDialogService;
        private bool _isLoading;
        private IDialogService _dialog;
        private IGoogleAdmobService _googleAdmobService;
        private IDatabaseService _databaseService;
        private IUserService _userService;
        public ObservableCollection<FriendBirthdayModel> LsBirthday
        {
            get => _lsBirthday;
            set => SetProperty(ref _lsBirthday, value);
        }

        public ICommand HappyBirthdayCommand { get; private set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public HappyBirthdayViewModel(INavigationService navigationService, IFacebookService facebookService, IPageDialogService pageDialogService, IDialogService dialog, IGoogleAdmobService googleAdmobService, IDatabaseService databaseService, IUserService userService) : base(navigationService)
        {
            _googleAdmobService = googleAdmobService;
            _dialog = dialog;
            _pageDialogService = pageDialogService;
            _facebookService = facebookService;
            _databaseService = databaseService;
            _userService = userService;

            HappyBirthdayCommand = new AsyncCommand(HappyBirthday);

        }

        private async Task HappyBirthday()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (LsBirthday.Any())
                {
                    var data = LsBirthday?.Where(x => x.IsPost || x.IsSendMessage)?.ToList();
                    if (data.Any())
                    {
                        foreach (var friend in data)
                        {
                            if (friend.IsPost)
                            {
                                var post = await _facebookService.PostNewsOnFacebookFriend(friend.Id, friend.MessageContent);
                            }

                            await Task.Delay(TimeSpan.FromSeconds(1));
                            if (friend.IsSendMessage)
                            {
                                var message = await _facebookService.SendMessageFacebook(friend.MessageContent, friend.Id);
                            }
                            await Task.Delay(TimeSpan.FromSeconds(1));
                        }

                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Chúc mừng sinh nhật thành công", "OK");
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Thông báo",
                        "Hôm nay không có ai sinh nhật", "OK");
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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
                if (parameters != null && parameters.ContainsKey(AppConstants.AddAdmod))
                {
                    var user = await _databaseService.GetAccountUser();
                    if (user != null)
                    {
                        var money = await _userService.GetPriceUser(user.UserName);
                        if (money > 0)
                        {
                            var num = await _userService.SetPriceUser(user.UserName, money - 1 + "");
                        }
                        else
                        {
                            if (await _pageDialogService.DisplayAlertAsync("Thông báo", $"Để sử dụng tính năng này bạn cần 1 xu !",
                                "Kiếm thếm xu", "Để sau"))
                            {
                                await NavigationService.NavigateAsync("EarnCoinsPage");
                            }
                            else
                            {
                                await NavigationService.GoBackAsync();
                            }
                        }
                    }
                }
                await InitData();
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async Task InitData()
        {
            if (await _facebookService.CheckCookieAndToken())
            {
                var data = await _facebookService.GetAllFriend<FriendsModel>(
                    Preferences.Get(AppConstants.TokenFaceook, ""), "id,name,birthday,picture{url},gender");
                if (data != null && data.data != null)
                {
                    var friend = data.data;
                    LsBirthday = new ObservableCollection<FriendBirthdayModel>();

                    foreach (var item in friend)
                    {
                        var day = item?.birthday;
                        if (!string.IsNullOrEmpty(day))
                        {
                            var d = day.Split('/');
                            var ngay = int.Parse(d[1]);
                            var thang = int.Parse(d[0]);
                            if (ngay == DateTime.Today.Day && thang == DateTime.Today.Month)
                            {
                                LsBirthday.Add(new FriendBirthdayModel()
                                {
                                    Id = item.id,
                                    Name = item.name,
                                    Picture = item.picture.data.url,
                                    Gender = item.gender,
                                    Birthday = item.birthday,
                                    MessageContent = "Chúc mừng sinh nhật " + item.name + ", tuổi mới thành công hơn",
                                });
                            }
                        }
                    }
                }
            }
            else
            {
                if (await _pageDialogService.DisplayAlertAsync("Thông báo", "Tính năng này cần kết nối với facebook của bạn", "Kết nối ngay", "Thôi"))
                {
                    await _dialog.ShowDialogAsync(nameof(ConnectFacebookDialog));
                }
                else
                {
                    await NavigationService.GoBackAsync();
                }
            }
        }
    }
}