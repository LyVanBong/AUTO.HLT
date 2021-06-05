using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Controls.ConnectFacebook;
using AUTO.HLT.MOBILE.VIP.Models.Facebook;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.Services.Facebook;
using AUTO.HLT.MOBILE.VIP.Services.LicenseKey;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTO.HLT.MOBILE.VIP.Controls.GoogleAdmob;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.Pokes
{
    public class PokesViewModel : ViewModelBase
    {
        private bool _isLoading;
        private IFacebookService _facebookService;
        private IPageDialogService _pageDialogService;
        private IDialogService _dialogService;
        private ObservableCollection<PokesFriendsModel> _pokesData;
        private ContentView _adModView;
        public ContentView AdModView
        {
            get => _adModView;
            set => SetProperty(ref _adModView, value);
        }
        public ICommand SelectAllFriendsCommand { get; private set; }
        public ICommand PokesFriendCommand { get; private set; }
        public ObservableCollection<PokesFriendsModel> PokesData
        {
            get => _pokesData;
            set => SetProperty(ref _pokesData, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public PokesViewModel(INavigationService navigationService, IFacebookService facebookService, IPageDialogService pageDialogService, IDatabaseService databaseService, IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            _pageDialogService = pageDialogService;
            _facebookService = facebookService;
            PokesFriendCommand = new Command<PokesFriendsModel>(PokesFriend);
            SelectAllFriendsCommand = new Command(SelectAllFriends);
        }

        private void SelectAllFriends()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (PokesData.Any())
                {
                    foreach (var item in PokesData)
                    {
                        item.IsPokes = !item.IsPokes;
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

        private async void PokesFriend(PokesFriendsModel obj)
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                var cookie = Preferences.Get(AppConstants.CookieFacebook, "");
                var paraFb = await _facebookService.GetParamaterFacebook(cookie);
                var fbDtsg = Regex.Match(paraFb, @"><input type=""hidden"" name=""fb_dtsg"" value=""(.*?)""")?.Groups[1]?.Value;
                var jazoest = Regex.Match(paraFb, @"/><input type=""hidden"" name=""jazoest"" value=""(.*?)""")?.Groups[1]?.Value;
                if (string.IsNullOrWhiteSpace(cookie) && string.IsNullOrWhiteSpace(fbDtsg) && string.IsNullOrWhiteSpace(jazoest))
                {
                    var result = await _pageDialogService.DisplayAlertAsync("Thông báo",
                        "Bạn cần kết nối với facebook của mình để sử dụng tinh năng này !", "OK", "Cancel");
                    if (result)
                    {
                        _dialogService.ShowDialog(nameof(ConnectFacebookDialog), null, async (res) =>
                        {
                            await UseServiceProduct();
                        });
                    }
                }
                else
                {
                    if (obj != null)
                    {
                        var res = await PokesMyFriend(obj, cookie, fbDtsg, jazoest);
                        if (res)
                            await _pageDialogService.DisplayAlertAsync("Thông báo", $"Bạn chọc {obj.FullName} thành công !",
                                "OK");
                        else
                            await _pageDialogService.DisplayAlertAsync("Thông báo", $"Bạn chọc {obj.FullName} lỗi !",
                                "OK");
                    }
                    else
                    {
                        if (PokesData.Any())
                        {
                            var data = PokesData.Where(x => x.IsPokes).ToList();
                            if (data.Any())
                            {
                                var total = 0;
                                foreach (var item in data)
                                {
                                    var res = await PokesMyFriend(item, cookie, fbDtsg, jazoest);
                                    if (res)
                                    {
                                        total++;
                                        PokesData.Remove(item);
                                    }
                                }
                                await _pageDialogService.DisplayAlertAsync("Thông báo", $"Bạn chọc {total} bạn bè thành công !",
                                    "OK");
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

        private async Task<bool> PokesMyFriend(PokesFriendsModel obj, string cookie, string fbDtsg, string jazoest)
        {
            try
            {
                var pokeFr = await _facebookService.PokesFriends(cookie, obj.UId, obj.Ext, obj.Hash, fbDtsg, jazoest, obj.DomIdReplace);
                if (pokeFr.Contains("mbasic_logout_button"))
                {
                    PokesData.Remove(obj);
                    return true;
                }
                else
                    return false;
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            InitializeData();

            if (parameters != null && parameters.ContainsKey(AppConstants.AddAdmod))
            {
                AdModView = new GoogleAdmobView() { HeightRequest = 150 };
                if (Device.RuntimePlatform == Device.iOS)
                    AdModView.Padding = new Thickness(0, 0, 0, 20);
            }
        }

        private async void InitializeData()
        {
            try
            {
                await UseServiceProduct();
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
        private async Task UseServiceProduct()
        {
            try
            {
                var cookie = Preferences.Get(AppConstants.CookieFacebook, "");
                if (await _facebookService.CheckCookieAndToken())
                {
                    var lsPokes = new List<PokesFriendsModel>();
                    var htmlPokes = await _facebookService.GetPokesFriends(cookie, "0");
                    Regex regex = new Regex(@"<div class=""br"" id="".*?></div></div><div class=""cl""></div></div></div></div></div>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    MatchCollection matchCollection = regex.Matches(htmlPokes);
                    foreach (Match match in matchCollection)
                    {
                        var data = match.Value;
                        var poke = new PokesFriendsModel();
                        poke.FullName = Regex.Match(data, @"<a class=""cc"" href="".*?"">(.*?)</a>")
                            ?.Groups[1]?.Value;
                        poke.IsPokes = false;
                        var uri = Regex.Match(data, @"/pokes/inline/\?dom_id_replace(.*?)""")?.Groups[1]?.Value;
                        poke.Ext = Regex.Match(uri, @";ext=(.*?)&")?.Groups[1]?.Value;
                        poke.Hash = Regex.Match($"{uri}\"", @";hash=(.*?)""")?.Groups[1]?.Value;
                        poke.UId = Regex.Match(uri, @";poke_target=(.*?)&")?.Groups[1]?.Value;
                        poke.DomIdReplace = Regex.Match(uri, @"=(.*?)&amp;is_hide")?.Groups[1]?.Value;
                        lsPokes.Add(poke);
                    }

                    PokesData = new ObservableCollection<PokesFriendsModel>(lsPokes);
                }
                else
                {
                    var result = await _pageDialogService.DisplayAlertAsync("Thông báo",
                        "Bạn cần kết nối với facebook của mình để sử dụng tinh năng này !", "OK", "Cancel");
                    if (result)
                    {
                        _dialogService.ShowDialog(nameof(ConnectFacebookDialog), null, async (res) =>
                        {
                            if (res.Parameters != null)
                            {
                                var para = res.Parameters.GetValue<string>("ConnectFacebookDone");
                                if (para == null)
                                {
                                    await NavigationService.GoBackAsync();
                                }
                                else
                                {
                                    if (para == "0")
                                    {
                                        await NavigationService.GoBackAsync();
                                    }
                                    else if (para == "1")
                                    {
                                        await UseServiceProduct();
                                    }
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}