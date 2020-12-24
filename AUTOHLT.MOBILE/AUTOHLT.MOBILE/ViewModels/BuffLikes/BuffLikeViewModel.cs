using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.Product;
using AUTOHLT.MOBILE.Services.Product;
using System.Collections.Generic;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.User;

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

        public BuffLikeViewModel(INavigationService navigationService, IProductService productService, IUserService userService, IDatabaseService databaseService) : base(navigationService)
        {
            _databaseService = databaseService;
            _userService = userService;
            _productService = productService;
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
                    var lsProduct = (List<BuffLikeModel>)data.Data;
                    foreach (var item in lsProduct)
                    {
                        if (item.GroupProduct == "1")
                        {
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
                                    }
                                }
                            }
                            product.Add(item);
                        }
                    }
                    
                    ProductData = new ObservableCollection<ProductModel>(product);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}