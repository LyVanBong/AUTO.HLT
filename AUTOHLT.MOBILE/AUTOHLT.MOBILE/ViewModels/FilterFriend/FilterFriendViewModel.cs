using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.DependencyServices;
using AUTOHLT.MOBILE.Models.Facebook;
using AUTOHLT.MOBILE.Services.Facebook;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;

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

        public FilterFriendViewModel(INavigationService navigationService, IFacebookService facebookService) : base(navigationService)
        {
            _facebookService = facebookService;
            ConnectFacebookCommand = new Command(ConnectFacebook);
            IsvisibleWebView = true;
            MessagingCenter.Subscribe<App>(this, AppConstants.GetCookieAndTokenDone, async (message) =>
            {
                IsvisibleWebView = false;
                await CheckFriendsReaction();
            });
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            MessagingCenter.Unsubscribe<App>(this, AppConstants.GetCookieAndTokenDone);
        }

        private void ConnectFacebook()
        {
            //var clearCookie = DependencyService.Get<IClearCookies>().ClearAllCookies();
            //if (clearCookie)
            //{
            //    Preferences.Remove(AppConstants.CookieFacebook);
            //    Preferences.Remove(AppConstants.TokenFaceook);
            //    Preferences.Remove(AppConstants.Fb_Dtsg);
            //    FriendsDoNotInteractData.Clear();
            //    IsvisibleWebView = true;
            //    UriFacebook = AppConstants.UriLoginFacebook;
            //}
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
                var q = "node(" + IdFacebook + "){timeline_feed_units.first(500).after(){page_info,edges{node{id,creation_time,feedback{reactors{nodes{id}},commenters{nodes{id}}}}}}}";
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
                            int p = 0;
                            p = obj[IdFacebook].timeline_feed_units.edges[i].node.feedback.reactors.nodes.Length;

                            if (p > 0)
                            {
                                for (int c = 0; c < p; c++)
                                {
                                    _reaction += obj[IdFacebook].timeline_feed_units.edges[i].node.feedback.reactors.nodes[c].id +
                                            "-";
                                }
                            }

                            var pp = obj[IdFacebook].timeline_feed_units.edges[i].node.feedback.commenters.nodes.Length;
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
                    FriendsDoNotInteractData = new ObservableCollection<FriendsDoNotInteractModel>();
                    var datax = _reaction.Split('-');
                    var datay = _comment.Split('-');
                    var allFriend = await GetAllFriend();
                    var data = allFriend.data;
                    var lenght = allFriend.data.Length;
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
                            Reaction = cx + "",
                            Comment = bl + "",
                            Status = "done",
                        };

                        FriendsDoNotInteractData.Add(friend);
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            UriFacebook = AppConstants.UriLoginFacebook;

            if (Preferences.ContainsKey(AppConstants.TokenFaceook))
            {
                var data = await _facebookService.GetInfoUser("name,picture", Preferences.Get(AppConstants.TokenFaceook, ""));
                if (data != null)
                {
                    IdFacebook = data.id;
                    NameUserFacebook = data.name;
                    AvatarFacebook = data.picture.data.url.Replace("\"", "");
                }
            }
            IsLoading = false;
        }
    }
}