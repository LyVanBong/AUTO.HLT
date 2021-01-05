using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
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
using Unity.Injection;
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
                if (obj != null)
                {
                    var cookie = Preferences.Get(AppConstants.CookieFacebook, "");
                    var html = await _facebookService.PokesFriends(cookie, obj.UriPokes);
                    if (html.Contains(obj.FullName))
                    {
                        await _pageDialogService.DisplayAlertAsync(Resource._1000021, $"Bạn chọc {obj.FullName} thành công !",
                              "OK");
                        PokesData.Remove(obj);
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync(Resource._1000021, Resource._1000041,
                            "OK");
                    }
                }
                else
                {
                    if (PokesData.Any())
                    {
                        var data = PokesData.Where(x => x.IsPokes).ToList();
                        if (data!= null && data.Any())
                        {
                            var total = 0;
                            var cookie = Preferences.Get(AppConstants.CookieFacebook, "");
                            foreach (var item in data)
                            {
                                var html = await _facebookService.PokesFriends(cookie, item.UriPokes);
                                if (html.Contains(item.FullName))
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
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                IsLoading = false;
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
                var product = await _productService.GetAllProduct();
                if (product != null && product.Code > 0 && product.Data != null && product.Data.Any())
                {
                    var user = await _databaseService.GetAccountUser();
                    if (user != null && user.UserName != null)
                    {
                        var pokes = product.Data.Where(x => x.GroupProduct == "9").ToList();
                        var regisProdduct = await _productService.GetListRegisterProductForUser(user.ID);
                        if (regisProdduct != null && regisProdduct.Code > 0 && regisProdduct.Data != null &&
                            regisProdduct.Data.Any())
                        {
                            var regis = regisProdduct.Data.ToList();
                            foreach (var item in pokes)
                            {
                                var obj = regis.FirstOrDefault(x => x.ID_ProductType == item.ID);
                                if (obj != null && obj.DateCreate != null)
                                {
                                    var date = (DateTime.Now - DateTime.Parse(obj.DateCreate)).TotalDays;
                                    var endDate = double.Parse(item.EndDate);
                                    if (date <= endDate)
                                    {
                                        await UseServiceProduct();
                                        return;
                                    }
                                }
                            }

                            await RegisterProduct(pokes, user);
                        }
                        else
                        {
                            await RegisterProduct(pokes, user);
                        }
                    }
                    else
                    {
                        await NavigationService.GoBackAsync();
                    }
                }
                else
                {
                    await NavigationService.GoBackAsync();
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

        private async Task RegisterProduct(List<ProductModel> pokes, UserModel user)
        {
            try
            {
                if (pokes != null && pokes.Any())
                {
                    var result =
                        await _pageDialogService.DisplayActionSheetAsync("Bạn đăng ký gọi ?", "Cancel", null,
                            _packetPokesFriends);
                    if (_packetPokesFriends.Contains(result))
                    {
                        foreach (var item in pokes)
                        {
                            if (result.Contains(item.EndDate))
                            {
                                var price = long.Parse(item.Price);
                                var res = await _pageDialogService.DisplayAlertAsync(Resource._1000021, $"bạn đăng ký {result} chọc bạn bè với giá {string.Format(new CultureInfo("en-US"), "{0:0,0}", price)} VND !", "OK", "Cancel");
                                if (res)
                                {
                                    var registerProduct = await _productService.RegisterProduct(item.ID, user.ID);
                                    if (registerProduct != null && registerProduct.Code > 0 && registerProduct.Data != null)
                                    {
                                        var money = await _userService.GetMoneyUser(user.UserName);
                                        if (money != null && money.Code > 0 && money.Data != null)
                                        {
                                            var myMoney = long.Parse(money.Data.Replace(".0000", ""));
                                            if (price <= myMoney)
                                            {
                                                var updateUser = await _userService.SetMoneyUser(user.UserName, myMoney - price + "");
                                                if (updateUser != null && updateUser.Code > 0)
                                                {
                                                    await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000040,
                                                        "OK");
                                                }
                                                else
                                                {
                                                    await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000041,
                                                        "OK");
                                                }
                                            }
                                            else
                                            {
                                                await _pageDialogService.DisplayAlertAsync(Resource._1000035, "Số dư hiện tại của bạn không đủ để thực hiện giao dịch này !",
                                                       "OK");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    await NavigationService.GoBackAsync();
                                }
                                return;
                            }
                        }
                    }
                    else
                    {
                        await NavigationService.GoBackAsync();
                    }
                }
                else
                {
                    await NavigationService.GoBackAsync();
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
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
                    Regex regex = new Regex(@"(<div class=""br"" id=""poke_live_item_).*?(</div></div></div><div class=""cl""></div></div></div></div></div>)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                    MatchCollection matchCollection = regex.Matches(htmlPokes);
                    foreach (Match match in matchCollection)
                    {
                        var data = match.Value;
                        var uidReg = new Regex(@"<div class=""br"" id=""poke_live_item_(.*?)"">");
                        var regexName = new Regex(@"<a class=""cc"" href=""/+[a-zA-Z0-9\.]+"">(.*?)</a>");
                        var regexAvatar = new Regex(@"<img src=""(.*?)"" class=""bz s"" alt=""");
                        var regexUri = new Regex(@"/pokes/inline/\?dom_id_replace(.*?)""");

                        lsPokes.Add(new PokesFriendsModel
                        {
                            UId = uidReg.Match(data)?.Groups[1]?.Value,
                            FullName = regexName.Match(data)?.Groups[1]?.Value,
                            Avatar = regexAvatar.Match(data)?.Groups[1]?.Value,
                            UriPokes = "https://d.facebook.com/pokes/inline/?dom_id_replace" + regexUri.Match(data)?.Groups[1]?.Value,
                            IsPokes = false,
                        });
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
                            await UseServiceProduct();
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