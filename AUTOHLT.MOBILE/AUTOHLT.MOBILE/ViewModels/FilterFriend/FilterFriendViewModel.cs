using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.DependencyServices;
using AUTOHLT.MOBILE.Models.Facebook;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Facebook;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AUTOHLT.MOBILE.ViewModels.FilterFriend
{
    public class FilterFriendViewModel : ViewModelBase
    {
        private bool _isLoading;
        private string _uriFacebook;
        private bool _isvisibleWebView;
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
        private bool _isConnectfb;

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
        public string UriFacebook
        {
            get => _uriFacebook;
            set => SetProperty(ref _uriFacebook, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsvisibleWebView
        {
            get => _isvisibleWebView;
            set => SetProperty(ref _isvisibleWebView, value);
        }

        public FilterFriendViewModel(INavigationService navigationService, IFacebookService facebookService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _facebookService = facebookService;
            FilterFriendsCommand = new Command(FilterFriends);
            ConnectFacebookCommand = new Command(ConnectFacebook);
            FillterCommand = new Command<FillterFriendModel>(Fillter);
            IsvisibleWebView = true;
            MessagingCenter.Subscribe<App>(this, AppConstants.GetokenDone, async (message) =>
            {
                await StartFillterFriend();
            });
            MessagingCenter.Subscribe<App>(this, AppConstants.GetCookieDone, (message) =>
            {
                IsLoading = true;
                IsvisibleWebView = false;
            });
        }

        private async void FilterFriends()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (FriendsDoNotInteractData != null && FriendsDoNotInteractData.Any())
                {
                    var data = FriendsDoNotInteractData.Where(x => x.IsSelected == true);
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
                                if (unFriend != null)
                                {
                                    num++;
                                }
                            }
                            await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                                $"Bạn đã xóa {num} thanh cong !", "OK");
                            await StartFillterFriend();
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

        private async Task StartFillterFriend()
        {
            try
            {
                var token = Preferences.Get(AppConstants.TokenFaceook, "");
                var cookie = Preferences.Get(AppConstants.CookieFacebook, "");
                if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(cookie))
                {
                    var data = await _facebookService.GetInfoUser("name,picture", token);
                    if (data != null)
                    {
                        IdFacebook = data.id;
                        NameUserFacebook = data.name;
                        AvatarFacebook = data.picture?.data?.url?.Replace("\"", "");
                    }

                    await CheckFriendsReaction();
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync(Resource._1000021, Resource._1000040, "OK");
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                IsLoading = false;
                _isConnectfb = false;
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
                            _doNotInteract.ForEach(model =>
                            {
                                model.IsSelected = false;
                                FriendsDoNotInteractData.Add(model);
                            });
                        }
                        else
                        {
                            _doNotInteract.ForEach(model =>
                            {
                                if ((model.Comment + model.Reaction) <= fillter.Id)
                                {
                                    model.IsSelected = true;
                                    FriendsDoNotInteractData.Add(model);
                                }
                            });
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

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            MessagingCenter.Unsubscribe<App>(this, AppConstants.GetokenDone);
        }

        private void ConnectFacebook()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(IdFacebook))
                {
                    if (_isConnectfb) return;
                    _isConnectfb = true;
                    Preferences.Remove(AppConstants.CookieFacebook);
                    Preferences.Remove(AppConstants.TokenFaceook);
                    Preferences.Remove(AppConstants.Fb_Dtsg);
                    Preferences.Remove(AppConstants.Jazoest);
                    UriFacebook = AppConstants.UriLoginFacebook;
                    IsvisibleWebView = true;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            IsLoading = true;
        }

        private async Task<bool> GetFriendReaction()
        {
            try
            {
                var q = "node(" + IdFacebook + "){timeline_feed_units.first(5000).after(){page_info,edges{node{id,creation_time,feedback{reactors{nodes{id}},commenters{nodes{id}}}}}}}";
                var data = await _facebookService.GetFriendsDoNotInteract(Preferences.Get(AppConstants.Fb_Dtsg, ""), q);
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

        private async Task<FriendsModel> GetAllFriend()
        {
            try
            {
                var data = await _facebookService.GetAllFriend("5000", Preferences.Get(AppConstants.TokenFaceook, ""));
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            return null;
        }

        private async Task CheckFriendsReaction()
        {
            try
            {

                var reac = await GetFriendReaction();
                if (reac)
                {
                    _doNotInteract = new List<FriendsDoNotInteractModel>();
                    var datax = _reaction.Split('-');
                    var datay = _comment.Split('-');
                    var allFriend = await GetAllFriend();
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