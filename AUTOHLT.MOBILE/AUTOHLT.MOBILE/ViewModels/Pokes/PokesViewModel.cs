using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Controls.Dialog.ConnectFacebook;
using AUTOHLT.MOBILE.Models.Facebook;
using AUTOHLT.MOBILE.Models.Product;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Facebook;
using AUTOHLT.MOBILE.Services.Product;
using AUTOHLT.MOBILE.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.Pokes
{
    public class PokesViewModel : ViewModelBase
    {
        private bool _isLoading;
        private IFacebookService _facebookService;
        private IProductService _productService;
        private string[] _packetPokesFriends = new string[] { "Gói dùng trong 30 ngày", "Gói dùng trong 180 ngày", "Gói dùng trong 365 ngày" };
        private IPageDialogService _pageDialogService;
        private IUserService _userService;
        private IDatabaseService _databaseService;
        private IDialogService _dialogService;
        private ObservableCollection<PokesFriendsModel> _pokesData;

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

        public PokesViewModel(INavigationService navigationService, IFacebookService facebookService, IProductService productService, IPageDialogService pageDialogService, IUserService userService, IDatabaseService databaseService, IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            _databaseService = databaseService;
            _userService = userService;
            _pageDialogService = pageDialogService;
            _productService = productService;
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
                    var result = await _pageDialogService.DisplayAlertAsync(Resource._1000021,
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
                            await _pageDialogService.DisplayAlertAsync(Resource._1000021, $"Bạn chọc {obj.FullName} thành công !",
                                "OK");
                        else
                            await _pageDialogService.DisplayAlertAsync(Resource._1000021, $"Bạn chọc {obj.FullName} lỗi !",
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
                                await _pageDialogService.DisplayAlertAsync(Resource._1000021, $"Bạn chọc {total} bạn bè thành công !",
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
                var pokeFr = await _facebookService.PokesFriends(cookie, obj.UId, obj.Ext, obj.Hash, fbDtsg, jazoest,obj.DomIdReplace);
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
                if (await _facebookService.CheckCookie(cookie))
                {
                    var lsPokes = new List<PokesFriendsModel>();
                    var htmlPokes = await _facebookService.GetPokesFriends(cookie, "0");
                    Regex regex = new Regex(@"<div class=""bt bu""><div><div class=""by"">(.*?)<div class=""cm""></div></div></div>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    MatchCollection matchCollection = regex.Matches(htmlPokes);
                    foreach (Match match in matchCollection)
                    {
                        var data = match.Value;
                        var poke = new PokesFriendsModel();
                        poke.FullName = Regex.Match(data, @"<a class=""cd"" href=""/+[a-zA-Z0-9\._\?=]+"">(.*?)</a>")
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
                    var result = await _pageDialogService.DisplayAlertAsync(Resource._1000021,
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
                                    if (para=="0")
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