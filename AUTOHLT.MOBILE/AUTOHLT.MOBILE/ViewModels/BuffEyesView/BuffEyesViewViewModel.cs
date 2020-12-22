using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTOHLT.MOBILE.Controls.Dialog.UseService;
using AUTOHLT.MOBILE.Models.Product;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Product;
using AUTOHLT.MOBILE.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.BuffEyesView
{
    public class BuffEyesViewViewModel : ViewModelBase
    {
        private IProductService _productService;
        private bool _isLoading;
        private ProductModel _like1Year;
        private ProductModel _like1Forever;
        private ProductModel _like2Year;
        private ProductModel _like2Forever;
        private IDatabaseService _databaseService;
        private UserModel _userModel;
        private IPageDialogService _pageDialogService;
        private IUserService _userService;
        private IDialogService _dialogService;

        public ICommand LikeUseServiceCommand { get; private set; }
        /// <summary>
        /// goij like 1 400 thoi han vinh vien
        /// </summary>
        public ProductModel Like2Forever
        {
            get => _like2Forever;
            set => SetProperty(ref _like2Forever, value);
        }

        /// <summary>
        /// goi like 1 400 like thoi han 1 nam
        /// </summary>
        public ProductModel Like2year
        {
            get => _like2Year;
            set => SetProperty(ref _like2Year, value);
        }
        /// <summary>
        /// goij like 1 300 thoi han vinh vien
        /// </summary>
        public ProductModel Like1Forever
        {
            get => _like1Forever;
            set => SetProperty(ref _like1Forever, value);
        }

        /// <summary>
        /// goi like 1 300 like thoi han 1 nam
        /// </summary>
        public ProductModel Like1year
        {
            get => _like1Year;
            set => SetProperty(ref _like1Year, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public BuffEyesViewViewModel(INavigationService navigationService, IProductService productService, IDatabaseService databaseService, IPageDialogService pageDialogService, IUserService userService, IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            _userService = userService;
            _pageDialogService = pageDialogService;
            _databaseService = databaseService;
            _productService = productService;
            IsLoading = true;
            LikeUseServiceCommand = new Command<ProductModel>(LikeUseService);
        }

        private async void LikeUseService(ProductModel product)
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                // Đã đang kỹ dịch vụ này tiến hành sử dụng
                if (product.IsRegisterProduct)
                {
                    var id = _userModel.ID;
                    var number = int.Parse(product.Number);
                    var num = 0;
                    var data = await _productService.GetHistoryUseServiceForUser(id);
                    if (data != null && data.Code > 0 && data.Data != null && data.Data.Any())
                    {
                        var lsHistoryUserService = data.Data.ToList()
                            .Where(x => x.ID_ProductType == product.ID && DateTime.Parse(x.DateCreate).Date == DateTime.Now.Date).ToList();
                        if (lsHistoryUserService.Any())
                        {
                            foreach (var item in lsHistoryUserService)
                            {
                                num += int.Parse(item.Number);
                            }
                        }
                    }

                    if (num >= number)
                    {
                        await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000061, "OK");
                    }
                    else
                    {
                        var para = new DialogParameters();
                        para.Add("IdProduct", product.ID);
                        para.Add("IdUser", id);
                        para.Add("Number", (number - num).ToString());
                        para.Add("Title", Resource._1000063);
                        await _dialogService.ShowDialogAsync(nameof(UseServiceDialog), para);
                    }
                }
                else // Chưa đang kỹ dịch vụ bắt đầu đăng ký lại
                {
                    var username = _userModel.UserName;
                    var idUser = _userModel.ID;
                    var moneyModel = await _userService.GetMoneyUser(username);
                    var money = long.Parse(moneyModel.Data.Replace(".0000", ""));
                    var price = long.Parse(product.Price);
                    if (money >= price)
                    {

                        var messager = string.Format(Resource._1000057, string.Format(new CultureInfo("en-US"), "{0:0,0}", long.Parse(product.Number)), Resource._1000063, product.TmpEndDate, string.Format(new CultureInfo("en-US"), "{0:0,0}", long.Parse(product.Price)));
                        var res = await _pageDialogService.DisplayAlertAsync(Resource._1000035, messager, "OK",
                            "Cancel");
                        if (res)
                        {
                            if (moneyModel != null && moneyModel.Code > 0 && moneyModel.Data != null)
                            {

                                var registerProduct = await _productService.RegisterProduct(product.ID, idUser);
                                if (registerProduct != null && registerProduct.Code > 0 && registerProduct.Data != null)
                                {
                                    var user = new UserModel
                                    {
                                        UserName = _userModel.UserName,
                                        Name = _userModel.Name,
                                        Password = _userModel.Password,
                                        Email = _userModel.Email,
                                        NumberPhone = _userModel.NumberPhone,
                                        Sex = _userModel.Sex,
                                        Role = _userModel.Role,
                                        IsActive = _userModel.IsActive,
                                        Age = _userModel.Age,
                                        Price = money - price + "",
                                        IdDevice = _userModel.IdDevice
                                    };
                                    var updateUser = await _userService.UpdateUser(user.UserName, user.Name, user.Password, user.Email, user.NumberPhone.ToString(), user.Sex.ToString(), user.Role.ToString(), user.IsActive.ToString(), user.Age.ToString(), user.Price.ToString(), user.IdDevice);
                                    if (updateUser != null && updateUser.Code > 0)
                                    {
                                        await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000040,
                                            "OK");
                                        await InitializeData();
                                    }
                                    else
                                    {
                                        await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000041,
                                            "OK");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync(Resource._1000035, moneyModel.Message, "OK");
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
                _userModel = await _databaseService.GetAccountUser();
                if (_userModel != null)
                {
                    var lsProduct = await _productService.GetAllProduct();
                    if (lsProduct != null && lsProduct.Code > 0 && lsProduct.Data != null && lsProduct.Data.Any())
                    {
                        Like1year = lsProduct.Data.FirstOrDefault(x => x.ID == "c81cd058-8374-4c3f-9de7-4002d0d46ee0");
                        Like1Forever = lsProduct.Data.FirstOrDefault(x => x.ID == "72235dda-5689-4b88-ac7e-0a82143e45d6");
                        Like2year = lsProduct.Data.FirstOrDefault(x => x.ID == "9ec8a62e-ee56-4c8f-ace1-89bc20296bfa");
                        Like2Forever = lsProduct.Data.FirstOrDefault(x => x.ID == "4f1946ed-913f-431b-97ef-a3c5d5094708");
                    }

                    var lsRegisterProduct = await _productService.GetListRegisterProductForUser(_userModel.ID);
                    if (lsRegisterProduct != null && lsRegisterProduct.Code > 0 && lsRegisterProduct.Data != null && lsRegisterProduct.Data.Any())
                    {
                        foreach (var item in lsRegisterProduct.Data)
                        {
                            switch (item.ID_ProductType)
                            {
                                case "c81cd058-8374-4c3f-9de7-4002d0d46ee0":
                                    var dataEnd = DateTime.Parse(item.DateCreate).Add(TimeSpan.FromDays(365));
                                    var dateNow = DateTime.Now;
                                    if (dateNow < dataEnd)
                                    {
                                        Like1year.IsRegisterProduct = true;
                                        RaisePropertyChanged(nameof(Like1year));
                                    }
                                    break;
                                case "72235dda-5689-4b88-ac7e-0a82143e45d6":
                                    Like1Forever.IsRegisterProduct = true;
                                    RaisePropertyChanged(nameof(Like1Forever));
                                    break;
                                case "9ec8a62e-ee56-4c8f-ace1-89bc20296bfa":
                                    var dataEnd2 = DateTime.Parse(item.DateCreate).Add(TimeSpan.FromDays(365));
                                    var dateNow2 = DateTime.Now;
                                    if (dateNow2 < dataEnd2)
                                    {
                                        Like2year.IsRegisterProduct = true;
                                        RaisePropertyChanged(nameof(Like2year));
                                    }

                                    break;
                                case "4f1946ed-913f-431b-97ef-a3c5d5094708":
                                    Like2Forever.IsRegisterProduct = true;
                                    RaisePropertyChanged(nameof(Like2Forever));
                                    break;
                                default:
                                    break;
                            }
                        }
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