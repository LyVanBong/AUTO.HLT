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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.BuffLikes
{
    public class BuffLikeViewModel : ViewModelBase
    {
        private bool _isLoading;
        private IProductService _productService;
        private ObservableCollection<ProductModel> _productData;
        private IUserService _userService;
        private IDatabaseService _databaseService;
        private List<ListRegisterProductModel> _regis;
        private IPageDialogService _pageDialogService;
        private IDialogService _dialogService;

        public ICommand BuffLikeCommand { get; private set; }
        public ObservableCollection<ProductModel> ProductData
        {
            get => _productData;
            set => SetProperty(ref _productData, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public BuffLikeViewModel(INavigationService navigationService, IProductService productService, IUserService userService, IDatabaseService databaseService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            _pageDialogService = pageDialogService;
            _databaseService = databaseService;
            _userService = userService;
            _productService = productService;
            BuffLikeCommand = new Command<ProductModel>(BuffLike);
        }

        private async void BuffLike(ProductModel obj)
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                var user = await _databaseService.GetAccountUser();
                if (obj != null)
                {
                    if (obj.IsRegisterProduct)
                    {
                        var id = user.ID;
                        var number = int.Parse(obj.Number);
                        var num = 0;
                        var data = await _productService.GetHistoryUseServiceForUser(id);
                        if (data != null && data.Code > 0 && data.Data != null && data.Data.Any())
                        {
                            var lsHistoryUserService = data.Data.ToList()
                                .Where(x => x.ID_ProductType == obj.ID &&
                                            DateTime.Parse(x.DateCreate).Date == DateTime.Now.Date).ToList();
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
                            para.Add("IdProduct", obj.ID);
                            para.Add("IdUser", id);
                            para.Add("Number", (number - num).ToString());
                            para.Add("Title", Resource._1000029);
                            await _dialogService.ShowDialogAsync(nameof(UseServiceDialog), para);
                        }
                    }
                    else
                    {
                        var username = user.UserName;
                        var idUser = user.ID;
                        var moneyModel = await _userService.GetMoneyUser(username);
                        if (moneyModel != null && moneyModel.Code > 0 && moneyModel.Data != null)
                        {
                            var money = long.Parse(moneyModel.Data.Replace(".0000", ""));
                            var price = long.Parse(obj.Price);
                            if (money >= price)
                            {
                                var messager = string.Format(Resource._1000057,
                                    string.Format(new CultureInfo("en-US"), "{0:0,0}", long.Parse(obj.Number)), "Like",
                                    $"{Resource._1000089} {obj.EndDate} {Resource._1000088}",
                                    string.Format(new CultureInfo("en-US"), "{0:0,0}", long.Parse(obj.Price)));
                                var res = await _pageDialogService.DisplayAlertAsync(Resource._1000035, messager, "OK",
                                    "Cancel");
                                if (res)
                                {
                                    var registerProduct = await _productService.RegisterProduct(obj.ID, idUser);
                                    if (registerProduct != null && registerProduct.Code > 0 && registerProduct.Data != null)
                                    {

                                        var updateUser = await _userService.SetMoneyUser(username, money - price + "");
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
                            else
                            {
                                await _pageDialogService.DisplayAlertAsync(Resource._1000035, moneyModel.Message, "OK");
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
                        if (item.GroupProduct == "1")
                        {
                            item.TitleProduct = $"Buff {item.Number} like / {Resource._1000088} {Resource._1000089} {item.EndDate} {Resource._1000088}";
                            item.Icon = "icon_like_product.png";
                            if (_regis != null && _regis.Any())
                            {
                                var obj = _regis.FirstOrDefault(x => x.ID_ProductType == item.ID);
                                if (obj != null)
                                {
                                    var endDate = DateTime.Parse(obj.DateCreate);
                                    var totalDay = (DateTime.Now - endDate).TotalDays;
                                    var number = double.Parse(item.Number);
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

                    ProductData = new ObservableCollection<ProductModel>(product.OrderBy(x => int.Parse(x.EndDate)));
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}