using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Models.Facebook;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Facebook;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTOHLT.MOBILE.Controls.Dialog.ConnectFacebook;
using AUTOHLT.MOBILE.Helpers;
using AUTOHLT.MOBILE.Services.Guide;
using Prism.Services.Dialogs;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.FilterFriend
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
        private ObservableCollection<FriendsDoNotInteractModel> _friendsDoNotInteractData;

        private List<FillterFriendModel> _fillterFriendModels = new List<FillterFriendModel>
        {
            new FillterFriendModel{Id = 0,NameFilter = Resource._1000098},
            new FillterFriendModel{Id = 5,NameFilter = "<5"},
            new FillterFriendModel{Id = 10,NameFilter = "<10"},
            new FillterFriendModel{Id = 20,NameFilter = "<20"},
            new FillterFriendModel{Id = 30,NameFilter = "<30"},
            new FillterFriendModel{Id = 50,NameFilter = "<50"},
            new FillterFriendModel{Id = -1,NameFilter = Resource._1000099},
        };

        private string _notifillterDone;
        private IPageDialogService _pageDialogService;
        private List<FriendsDoNotInteractModel> _doNotInteract;
        private IDialogService _dialogService;
        private IGuideService _guideService;

        public ICommand FilterFriendsCommand { get; private set; }
        public string NotifillterDone
        {
            get => _notifillterDone;
            set => SetProperty(ref _notifillterDone, value);
        }

        public ICommand FillterCommand { get; private set; }
        public List<FillterFriendModel> FillterFriendModels
        {
            get => _fillterFriendModels;
            set => SetProperty(ref _fillterFriendModels, value);
        }

        public ObservableCollection<FriendsDoNotInteractModel> FriendsDoNotInteractData
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

        public ICommand HDSDCommand { get; private set; }
        public FilterFriendViewModel(INavigationService navigationService, IFacebookService facebookService, IPageDialogService pageDialogService, IDialogService dialogService,IGuideService guideService) : base(navigationService)
        {
            _guideService = guideService;
            _dialogService = dialogService;
            _pageDialogService = pageDialogService;
            _facebookService = facebookService;
            FilterFriendsCommand = new Command(FilterFriends);
            ConnectFacebookCommand = new Command(ConnectFacebook);
            FillterCommand = new Command<FillterFriendModel>(Fillter);
            HDSDCommand = new Command(HDSDApp);
        }
        private async void HDSDApp()
        {
            try
            {
                var data = await _guideService.GetGuide(11);
                await Browser.OpenAsync(data?.Url);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        private async void FilterFriends()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (FriendsDoNotInteractData != null && FriendsDoNotInteractData.Any())
                {
                    var res = await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                             "Chỉ nên xóa tối đa 200 bạn bè trong một ngày !", "OK", "Cancel");
                    if (res)
                    {
                        var dataFriends = FriendsDoNotInteractData.Where(x => x.IsSelected == true);
                        if (dataFriends.Any())
                        {
                            var data = dataFriends.Take(499).ToList();
                            if (data.Any())
                            {
                                var fb_dtsg = Preferences.Get(AppConstants.Fb_Dtsg, "");
                                var jazoest = Preferences.Get(AppConstants.Jazoest, "");
                                var cookie = Preferences.Get(AppConstants.CookieFacebook, "");
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
                                    await _pageDialogService.DisplayAlertAsync(Resource._1000021,
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

        private async Task StartFillterFriend(string token, string cookie, string fbdtsg)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(cookie) && !string.IsNullOrWhiteSpace(fbdtsg))
                {
                    new Thread(async () => await FacebookHelper.UseServiceFacebookFree("Filter")).Start();
                    var data = await _facebookService.GetInfoUser("name,picture", token);
                    if (data != null)
                    {
                        IdFacebook = data.id;
                        NameUserFacebook = data.name;
                        AvatarFacebook = data.picture?.data?.url?.Replace("\"", "");
                    }

                    await CheckFriendsReaction(token, fbdtsg);
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync(Resource._1000021, Resource._1000041, "OK");
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
                        FriendsDoNotInteractData = new ObservableCollection<FriendsDoNotInteractModel>();
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
                                if ((model.Comment + model.Reaction) <= fillter.Id)
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
                if (string.IsNullOrWhiteSpace(IdFacebook))
                {
                    if (IsLoading) return;
                    IsLoading = true;
                    var token = Preferences.Get(AppConstants.TokenFaceook, "");
                    var cookie = Preferences.Get(AppConstants.CookieFacebook, "");
                    var fbdtsg = await _facebookService.GeJazoestAndFbdtsg(cookie);
                    if (await CheckCookieAndToken(token, cookie))
                    {
                        await StartFillterFriend(token, cookie, fbdtsg.Fbdtsg);
                    }
                    else
                    {
                        _dialogService.ShowDialog(nameof(ConnectFacebookDialog), null, async (result) =>
                         {
                             token = Preferences.Get(AppConstants.TokenFaceook, "");
                             cookie = Preferences.Get(AppConstants.CookieFacebook, "");
                             fbdtsg = await _facebookService.GeJazoestAndFbdtsg(cookie);
                             await StartFillterFriend(token, cookie, fbdtsg.Fbdtsg);
                         });
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally { IsLoading = false; }
        }

        private async Task<bool> CheckCookieAndToken(string token, string cookie)
        {
            try
            {
                var isCookie = await _facebookService.CheckCookie(cookie);
                if (!isCookie)
                {
                    return false;
                }

                var friends = await GetAllFriend(token, "1");
                if (friends == null || friends.data == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return false;
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            IsLoading = true;
        }

        private async Task<bool> GetFriendReaction(string fbdtsg)
        {
            try
            {
                var q = "node(" + IdFacebook + "){timeline_feed_units.first(100).after(){page_info,edges{node{id,creation_time,feedback{reactors{nodes{id}},commenters{nodes{id}}}}}}}";
                var data = await _facebookService.GetFriendsDoNotInteract(fbdtsg, q);
                if (data != null)
                {
                    var obj = JsonConvert.DeserializeObject<Dictionary<string, ReactionModel>>(data);
                    if (obj != null)
                    {
                        _comment = "";
                        _reaction = "";
                        var id = IdFacebook;
                        var lenght = obj[IdFacebook].timeline_feed_units.edges.Length - 1;
                        for (int i = 0; i < lenght; i++)
                        {
                            int? p = 0;
                            p = obj[IdFacebook]?.timeline_feed_units?.edges[i]?.node?.feedback?.reactors?.nodes?.Length;

                            if (p > 0)
                            {
                                for (int c = 0; c < p; c++)
                                {
                                    _reaction += obj[IdFacebook].timeline_feed_units.edges[i].node.feedback.reactors.nodes[c].id +
                                            "-";
                                }
                            }

                            int? pp = obj[IdFacebook]?.timeline_feed_units?.edges[i]?.node?.feedback?.commenters?.nodes?.Length;
                            if (pp > 0)
                            {
                                for (int cc = 0; cc < pp; cc++)
                                {
                                    _comment += obj[IdFacebook].timeline_feed_units.edges[i].node.feedback.commenters.nodes[cc].id +
                                           "-";
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return false;
            }
        }

        private async Task<FriendsModel> GetAllFriend(string token, string numberFriends)
        {
            try
            {
                var data = await _facebookService.GetAllFriend(numberFriends, token);
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            return null;
        }

        private async Task CheckFriendsReaction(string token, string fbdtsg)
        {
            try
            {

                var reac = await GetFriendReaction(fbdtsg);
                if (reac)
                {
                    var allFriend = await GetAllFriend(token, "5000");
                    if (allFriend != null)
                    {
                        _doNotInteract = new List<FriendsDoNotInteractModel>();
                        var datax = _reaction.Split('-');
                        var datay = _comment.Split('-');
                        var data = allFriend.data;
                        var lenght = allFriend.data.Length;
                        var countFriendNoReaction = 0;
                        for (int i = 0; i < lenght; i++)
                        {
                            var cx = 0;
                            var bl = 0;
                            var idx = allFriend.data[i].id;
                            if (_reaction.Contains(idx))
                            {
                                foreach (var item in datax)
                                {
                                    if (idx == item)
                                    {
                                        cx++;
                                    }
                                }
                            }

                            if (_comment.Contains(idx))
                            {
                                foreach (var item in datay)
                                {
                                    if (item == idx)
                                    {
                                        bl++;
                                    }
                                }
                            }


                            var friend = new FriendsDoNotInteractModel
                            {
                                Id = i + 1 + "",
                                Uid = data[i].id,
                                Name = data[i].name,
                                Reaction = cx,
                                Comment = bl,
                                Status = Resource._1000096,
                                Picture = data[i]?.picture?.data?.url,
                            };
                            if (cx == 0 && bl == 0)
                            {
                                countFriendNoReaction++;
                            }
                            _doNotInteract.Add(friend);
                        }
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            FriendsDoNotInteractData = new ObservableCollection<FriendsDoNotInteractModel>(_doNotInteract);
                            NotifillterDone = string.Format(Resource._1000097, countFriendNoReaction, lenght);
                        });
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