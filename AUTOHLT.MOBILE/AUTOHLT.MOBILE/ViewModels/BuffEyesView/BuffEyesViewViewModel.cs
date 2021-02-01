using AUTOHLT.MOBILE.Controls.Dialog.UseService;
using AUTOHLT.MOBILE.Models.Product;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Product;
using AUTOHLT.MOBILE.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTOHLT.MOBILE.Services.Guide;
using Xamarin.Essentials;
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
        private IPageDialogService _pageDialogService;
        private IUserService _userService;
        private IDialogService _dialogService;
        private List<ListRegisterProductModel> _regis;
        private List<ProductModel> _productData;
        private IGuideService _guideService;
        public List<ProductModel> ProductData
        {
            get => _productData;
            set => SetProperty(ref _productData, value);
        }

        public ICommand BuffViewEyeCommand { get; private set; }
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

        public ICommand HDSDCommand { get; private set; }
        public BuffEyesViewViewModel(INavigationService navigationService, IProductService productService, IDatabaseService databaseService, IPageDialogService pageDialogService, IUserService userService, IDialogService dialogService,IGuideService guideService) : base(navigationService)
        {
            _guideService = guideService;
            _dialogService = dialogService;
            _userService = userService;
            _pageDialogService = pageDialogService;
            _databaseService = databaseService;
            _productService = productService;
            IsLoading = true;
            BuffViewEyeCommand = new Command<ProductModel>(BuffViewEye);
            HDSDCommand = new Command(HDSDApp);
        }
        private async void HDSDApp()
        {
            try
            {
                var data = await _guideService.GetGuide(2);
                await Browser.OpenAsync(data?.Url);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        private async void BuffViewEye(ProductModel product)
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                if (product != null)
                {
                    var user = await _databaseService.GetAccountUser();
                    if (product.IsRegisterProduct)
                    {
                        var id = user.ID;
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
                    else
                    {

                        var username = user.UserName;
                        var idUser = user.ID;
                        var moneyModel = await _userService.GetMoneyUser(username);
                        var money = long.Parse(moneyModel.Data.Replace(".0000", ""));
                        var price = long.Parse(product.Price);
                        if (money >= price)
                        {

                            var messager = string.Format(Resource._1000057, string.Format(new CultureInfo("en-US"), "{0:0,0}", long.Parse(product.Number)), Resource._1000063, product.EndDate, string.Format(new CultureInfo("en-US"), "{0:0,0}", long.Parse(product.Price)));
                            var res = await _pageDialogService.DisplayAlertAsync(Resource._1000035, messager, "OK",
                                "Cancel");
                            if (res)
                            {
                                if (moneyModel != null && moneyModel.Code > 0 && moneyModel.Data != null)
                                {

                                    var registerProduct = await _productService.RegisterProduct(product.ID, idUser);
                                    if (registerProduct != null && registerProduct.Code > 0 && registerProduct.Data != null)
                                    {

                                        var updateUser = await _userService.SetMoneyUser(username, money - price + "");
                                        if (updateUser != null && updateUser.Code > 0)
                                        {
                                            await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000040,
                                                "OK");
                                            await InitializeData();
                                            await _productService.AddHistoryUseService(product.ID,"Tăng mặt livestream",user.ID,"0",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
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
                        if (item.GroupProduct == "2")
                        {
                            item.TitleProduct= $"Buff { item.Number} {Resource._1000087} / {Resource._1000088} {Resource._1000089} { item.EndDate} {Resource._1000088}";
                            item.Icon = "icon_eye_view.png";
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
                                    }
                                }
                            }
                            product.Add(item);
                        }
                    }

                    ProductData = new List<ProductModel>(product.OrderBy(x=>int.Parse(x.EndDate)));
                }

            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}