using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Models.Product;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Product;
using AUTOHLT.MOBILE.Services.Telegram;
using AUTOHLT.MOBILE.Services.User;
using AUTOHLT.MOBILE.ViewModels;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using AUTOHLT.MOBILE.Models.Guide;
using AUTOHLT.MOBILE.Services.Guide;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.Controls.Dialog.BuffService
{
    public class BuffDialogViewModel : ViewModelBase, IDialogAware
    {
        private string _number;
        private string _price;
        private string _content;
        private string _amount;
        private bool _isEnabledLogin;
        private string _textSubmit;
        private string _idProduct;
        private ProductModel _productModel;
        private string _userName;
        private IProductService _productService;
        private IUserService _userService;
        private string _userMoney;
        private IPageDialogService _pageDialogService;
        private IDatabaseService _database;
        private ITelegramService _telegramService;
        private string _noteService;
        private string _numberService;
        private bool _isLoading;
        private IGuideService _guideService;
        public ICommand HDSDCommand { get; private set; }
        private GuideModel _guide;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string NumberService
        {
            get => _numberService;
            set => SetProperty(ref _numberService, value);
        }

        public ICommand ClosePopupCommand { get; private set; }

        public string NoteService
        {
            get => _noteService;
            set => SetProperty(ref _noteService, value);
        }

        public string UserMoney
        {
            get => _userMoney;
            set => SetProperty(ref _userMoney, value);
        }

        public ICommand UnfocusedCommand { get; private set; }

        public string TextSubmit
        {
            get => _textSubmit;
            set => SetProperty(ref _textSubmit, value);
        }

        public bool IsEnabledLogin
        {
            get => _isEnabledLogin;
            set => SetProperty(ref _isEnabledLogin, value);
        }

        public ICommand UseServiceCommand { get; private set; }

        public string Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public string Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public string Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        public BuffDialogViewModel(INavigationService navigationService, IProductService productService, IUserService userService, IPageDialogService pageDialogService, IDatabaseService databaseService, ITelegramService telegramService,IGuideService guideService) : base(navigationService)
        {
            _guideService = guideService;
            _telegramService = telegramService;
            _database = databaseService;
            _pageDialogService = pageDialogService;
            _userService = userService;
            _productService = productService;
            UseServiceCommand = new Command(UseService);
            UnfocusedCommand = new Command<string>(Unfocused);
            ClosePopupCommand = new Command(ClosePopup);
            IsLoading = true;
            HDSDCommand = new Command(HDSDApp);
        }

        private async void HDSDApp()
        {
            try
            {
                // sub
                if (_idProduct== "1ad3c424-5333-46b0-a0e8-5c31a6dbb161")
                {
                    _guide = await _guideService.GetGuide(7);
                }
                else // like
                {
                    _guide = await _guideService.GetGuide(8);
                }
                await Browser.OpenAsync(_guide?.Url);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void ClosePopup()
        {
            if (RequestClose != null)
                RequestClose(null);
        }

        private void Unfocused(string key)
        {
            if (IsLoading) return;
            IsLoading = true;
            if (key == "0")
            {
                if (Number == null) return;
                var res = (long.Parse(Price) / long.Parse(NumberService)) *
                          (long.Parse(Number));
                if (res > long.Parse(UserMoney))
                {
                    _pageDialogService.DisplayAlertAsync(Resource._1000021, "Số dư không đủ !", "OK");
                    Number = "";
                }
                else
                {
                    Amount = res + "";
                }
            }
            if (!string.IsNullOrWhiteSpace(Content) && !string.IsNullOrWhiteSpace(Number))
            {
                IsEnabledLogin = true;
            }
            else
                IsEnabledLogin = false;

            IsLoading = false;
        }

        private async void UseService()
        {
            try
            {
                if (IsLoading)
                {
                    return;
                }

                IsLoading = true;
                if (!string.IsNullOrWhiteSpace(Number) && !string.IsNullOrWhiteSpace(Content))
                {
                    var amout = long.Parse(Amount);
                    var user = await _database.GetAccountUser();
                    var userName = user?.UserName;
                    var idAccount = user?.ID;
                    if (user != null && user.UserName != null)
                    {
                        var money = long.Parse(user.Price.Replace(".0000", ""));
                        if (amout > 0 && amout <= money)
                        {
                            var data = money - amout;
                            var update = await _userService.SetMoneyUser(_userName, data + "");
                            if (update != null && update.Code > 0)
                            {
                                var num = long.Parse(Number) * long.Parse(NumberService);
                                var historyService = await _productService.AddHistoryUseService(_idProduct, Title,
                                    _userName,
                                    num + "", DateTime.Now.ToString("yyy/MM/dd hh:mm:ss"));
                                var message = $"{Title}\n" +
                                              $"Số lượng: {Number}\n" +
                                              $"Nội dung: {Content}\n" +
                                              $"Id người dùng dịch vụ: {user.ID}\n" +
                                              $"Thời yêu cầu dịch vụ: {DateTime.Now.ToString("F")}";
                                var tele = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork,
                                    message);
                                await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000040, "OK");
                            }
                            else
                            {
                                await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000041, "OK");
                            }
                        }
                        else
                        {
                            await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000036, "OK");
                            //Number = "";
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
                ClosePopup();
            }
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                if (parameters != null)
                {
                    TextSubmit = parameters.GetValue<string>("ServiceName");
                    Title = Resource._1000058 + " " + TextSubmit;
                    _idProduct = parameters.GetValue<string>("IdProduct");
                    _userName = parameters.GetValue<string>("UserName");
                    var service = parameters.GetValue<string>("Service");
                    var myMoney = await _userService.GetMoneyUser(_userName);
                    if (myMoney != null && myMoney.Code > 0)
                    {
                        UserMoney = myMoney.Data.Replace(".0000", "");
                    }
                    else
                    {
                        if (RequestClose != null)
                            RequestClose(null);
                    }

                    var data = await _productService.GetAllProduct();
                    if (data != null && data.Code > 0 && data.Data != null && data.Data.Any())
                    {
                        _productModel = data.Data.ToList().FirstOrDefault(x => x.ID == _idProduct);
                        if (_productModel != null)
                        {
                            Price = _productModel.Price;
                            NumberService = _productModel.Number;
                            NoteService = string.Format(Resource._1000090, string.Format(new CultureInfo("en-US"),
                                    "{0:0,0}",
                                    decimal.Parse(_productModel.Number)), service,
                                string.Format(new CultureInfo("en-US"), "{0:0,0}",
                                    decimal.Parse(_productModel.Price)));
                        }
                        else
                        {
                            if (RequestClose != null)
                                RequestClose(null);
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

        public event Action<IDialogParameters> RequestClose;
    }
}