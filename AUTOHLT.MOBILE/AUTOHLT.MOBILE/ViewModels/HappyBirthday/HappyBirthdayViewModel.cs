using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Controls.Dialog.ConnectFacebook;
using AUTOHLT.MOBILE.Models.Facebook;
using AUTOHLT.MOBILE.Services.Facebook;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace AUTOHLT.MOBILE.ViewModels.HappyBirthday
{
    public class HappyBirthdayViewModel : ViewModelBase
    {
        private ObservableCollection<FriendBirthdayModel> _lsBirthday;
        private IFacebookService _facebookService;
        private IPageDialogService _pageDialogService;
        private bool _isLoading;
        private IDialogService _dialog;

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

        public HappyBirthdayViewModel(INavigationService navigationService, IFacebookService facebookService, IPageDialogService pageDialogService, IDialogService dialog) : base(navigationService)
        {
            _dialog = dialog;
            _pageDialogService = pageDialogService;
            _facebookService = facebookService;

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
                IsLoading = true;
                await InitData();
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

        private async Task InitData()
        {
            if (await _facebookService.CheckCookieAndToken())
            {
                var data = await _facebookService.GetAllFriend<FriendsModel>(
                    Preferences.Get(AppConstants.TokenFaceook, ""), "id,name,birthday,picture{url},gender", "");
                if (data != null && data.data != null)
                {
                    var friend = data.data;
                    if (friend != null && friend.Any())
                    {
                        LsBirthday = new ObservableCollection<FriendBirthdayModel>();
                        foreach (var item in friend)
                        {
                            var day = item?.birthday;
                            if (!string.IsNullOrEmpty(day))
                            {
                                LsBirthday.Add(new FriendBirthdayModel()
                                {
                                    Id = item.id,
                                    Name = item.name,
                                    Picture = item.picture.data.url,
                                    Gender = item.gender,
                                    Birthday = item.birthday,
                                    MessageContent = $"Thay mặt chủ tịch nước , tổng bí thư , các tập thể ban ngành chúc {item.name} sinh nhật vui vẻ",
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