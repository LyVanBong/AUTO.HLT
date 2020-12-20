using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Models.Product;
using AUTOHLT.MOBILE.Models.User;
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
using System.Linq;
using System.Windows.Input;
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

        public BuffDialogViewModel(INavigationService navigationService, IProductService productService, IUserService userService, IPageDialogService pageDialogService, IDatabaseService databaseService, ITelegramService telegramService) : base(navigationService)
        {
            _telegramService = telegramService;
            _database = databaseService;
            _pageDialogService = pageDialogService;
            _userService = userService;
            _productService = productService;
            UseServiceCommand = new Command(UseService);
            UnfocusedCommand = new Command<string>(Unfocused);
        }

        private void Unfocused(string key)
        {
            if (key == "0")
            {
                if (Number == null) return;
                var res = long.Parse(Price) * long.Parse(Number);
                if (res > int.Parse(UserMoney))
                {
                    var a = int.Parse(UserMoney) / int.Parse(Price);
                    Amount = a * int.Parse(Price) + "";
                    Number = a + "";
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
        }

        private async void UseService()
        {
            if (!string.IsNullOrWhiteSpace(Number) && !string.IsNullOrWhiteSpace(Content))
            {
                var amout = long.Parse(Amount);
                var user = await _database.GetAccountUser();
                if (amout > 0 && amout <= long.Parse(user.Price))
                {
                    var data = new UserModel
                    {
                        UserName = user.UserName,
                        Name = user.Name,
                        Password = user.Password,
                        Email = user.Email,
                        NumberPhone = user.NumberPhone,
                        Sex = user.Sex,
                        Role = user.Role,
                        IsActive = user.IsActive,
                        Age = user.Age,
                        Price = long.Parse(user.Price) - amout + "",
                        IdDevice = user.IdDevice,
                    };
                    var update = await _userService.UpdateUser(data.UserName, data.Name, data.Password,
                        data.Email, data.NumberPhone.ToString(), data.Sex.ToString(), data.Role.ToString(),
                        data.IsActive.ToString(), data.Age.ToString(), data.Price.ToString(), data.IdDevice);
                    if (update != null && update.Code > 0)
                    {
                        await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000040, "OK");
                        Number = "";
                        var myMoney = await _userService.GetMoneyUser(_userName);
                        if (myMoney != null && myMoney.Code > 0)
                        {
                            UserMoney = myMoney.Data;
                        }
                        else
                        {
                            if (RequestClose != null)
                                RequestClose(null);
                        }
                        var message = $"{Title}\n" +
                                      $"Số lượng: {Number}\n" +
                                      $"Nội dung: {Content}\n" +
                                      $"Id người dùng dịch vụ: {user.ID}\n" +
                                      $"Thời yêu cầu dịch vụ: {DateTime.Now.ToString("F")}";
                        var tele = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork,
                            message);
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000041, "OK");
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000036, "OK");
                    Number = "";
                }
            }
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters != null)
            {
                TextSubmit = parameters.GetValue<string>("ServiceName");
                Title = Resource._1000058 + " " + TextSubmit;
                _idProduct = parameters.GetValue<string>("IdProduct");
                _userName = parameters.GetValue<string>("UserName");
                var myMoney = await _userService.GetMoneyUser(_userName);
                if (myMoney != null && myMoney.Code > 0)
                {
                    UserMoney = myMoney.Data;
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
                    }
                    else
                    {
                        if (RequestClose != null)
                            RequestClose(null);
                    }
                }
            }
        }

        public event Action<IDialogParameters> RequestClose;
    }
}