using AUTO.DLL.MOBILE.Services.Facebook;
using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Controls.ConnectFacebook;
using AUTO.HLT.MOBILE.VIP.Models.Facebook;
using AUTO.HLT.MOBILE.VIP.Services.LicenseKey;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTO.DLL.MOBILE.Models.Facebook;
using Xamarin.Essentials;
using Xamarin.Forms;
using IFacebookService = AUTO.HLT.MOBILE.VIP.Services.Facebook.IFacebookService;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.FilterFriend
{
    public class FilterFriendViewModel : ViewModelBase
    {
        private bool _isLoading;
        private IFacebookService _facebookService;
        private string _idFacebook;
        private string _nameUserFacebook;
        private string _avatarFacebook;
        private string _reaction;
        private string _comment;
        private ObservableCollection<MyFriendModel> _friendsDoNotInteractData;

        private List<FillterFriendModel> _fillterFriendModels = new List<FillterFriendModel>
        {
            new FillterFriendModel{Id = 0,NameFilter = "Không tương tác"},
            new FillterFriendModel{Id = 5,NameFilter = "<5"},
            new FillterFriendModel{Id = 10,NameFilter = "<10"},
            new FillterFriendModel{Id = 20,NameFilter = "<20"},
            new FillterFriendModel{Id = 30,NameFilter = "<30"},
            new FillterFriendModel{Id = 50,NameFilter = "<50"},
            new FillterFriendModel{Id = -1,NameFilter ="Không lọc"},
        };

        private IPageDialogService _pageDialogService;
        private List<MyFriendModel> _doNotInteract;
        private IDialogService _dialogService;
        private ILicenseKeyService _licenseKeyService;
        private int _numberPost = 15;

        private AUTO.DLL.MOBILE.Services.Facebook.IFacebookService _facebook = new FacebookeService();

        public ICommand FilterFriendsCommand { get; private set; }

        public ICommand FillterCommand { get; private set; }
        public List<FillterFriendModel> FillterFriendModels
        {
            get => _fillterFriendModels;
            set => SetProperty(ref _fillterFriendModels, value);
        }

        public ObservableCollection<MyFriendModel> FriendsDoNotInteractData
        {
            get => _friendsDoNotInteractData;
            set => SetProperty(ref _friendsDoNotInteractData, value);
        }

        public string AvatarFacebook
        {
            get => _avatarFacebook;
            set => SetProperty(ref _avatarFacebook, value);
        }

        public string NameUserFacebook
        {
            get => _nameUserFacebook;
            set => SetProperty(ref _nameUserFacebook, value);
        }

        public string IdFacebook
        {
            get => _idFacebook;
            set => SetProperty(ref _idFacebook, value);
        }

        public ICommand ConnectFacebookCommand { get; private set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public int NumberPost
        {
            get => _numberPost;
            set => SetProperty(ref _numberPost, value);
        }

        public FilterFriendViewModel(INavigationService navigationService, IFacebookService facebookService, IPageDialogService pageDialogService, IDialogService dialogService, ILicenseKeyService licenseKeyService) : base(navigationService)
        {
            _licenseKeyService = licenseKeyService;
            _dialogService = dialogService;
            _pageDialogService = pageDialogService;
            _facebookService = facebookService;
            FilterFriendsCommand = new Command(FilterFriends);
            ConnectFacebookCommand = new Command(ConnectFacebook);
            FillterCommand = new Command<FillterFriendModel>(Fillter);
        }
        private async void FilterFriends()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (FriendsDoNotInteractData != null && FriendsDoNotInteractData.Any())
                {
                    var res = await _pageDialogService.DisplayAlertAsync("Thông báo",
                             "Chỉ nên xóa tối đa 200 bạn bè trong một ngày !", "OK", "Cancel");
                    if (res)
                    {
                        var dataFriends = FriendsDoNotInteractData.Where(x => x.IsSelected == true);
                        if (dataFriends.Any())
                        {
                            var data = dataFriends.Take(499).ToList();
                            if (data.Any())
                            {
                                var cookie = Preferences.Get(AppConstants.CookieFacebook, "");
                                var fb_para = await _facebookService.GeJazoestAndFbdtsg(cookie);
                                var fb_dtsg = fb_para.Fbdtsg;
                                var jazoest = fb_para.Jazoest;

                                if (fb_dtsg != null && jazoest != null && cookie != null)
                                {
                                    var num = 0;
                                    foreach (var item in data)
                                    {
                                        var unFriend = await _facebookService.UnFriend(fb_dtsg, jazoest, item.Uid, cookie);
                                        if (unFriend != null && unFriend.Contains(item.Uid))
                                        {
                                            num++;
                                            FriendsDoNotInteractData.Remove(item);
                                            _doNotInteract.Remove(item);
                                        }
                                    }
                                    await _pageDialogService.DisplayAlertAsync("Thông báo",
                                        $"Bạn đã xóa {num} thanh cong !", "OK");
                                }
                            }
                        }
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

        private async Task StartFillterFriend(string token, string cookie, string limit)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(cookie) && !string.IsNullOrWhiteSpace(limit))
                {
                    var data = await _facebookService.GetInfoUser();
                    if (data != null)
                    {
                        IdFacebook = data.id;
                        NameUserFacebook = data.name;
                        AvatarFacebook = data.picture?.data?.url?.Replace("\"", "");
                    }

                    await CheckFriendsReaction(cookie, token, limit);
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Lỗi", "OK");
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

        private void Fillter(FillterFriendModel fillter)
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (fillter != null)
                {
                    if (FriendsDoNotInteractData != null && FriendsDoNotInteractData.Any())
                    {
                        FriendsDoNotInteractData = new ObservableCollection<MyFriendModel>();
                        if (fillter.Id == -1)
                        {
                            foreach (var model in _doNotInteract)
                            {
                                model.IsSelected = false;
                                FriendsDoNotInteractData.Add(model);
                            }
                        }
                        else
                        {
                            foreach (var model in _doNotInteract)
                            {
                                if (model.Interactive <= fillter.Id)
                                {
                                    model.IsSelected = true;
                                    FriendsDoNotInteractData.Add(model);
                                }
                            }
                        }
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

        private async void ConnectFacebook()
        {
            try
            {
                var license = await _licenseKeyService.CheckLicenseForUser();
                if (license == null)
                {
                    await _pageDialogService.DisplayAlertAsync("Thông báo",
                        "Bạn nên nâng cấp tài khoản để sử dụng đầy đủ tính năng hơn", "OK");
                    await NavigationService.GoBackAsync();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(IdFacebook))
                    {
                        if (IsLoading) return;
                        IsLoading = true;
                        var token = Preferences.Get(AppConstants.TokenFaceook, "");
                        var cookie = Preferences.Get(AppConstants.CookieFacebook, "");
                        if (await _facebookService.CheckCookieAndToken())
                        {
                            await StartFillterFriend(token, cookie, NumberPost + "");
                        }
                        else
                        {
                            _dialogService.ShowDialog(nameof(ConnectFacebookDialog), null, async (result) =>
                            {
                                token = Preferences.Get(AppConstants.TokenFaceook, "");
                                cookie = Preferences.Get(AppConstants.CookieFacebook, "");
                                await StartFillterFriend(token, cookie, NumberPost + "");
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally { IsLoading = false; }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            IsLoading = true;
        }

        private async Task CheckFriendsReaction(string cookie, string token, string limit)
        {
            try
            {
                var myFriend = await _facebook.GetMyFriend(cookie);
                if (myFriend != null && myFriend.Any())
                {
                    var lsUid = await _facebook.GetUIdFromPost(cookie, limit, token);
                    if (lsUid != null && lsUid.Any())
                    {
                        foreach (var friend in myFriend)
                        {
                            var reaction = lsUid.Count(x => x == friend.Uid);
                            if (reaction > 0)
                            {
                                friend.Interactive = reaction;
                                friend.Status = "Có tương tác";
                            }
                            else
                            {
                                friend.Status = "Chưa tương tác";
                            }
                        }
                        FriendsDoNotInteractData = new ObservableCollection<MyFriendModel>(myFriend);
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = false;
        }
    }
}