using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Models.Facebook;
using AUTO.HLT.MOBILE.VIP.Services.Facebook;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTO.HLT.MOBILE.VIP.Controls.ConnectFacebook;
using AUTO.HLT.MOBILE.VIP.Models.LicenseKey;
using AUTO.HLT.MOBILE.VIP.Services.LicenseKey;
using Microsoft.AppCenter.Crashes;
using Prism.Services;
using Prism.Services.Dialogs;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.HappyBirthday
{
    public class HappyBirthdayViewModel : ViewModelBase
    {
        private ObservableCollection<FriendBirthdayModel> _lsBirthday;
        private IFacebookService _facebookService;
        private ILicenseKeyService _licenseKeyService;
        private LicenseKeyModel _licenseKeyModel;
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

        public HappyBirthdayViewModel(INavigationService navigationService, IFacebookService facebookService, ILicenseKeyService licenseKeyService, IPageDialogService pageDialogService, IDialogService dialog) : base(navigationService)
        {
            _dialog = dialog;
            _pageDialogService = pageDialogService;
            _licenseKeyService = licenseKeyService;
            _facebookService = facebookService;

            HappyBirthdayCommand = new AsyncCommand(HappyBirthday);
        }

        private async Task HappyBirthday()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (_licenseKeyModel != null)
                {
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
                else
                {

                    await _pageDialogService.DisplayAlertAsync("Thông báo",
                        "Bạn nên nâng cập tài khoản để sử dụng đầy đủ tinh năng hơn", "OK");
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
                    Preferences.Get(AppConstants.TokenFaceook, ""), "id,name,birthday,picture{url},gender");
                if (data != null && data.data != null)
                {
                    var friend = data.data;
                    _licenseKeyModel = await _licenseKeyService.CheckLicenseForUser();
                    LsBirthday = new ObservableCollection<FriendBirthdayModel>();
                    if (_licenseKeyModel == null || _licenseKeyModel.CountEndDate < 0)
                    {
                        _licenseKeyModel = null;
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
                    else
                    {
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